using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CenterPeice {
    // private static GameObject centerPeice = GameObject.Find("CenterPeice");
    private static GameObject centerPeice;
    private static string centerPeiceName = "CenterPeice";
    private static Mesh mesh;
    private static Vector3[] vertices;
    private static List<int> triangles;
    // private static int[] triangles;

    private static Vector3 cellScale = Resources.Load<GameObject>("PreFabs/Cell").transform.localScale;
    private static int pixPerScale = 1;

    [MenuItem("Tools/CenterPeice")]
    public static void CreateCenterPeice() {
        GameObject centerPeiceOld = GameObject.Find(centerPeiceName); 
        if (centerPeiceOld != null) {
            GameObject.DestroyImmediate(centerPeiceOld);
        }
        
        centerPeice = new GameObject();
        mesh = new Mesh();
        
        centerPeice.name = centerPeiceName;
        centerPeice.AddComponent<MeshFilter>().mesh = mesh;    
        centerPeice.AddComponent<MeshRenderer>();
        // centerPeice.GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
    }

    private static void CreateShape() {
        // Using 3 because 3 cubes edges constitute center peice
        // Here pixPerScale may play no role as apparantly in unity its 1
        float width = cellScale[0] * pixPerScale * 3; 
        float height = cellScale[1] * pixPerScale; 
        float depth = cellScale[2] * pixPerScale * 3; 

        // vertices = new Vector3[] {
        //     // This is Base
        //     new Vector3(0, 0, 0), 
        //     new Vector3(0, 0, depth), 
        //     new Vector3(width, 0, depth), 
        //     new Vector3(width, 0, 0), 

        //     // This is Top
        //     new Vector3(0, height, 0), 
        //     new Vector3(0, height, depth), 
        //     new Vector3(width, height, depth), 
        //     new Vector3(width, height, 0)
        // };

        // triangles = new int[] {
        //     // Bottom Face
        //     0, 1, 2,
        //     2, 3, 0,

        //     // Top Face
        //     4, 5, 6,
        //     6, 7, 4
        // };
        
        int n = 4;
        vertices = PolygonPoints(n, 3, 2, 0, 0).ToArray();

        // 0 index contains center

        triangles = new List<int>();
        // triangles.AddRange(new List<int>() {0, 1, 2});
        // triangles.AddRange(new List<int>() {0, 3, 4});
        for (int i = 1; i <= n; i++) {
            triangles.AddRange(new List<int>() {0, i, (i) % (n) + 1});
            Debug.Log(i);
            Debug.Log((i + 1) % (n + 1));
        }
        // triangles.AddRange(new List<int>() {0, 0, 4});
        // triangles.AddRange(new List<int>() {0, 1, 2});
        // Debug.Log(vertices[0]);
        // Debug.Log(vertices[1]);
        // Debug.Log(vertices[2]);
        // Debug.Log(vertices[3]);
        // Debug.Log(vertices[4]);

        updateMesh();
    }

    // x and y are centers
    public static List<Vector3> PolygonPoints(int n, float a, float height, int x, int z) {
        float theta = (float) (2 * Mathf.PI) / (float)n;
        // Debug.Log("tan theta is " + Mathf.Tan(theta).ToString());
        float r = (float)(a / (2.0 * Mathf.Tan(theta / 2)));

        List<Vector3> polygonPoints = new List<Vector3>();
        polygonPoints.Add(new Vector3(x, height, z));
        for (int i = 0; i < n; i++) {
            polygonPoints.Add(new Vector3(
                z + r * Mathf.Sin(theta * i),
                height,
                x + r * Mathf.Cos(theta * i)
            ));
        }
        
        return polygonPoints;
    }
    // // sqList must be clockwise for it to work
    // private static List<int> SqPoints2TrPonints(List<int> sqList) {
    //     List<int> trList = new List<int>();
    //     trList.AddRange(new List<int>() { sqList[0], sqList[1], sqList[2]}); 
    //     trList.AddRange(new List<int>() { sqList[2], sqList[3], sqList[0]}); 
    // // }

    private static void updateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
    }
}
