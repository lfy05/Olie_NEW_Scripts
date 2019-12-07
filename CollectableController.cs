using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour{
    // procedural generation system required attributes
    public float InstantiateAtHeight;

    // visual effect system
    float displaceAmount;
    //public float VertexDisplaceWeight = 0.5f;
    MeshRenderer meshRenderer;

    // health system
    public int healthEffect;

    // Start is called before the first frame update
    void Start(){
        // initialize all references
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update(){
        displaceAmount = Mathf.Lerp(displaceAmount, 0, Time.deltaTime);
        meshRenderer.material.SetFloat("_Amount", displaceAmount);

        // destroy if falls under the terrain
        float terrainY = GameObject.FindGameObjectWithTag("Terrain").transform.position.y;
        if (transform.position.y < terrainY)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player")
            displaceAmount = 1f;
    }

    private void OnBecameInvisible() {
        gameObject.SetActive(false);
    }
}
