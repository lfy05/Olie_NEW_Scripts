using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour{
    // create variables
    float displaceAmount;
    public float displaceWeight = 0.5f;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start(){
        // initialize all references
        meshRenderer = GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update(){
        displaceAmount = Mathf.Lerp(displaceAmount, 0, Time.deltaTime);
        meshRenderer.material.SetFloat("_Amount", displaceAmount);

        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Btn down");
            displaceAmount = displaceWeight;
        }
    }
}
