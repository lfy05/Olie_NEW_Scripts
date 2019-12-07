using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUV : MonoBehaviour{

    // get assigned mesh and relative properties
    Mesh mesh;
    Vector3[] vertices;

    // UV array
    Vector2[] UVs;


    // Start is called before the first frame update
    void Start(){
        // initialize vertices
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        // initialize UV array
        UVs = new Vector2[vertices.Length];

        // assign UV coordinates
        for (int vertexCntr = 0; vertexCntr < vertices.Length; vertexCntr++) {
            UVs[vertexCntr] = new Vector2(vertices[vertexCntr].x, vertices[vertexCntr].z);
        }

        // update UVs to the mesh
        mesh.uv = UVs;

    }

    // Update is called once per frame
    void Update(){
        
    }
}
