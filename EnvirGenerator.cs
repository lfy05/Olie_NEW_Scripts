using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class EnvirGenerator : MonoBehaviour
{
    // terrain mesh generation
    Mesh mesh;
    [HideInInspector] public Vector3[] vertices;
    int[] triangles;

    public int xSize;
    public int zSize;
    Vector2[] UVs;

    int maxVisZPos = 0;     // this is the furtherest Z position the camera can see
    int furstZPos = 0;      // this is the furtherest Z position of the mesh -- The Z position of the end of the mesh

    // camera
    Camera camera;
    float renderingDist;
    Vector3 cameraPos;

    // collider
    BoxCollider boxCollider;

    /* collectable generator */
    public GameObject collectable1;
    //public GameObject collectable2;
    //public GameObject collectable3;
    //public GameObject collectable4;
    public int numberOfGeneration;

    public float collectableGenerationHeight = 3;

    // hidden public variable

    // Start is called before the first frame update
    void Start(){

        

        // creating a mesh object and add it to the mesh filter
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // get camera reference
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // get rendering distance
        renderingDist = camera.farClipPlane;

        // for procedural generation to work properly, 
        // correct zSize input to make it at least 30 units
        // bigger then the farClipPlane
        if (zSize <= renderingDist)
            zSize = (int)(renderingDist + 30f);

        // update furtherest Z position -> camera's position + rendering distance
        maxVisZPos = (int)(cameraPos.z + renderingDist);

        // get box collider reference
        boxCollider = GetComponent<BoxCollider>();

        // set collider size
        boxCollider.size = new Vector3(xSize, boxCollider.size.y, zSize);

        // update mesh
        GenerateMesh(0);
    }

    private void Update() {
        // find player position
        //Vector3 playerPos = player.transform.position;

        // find camera position
        cameraPos = camera.transform.position;

        // update furtherest Z position -> camera's position + rendering distance
        maxVisZPos = (int)(cameraPos.z + renderingDist);
        
        // determine if start generating more mesh -- If the camera can see beyond the end of the mesh, generate more terrain
        if (maxVisZPos >= furstZPos)
            GenerateMesh(furstZPos);
    }

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

        // update box collider center
        Vector3 centerOfMesh = new Vector3(xSize / 2, 0, furstZPos - (zSize / 2));
        boxCollider.center = centerOfMesh;

        // UV mapping
        mesh.uv = UVs;

        // Everytime the mesh is updated, new collectables are generated
        GameObject[] collectables = { collectable1 };
        GenerateCollectables(collectables, numberOfGeneration);

        
    }

    void GenerateCollectables(GameObject collectable) {
        int randomIndex = Random.Range(0, vertices.Length);
        Instantiate(collectable, vertices[randomIndex], Quaternion.identity);
    }

    void GenerateCollectables(GameObject collectable, Vector3 position) {
        Instantiate(collectable, position, Quaternion.identity);
    }

    void GenerateCollectables(GameObject collectable, int num) {
        for (int i = 0; i < num; i++) {
            int randomIndex = Random.Range(0, vertices.Length);
            Instantiate(collectable, vertices[randomIndex], Quaternion.identity);

        }
    }

    void GenerateCollectables(GameObject[] collectables, int num) {
        for (int numCounter = 0; numCounter < num; numCounter++) {
            if (Random.Range(0.0f, 1.0f) >= 0.5) {                                                              // randomly decided if generate this collectable or not.
                // see if the height was specified on the prefab
                GameObject objectToGen = collectables[Random.Range(0, collectables.Length)];
                float objectSpecHeight = objectToGen.GetComponent<CollectableController>().InstantiateAtHeight;
                if (objectSpecHeight >= 0)
                    collectableGenerationHeight = objectSpecHeight;

                Vector3 randomVertex = vertices[Random.Range(0, vertices.Length)];
                Vector3 randomPos = new Vector3(randomVertex.x, collectableGenerationHeight, randomVertex.z);                            // deriving a random position
                Instantiate(objectToGen,
                    randomPos,
                    Quaternion.identity);                                                                       // randomly decided which one to generate and which vertex to generate on
                Debug.Log("Generation Complete");
            } else
                num++;              // If not generated, add one to num to ensure the total number of generation remains the same
        }
    }
}
