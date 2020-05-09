using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
public class CenterPeice {
    // private static GameObject centerPeice = GameObject.Find("CenterPeice");

    private static int n = 8;
    private static GameObject centerPeice;
    private static string centerPeiceName = "CenterPeice";
    private static Mesh mesh;
    private static List<Vector3> vertices;
    private static List<int> triangles;

    // private static Vector3 cellScale = Resources.Load<GameObject>("PreFabs/Cell").transform.localScale;
    private static Vector3 cellScale = new Vector3(1, 1, 1);
    private static int pixPerScale = 1;

    // [MenuItem("Tools/CenterPeice")]
    public static void CreateCenterPeice() {
        GameObject centerPeiceOld = GameObject.Find(centerPeiceName); 
        if (centerPeiceOld != null) {
            GameObject.DestroyImmediate(centerPeiceOld);
        }
        
        centerPeice = new GameObject();
        mesh = new Mesh();
        
        centerPeice.name = centerPeiceName;
        MeshFilter centerPeiceMf = centerPeice.AddComponent<MeshFilter>(); 
        MeshRenderer centerPeiceMr = centerPeice.AddComponent<MeshRenderer>();
        
        centerPeiceMf.sharedMesh = mesh;
        // centerPeice.GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();

        centerPeice.transform.localScale = new Vector3(1.2f, 0.0f, 1.2f);
    }

    private static void CreateShape() {
        // Using 3 because 3 cubes edges constitute center peice
        // Here pixPerScale may play no role as apparantly in unity its 1
        float width = cellScale[0] * pixPerScale * 3; 
        float height = cellScale[1] * pixPerScale; 
        float depth = cellScale[2] * pixPerScale * 3; 

        vertices = PolygonPoints(n, 3, height, 0, 0);
        Debug.Log(vertices[1].y);
        // vertices.AddRange(PolygonPoints(n, 3, (int)height, 0, 0));
        // 0 index contains center

        triangles = new List<int>();

        // let h represents adding heights
        // so that for h objects can be called by adding n
        for (int i = 1; i <= n; i++) {
            int p1 = i;
            int p2 = i % n + 1;

            // Adding Base
            triangles.AddRange(new List<int>() {0, p1, p2});

            // // Adding Top
            // triangles.AddRange(new List<int>() {n + 1, p1 + n + 1, p2 + n + 1});

            // List<int> trList = new List<int>() {p1, p2, p2 + n + 1, p1 + n + 1}; 
            // triangles.AddRange(SqPoints2TrPoints(trList));

        }
        UpdateMesh();
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
    
    // sqList must be clockwise for it to work
    private static List<int> SqPoints2TrPoints(List<int> sqList) {
        List<int> trList = new List<int>();
        trList.AddRange(new List<int>() { sqList[0], sqList[1], sqList[2]}); 
        trList.AddRange(new List<int>() { sqList[2], sqList[3], sqList[0]}); 

        return trList;
    }

    private static void UpdateMesh() {
        // mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
