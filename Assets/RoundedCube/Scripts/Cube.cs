using System.Collections;
using UnityEngine;

[RequireComponent(typeof (MeshRenderer), typeof (MeshFilter))]
public class Cube : MonoBehaviour{
    public int xSize, ySize, zSize;
    private Mesh mesh;
    private Vector3[] vertices;
    
    void Awake(){
        StartCoroutine(Generate());
    }
    private IEnumerator Generate(){
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        int corners = 8;
        int edgeVertices = 4 * (xSize + ySize + zSize - 3);
        int faceVertices = 2 * ((xSize-1) * (ySize-1) + (ySize-1) * (zSize-1) + (xSize-1) * (zSize-1));
        vertices = new Vector3[corners+edgeVertices+faceVertices];

        int v = 0;
        for (int x = 0; x <= xSize; x++){
            vertices[v++] = new Vector3(x,0,0);
        }
        for (int z = 1; z <= zSize; z++) {
			vertices[v++] = new Vector3(xSize, 0, z);
			yield return wait;
		}
		for (int x = xSize - 1; x >= 0; x--) {
			vertices[v++] = new Vector3(x, 0, zSize);
			yield return wait;
		}
		for (int z = zSize - 1; z > 0; z--) {
			vertices[v++] = new Vector3(0, 0, z);
			yield return wait;
		}
        yield return wait;

    }
    // Update is called once per frame
    private void OnDrawGizmos(){
        if (vertices == null){
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++){
            Gizmos.DrawSphere(vertices[i],0.1f);
        }
    }
}
