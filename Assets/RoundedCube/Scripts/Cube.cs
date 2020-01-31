using System.Collections;
using UnityEngine;

[RequireComponent(typeof (MeshRenderer), typeof (MeshFilter))]
public class Cube : MonoBehaviour{
    
    public int xSize, ySize; //, zSize;
    private Mesh mesh;
    private Vector3[] vertices;
    
    void Awake(){
        Generate();
    }
    private void Generate(){

        WaitForSeconds wait = new WaitForSeconds(0.5f);
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";

        // int corners = 8;
        // int edgeVertices = 4 * (xSize + ySize + zSize - 3);
        // int faceVertices = 2 * ((xSize-1) * (ySize-1) + (ySize-1) * (zSize-1) + (xSize-1) * (zSize-1));
        // vertices = new Vector3[corners+edgeVertices+faceVertices];

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        // int v = 0;
        // for (int x = 0; x <= xSize; x++){
        //     vertices[v++] = new Vector3(x,0,0);
        // }
        // for (int z = 1; z <= zSize; z++) {
		// 	vertices[v++] = new Vector3(xSize, 0, z);
		// 	yield return wait;
		// }
		// for (int x = xSize - 1; x >= 0; x--) {
		// 	vertices[v++] = new Vector3(x, 0, zSize);
		// 	yield return wait;
		// }
		// for (int z = zSize - 1; z > 0; z--) {
		// 	vertices[v++] = new Vector3(0, 0, z);
		// 	yield return wait;
		// }
        // yield return wait;
        for (int i = 0, y = 0; y <= ySize; y++) {
            for (int x = 0; x <= xSize; x++,i++) {
                vertices[i] = new Vector3(x,y);
                uv[i] = new Vector2((float)x / xSize, (float)y /ySize);
                tangents[i] = tangent;
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
            for (int x = 0; x <xSize; x++, ti+= 6, vi++){
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;      
            }       
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
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
