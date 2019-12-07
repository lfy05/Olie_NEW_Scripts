using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBodyVisualController : MonoBehaviour
{
    // get material and collider
    MeshCollider meshCollider;
    Material animalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // get mesh collider reference
        meshCollider = GetComponent<MeshCollider>();

        // get material reference
        animalMaterial = GetComponent<SkinnedMeshRenderer>().material;

        // subscribe to events
        PlayerController.takeDamage += OnHealthUpdated;
        PlayerController.gainHealth += OnHealthUpdated;
    }

    private void OnCollisionEnter(Collision collision) {
        // Debug Msg
        Debug.Log("Collision");

        animalMaterial.SetFloat("_CollisionVisibility", 1f);

        // set the first collision contact as the center for the sphere mask
        animalMaterial.SetVector("_CollisionCenter",
            collision.GetContact(0).point);
    }

    void OnHealthUpdated(int healthAdjust) {
        // if lossing health
        if (healthAdjust <= 0) { }

        // if gaining health
        if (healthAdjust > 0) { }

    }
}
