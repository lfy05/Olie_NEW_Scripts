using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFeetCollision : MonoBehaviour
{
    // collider 
    BoxCollider boxCollider;

    // footprint prefab reference
    public GameObject ftptPrb;

    // Start is called before the first frame update
    void Start()
    {
        // get box collider reference
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision) {
        // Instantiate prefab footprint at collision center
        Instantiate(ftptPrb,
            collision.GetContact(0).point,
            Quaternion.identity);
    }
}
