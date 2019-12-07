using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SuperCollectGenerator : MonoBehaviour
{
    Dictionary<string, SuperCollectable> superCollectOptions = new Dictionary<string, SuperCollectable>();
    Dictionary<string, GameObject> spcPrefabs = new Dictionary<string, GameObject>();
    ArrayList spCKeyCatalogue = new ArrayList();
    ArrayList votesAList = new ArrayList();

    public string DatabaseLocationURL;

    public GameObject player;

    public GameObject SuperCollectable_1;
    public GameObject SuperCollectable_2;
    public float PollingInterval;
    public float PollingWaitTime;
    public float FetchVoteCountWaitTime;
    private bool isPolling = false;

    public Text pollingStatusIndicator;

    // currently fetched score
    int tempVoteCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        // get polling status indicator referene 
        //Text[] textBoxes = GameObject.FindGameObjectWithTag("UI").GetComponentsInChildren<Text>();
        //pollingStatusIndicator = textBoxes[2];

        // adding options to the dictionary and key catalogue
        AddSuperCollectable(SuperCollectable_1);
        AddSuperCollectable(SuperCollectable_2);

        StartCoroutine(pollingManager());

    }

    void AddSuperCollectable(GameObject SuperCollectable) {
        // adding to dictionary
        superCollectOptions.Add(SuperCollectable.GetComponent<SuperCollectable>().optionKey,
            SuperCollectable.GetComponent<SuperCollectable>());

        spcPrefabs.Add(SuperCollectable.GetComponent<SuperCollectable>().optionKey, SuperCollectable);

        // adding to catalogue
        spCKeyCatalogue.Add(SuperCollectable.GetComponent<SuperCollectable>().optionKey);

        // update to firebaseRTDatabase
        string fullURL = DatabaseLocationURL + "/OPTIONS/" + SuperCollectable.GetComponent<SuperCollectable>().optionKey + ".json";
        RestClient.Put(fullURL, SuperCollectable.GetComponent<SuperCollectable>());

    }

    void getVoteCnts (string key) {
        
        string voteCntURL = DatabaseLocationURL + "/OPTIONS/" + key + "/voteCnt.json";
        RestClient.Get(voteCntURL).Then(response => {
            Debug.Log("Fetched Params" + response.Text);
            // try parsing string to int
            try {
                votesAList.Add(Int32.Parse(response.Text.ToString()));
            } catch (FormatException e) {
                Debug.Log("Parse Failed");
                votesAList.Add(0);
            }

        });
    }
    
    void TurnOffVoting() {
        string voteSwtichURL = DatabaseLocationURL + "/SWITCH.json";
        RestClient.Put(voteSwtichURL, "false");
        pollingStatusIndicator.text = "Polling Off";
    }

    void TurnOnVoting() {
        string voteSwtichURL = DatabaseLocationURL + "/SWITCH.json";
        RestClient.Put(voteSwtichURL, "true");
        pollingStatusIndicator.text = "Polling On";
    }

    void ResetVotes() {
        // iterate through every key
        for (int i = 0; i < spCKeyCatalogue.Count; i++) {
            SuperCollectable spc = superCollectOptions[spCKeyCatalogue[i].ToString()];
            string voteCntURL = DatabaseLocationURL + "/OPTIONS/" + spc.GetComponent<SuperCollectable>().optionKey + "/voteCnt.json";
            RestClient.Put(voteCntURL, "0");
        }
    }

    IEnumerator pollingManager() {
        while (!player.GetComponent<PlayerMotor>().isDead) {
            if (!isPolling) {
                yield return new WaitForSeconds(PollingInterval);
                StartCoroutine(polling());
            } else yield return null;
        }
    }

    IEnumerator polling() {

        isPolling = true;

        TurnOnVoting();

        yield return new WaitForSeconds(PollingWaitTime);

        TurnOffVoting();

        // iterate through every key
        for (int i = 0; i < spCKeyCatalogue.Count; i++) {
            getVoteCnts(spCKeyCatalogue[i].ToString());
            yield return new WaitForSeconds(FetchVoteCountWaitTime);
        }

        // find the largest vote number
        int largestVoteCnt = 0;
        for (int i = 0; i < votesAList.Count; i++) {
            if ((int)votesAList[i] > largestVoteCnt)
                largestVoteCnt = (int)votesAList[i];
        }

        // get the index of that count
        int indexOfSC = votesAList.IndexOf(largestVoteCnt);

        // get the key to of the index
        string keyOfSPC = spCKeyCatalogue[indexOfSC].ToString();

        // find the game object
        GameObject winningSPC = spcPrefabs[keyOfSPC];
        pollingStatusIndicator.text = "Chosen SuperCollectable: " + winningSPC.GetComponent<SuperCollectable>().optionName;

        yield return new WaitForSeconds(0.5f);

        // get vertices
        Vector3[] vertices = GameObject.FindGameObjectWithTag("Terrain").GetComponent<EnvirGenerator>().vertices;
        Vector3 randomVertex = vertices[UnityEngine.Random.Range(0, vertices.Length)];
        Vector3 randomPos = new Vector3(randomVertex.x,
            winningSPC.GetComponent<CollectableController>().InstantiateAtHeight,
            randomVertex.z);

        // instantiate that
        Instantiate(winningSPC, randomPos, Quaternion.identity);

        // cleanup
        votesAList.Clear();
        ResetVotes();

        isPolling = false;
        
    }
}
