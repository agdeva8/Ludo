using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// custom functions

public class CreateBoard {
    private static float a = 1;
    private static int n = 8;
    private static float theta = (360 / n);
private static Vector3 cellScale = new Vector3(1, 1, 1);

    [MenuItem("Tools/Create Board")]
    public static void createRect() {
        
        GameObject[] rectParents = new GameObject[n];
        // GameObject firstCube = InstNewCell(6, 0, 0);
        // GameObject secCube = InstNewCell(1, 0, 0, firstCube);
        // secCube.transform.localPosition = new Vector3(1, 0, 0);
        // secCube.transform.parent = firstCube.transform;
        // secCube.transform.position = new Vector3(1, 0, 0);
        // GameObject[,] Cells = CreateGrid(6, 3, firstCube);
        // GameObject.DestroyImmediate(Cells[0, 0]);
        // Cells[0, 0] = firstCube;

        List<GameObject[,]> Cells = new List<GameObject[,]>();
        for (int i = 0; i < n; i++) {
            rectParents[i] = new GameObject("Rect Parent " + i.ToString());
            if (i >= 1) {
                rectParents[i].transform.parent = rectParents[i - 1].transform;
                // rectParents[i].transform.Rotate(new Vector3(0, 45, 0));
            }

            rectParents[i].transform.localPosition = new Vector3(3 * a, 0, 0);
            Cells.Add(CreateGrid(3, 6, rectParents[i]));
            
            if (i >= 1) {
                rectParents[i].transform.localRotation = Quaternion.Euler(0, theta, 0);
            }
        }

        // GameObject c1 = InstNewCell(0, 0, 0);
        // GameObject c2 = InstNewCell(0, 0, 0);

        // float theta = Mathf.PI / 4;

        // // c1.transform.Translate(new Vector3(-a, 0, 0));
        // // GameObject c2Parent = InstNewCell(a / 2, 0, a / 2);
        // // GameObject c2Parent = new GameObject();
        // // c2Parent.transform.Translate(new Vector3(a / 2, 0, a / 2));
        // // c2Parent.transform.Rotate(new Vector3(0, 45, 0));
        // // c2.transform.SetParent(c2Parent.transform);
        
        // c2.transform.Translate(new Vector3(1, 0, 0));

        // SetPivot setPivot = new SetPivot();
        // setPivot.p = new Vector3(-1.0f, 0.0f, 1.0f);
        // setPivot.SetPivotObject(c2);
        // setPivot.UpdatePivot();

        // Vector3 cornerVertex = new Vector3(1, 0, 0);
        // c2.transform.Translate(cornerVertex);
        // c2.transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0f, 1f, 0f), 90);

        // c2.transform.Rotate(new Vector3(0, 45, 0));

        // // float dx = a * Mathf.Sin(theta); 
        // // c2.transform.Translate(new Vector3(1 + dx, 0, 0));

        // Vector3 newPoint = FindPointLDist(new Vector3(-Mathf.Sin(theta), 0, Mathf.Cos(theta)), Mathf.Tan(theta), a / 2);
        // float dx = Mathf.Abs(-a / 2 - newPoint[0]);
        // float dy = Mathf.Abs(-a / 2 - newPoint[2]);

        // c2.transform.Translate(new Vector3(1 + dx, 0, dy));
        
        // GameObject firstCube = InstNewCell(0, 0, 0);
        // GameObject[,] Cells = CreateGrid(6, 3, firstCube);
        // GameObject.DestroyImmediate(Cells[0, 0]);
        // Cells[0, 0] = firstCube;
        
        // List<Vector3> vertices = CenterPeice.PolygonPoints(n, 3 * a, 0, 0, 0);

        // Debug.Log(vertices[1] + "\t" + vertices[2]);

        // float m = GetSlope(vertices[1], vertices[2]);

        // Debug.Log("m = " + m);
        // Vector3 intermediateVertex = FindPointLDist(vertices[1], m, a / 2); 
        // Vector3 firstCubeCenter = FindPointLDist(intermediateVertex, (-1 / m), a / 2); 

        // GameObject firstCell = InstNewCell(firstCubeCenter);
        // InstNewCell(vertices[2]);
    }

    
    public static GameObject[,] CreateGrid(int numRows, int numCols, GameObject parent = null) {
        // GameObject myFirstCell = (GameObject)GameObject.Instantiate(Resources.Load("PreFabs/Cell"),
        //                     new Vector3(-4, 0, 0), Quaternion.identity);

        GameObject[,] Cells = new GameObject[numRows, numCols];
        for (int i = 0; i < numRows; i++) {
            for (int j = 0; j < numCols; j++) {
                Cells[i, j] = InstNewCell(0, 0, 0, parent);

                Cells[i, j].transform.localPosition = 
                                new Vector3(i * cellScale[2] + a / 2, 0, j * cellScale[0] + a / 2); 
                // Cells[i, j].name = "Cells_" + i.ToString() + "_" + j.ToString(); 
            }
        }

        return Cells;
    }

    private static GameObject InstNewCell(float x, float y, float z, GameObject parent = null) { 
        GameObject newCell;
        if (parent == null) {
            newCell = (GameObject)GameObject.Instantiate(Resources.Load("PreFabs/Cell"),
                                        new Vector3(x, y, z),
                                        Quaternion.identity);
        }
        else {
            newCell = (GameObject)GameObject.Instantiate(Resources.Load("PreFabs/Cell"),
                                        new Vector3(x, y, z),
                                        Quaternion.identity, parent.transform);
        }
        return newCell;
    }

    private static GameObject InstNewCell(Vector3 point, GameObject parent = null) { 
        float x = point[0];
        float y = point[1];
        float z = point[2];

        return InstNewCell(x, y, z, parent);
    }
    
    private static float GetSlope(Vector3 p1, Vector3 p2) {
        float m = (p2[2] - p1[2]) / (p2[0] - p1[0]);
        return m;
    }
    private static Vector3 FindPointLDist(Vector3 point, float m, float l) {
        Vector3 newPoint;
        float sqrtTerm = Mathf.Sqrt(1 / (1 + Mathf.Pow(m, 2)));
        newPoint = new Vector3(
            point[0] + l * sqrtTerm,
            point[1],
            point[2] + m * l * sqrtTerm);

        return newPoint;
    }
}
