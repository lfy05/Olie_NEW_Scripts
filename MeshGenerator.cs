using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    // terrain mesh generation
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 17;
    public int zSize = 17;
    Vector2[] UVs;

    int maxVisZPos = 0;     // this is the furtherest Z position the camera can see
    int furstZPos = 0;      // this is the furtherest Z position of the mesh -- The Z position of the end of the mesh

    // player
    GameObject player;

    // camera
    Camera camera;
    float renderingDist;
    Vector3 cameraPos;



    // Start is called before the first frame update
    void Start(){
        // creating a mesh object and add it to the mesh filter
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // update mesh
        GenerateMesh(0);

        // get player reference
        player = GameObject.FindGameObjectWithTag("Player");

        // get camera reference
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // get rendering distance
        renderingDist = camera.farClipPlane;
    }

    private void Update() {
        // find player position
        Vector3 playerPos = player.transform.position;

        // find camera position
        cameraPos = camera.transform.position;

        // update furtherest Z position -> camera's position + rendering distance
        maxVisZPos = (int)(cameraPos.z + renderingDist);
        
        // determine if start generating more mesh -- If the camera can see beyond the end of the mesh, generate more terrain
        if (maxVisZPos >= furstZPos)
            GenerateMesh(furstZPos);

    }

    /* void CreateShape(int startingZ) {

        // create vertex array
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        // iterate through every vertex and assign them a position
        for (int z = 0, i = 0; z < zSize + 1; z++) {
            for (int x = 0; x < xSize + 1; x++) {
                vertices[i] = new Vector3(x, 0, z + startingZ);
                i++;
            }
        }

        // vertex currently looking at
        int vert = 0;
        // the triangle currently looking at
        int tris = 0;

        triangles = new int[xSize * zSize * 6];

        // iterate through z
        for (int z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++) {
                // create 2 triangles
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        // define UVs
        UVs = new Vector2[vertices.Length];
        for (int z = 0, i = 0; z < zSize + 1; z++) {
            for (int x = 0; x < xSize + 1; x++) {
                UVs[i] = new Vector2((float)x, (float)z);
                i++;
            }
        }
    }

    void UpdateMesh() {
        // clear mesh from previous data
        mesh.Clear();

        // update triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        // UV mapping
        mesh.uv = UVs;
    } */

    void GenerateMesh(int startingZ) {
        /* create shape */

        // create vertex array
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        // iterate through every vertex and assign them a position
        for (int z = 0, i = 0; z < zSize + 1; z++) {
            for (int x = 0; x < xSize + 1; x++) {
                vertices[i] = new Vector3(x, 0, z + cameraPos.z);       // the beginning of the new terrain should be the camera position
                i++;
            }
        }

        // vertex currently looking at
        int vert = 0;
        // the triangle currently looking at
        int tris = 0;

        triangles = new int[xSize * zSize * 6];

        // iterate through z
        for (int z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++) {
                // create 2 triangles
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        // define UVs
        UVs = new Vector2[vertices.Length];
        for (int z = 0, i = 0; z < zSize + 1; z++) {
            for (int x = 0; x < xSize + 1; x++) {
                UVs[i] = new Vector2((float)x, (float)z);
                i++;
            }
        }

        /* update mesh */

        // clear mesh from previous data
        mesh.Clear();

        // update triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        // update furstZPos
        furstZPos = (int) (zSize + cameraPos.z);            // update furtherest Z pos for terrain. That would be the Z size of the newly generated terrain 
                                                            // plus the beginning of the terrain -- aka the cameraPos.z

        // UV mapping
        mesh.uv = UVs;
    }
}
