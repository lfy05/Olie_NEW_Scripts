using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGenerator : MonoBehaviour
{
    // fields for attack generation
    public float attackInterval;
    public float attackForceFactor;
    public float attackVariationX;
    public float attackVariationY;
    public float attackVariationZ;
    public float attackProjectileDistanceZ;
    public GameObject attackPrefab;

    // fields for position update
    GameObject camera;
    public GameObject player;
    public GameObject playerBody;

    // Start is called before the first frame update
    void Start()
    {
        // find camera reference
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        // find player reference
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(GenerateAttck(attackPrefab));
    }

    private void Update() {
        gameObject.GetComponent<Transform>().position = new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
    }

    IEnumerator GenerateAttck(GameObject atkPfb) {
        // when the game is still going on
        while (playerBody.GetComponent<PlayerController>().health > 0) {
            // straight line between generator and player
            float directX = player.transform.position.x - transform.position.x;
            float directY = player.transform.position.y - transform.position.y;
            float directZ = player.transform.position.z - transform.position.z;

            // take in variation and generate random force Vector3
            // Random.InitState((int)System.DateTime.Now.Millisecond);
            Vector3 forceTrfm = new Vector3(
                directX + Random.Range(-attackVariationX, attackVariationX),
                directY + Random.Range(-attackVariationY, attackVariationY),
                directZ + Random.Range(-2f, attackVariationZ));

            // instantiate prefab
            Instantiate(atkPfb, gameObject.transform.position, Quaternion.identity)
                .GetComponent<Rigidbody>().AddForce(forceTrfm * attackForceFactor, ForceMode.Impulse);

            // apply attack interval
            yield return new WaitForSeconds(attackInterval);
        }

        
    }
}
