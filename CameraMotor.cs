using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    // fields for camera movement;
    GameObject player;
    Transform playerTrfm;

    Transform cameraTrfm;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // get references
        player = GameObject.FindGameObjectWithTag("Player");
        playerTrfm = player.GetComponent<Transform>();

        cameraTrfm = GetComponent<Transform>();

        // get initial offset
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() {
        transform.position = player.transform.position + offset;
    }
}
