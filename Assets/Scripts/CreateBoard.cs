using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// custom functions

public class CreateBoard {
    private static float a = 1;
    private static int n = 8;
    private static float theta = (360 / n);
    private static List<GameObject[,]> Cells;
    private static Vector3 cellScale;
    private static List<Color> playerColors;
    private static List<GameObject> stopPointCubes;

    [MenuItem("Tools/Create Board")]
    public static void Main() {
        Cells = new List<GameObject[,]>();
        stopPointCubes = new List<GameObject>();
        cellScale = new Vector3(1, 1, 1);
        CreateRect();
        CreateCenter();
        playerColors = new List<Color>() {
            Color.white,
            Color.yellow,
            Color.blue,
            Color.red,
            Color.green,
            Color.black,
            Color.magenta,
            Color.cyan
        };

        SetBasicColor();
        AddStopPoints();
    }
    
    private static void AddStopPoints() {
        for (int player = 0; player < n; player++) {
            PlaceStopCube(player, 0, 3);
            PlaceStopCube(player, 2, 4);
        }
    }

    private static void PlaceStopCube(int player, int r, int c) {
        GameObject parent = Cells[player][r, c];
        GameObject stopPointCube = InstObj(new Vector3(0, 0, 0), "PreFabs/StopCube", parent); 
        stopPointCube.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        stopPointCube.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        stopPointCubes.Add(stopPointCube);
    }

    private static void SetBasicColor(){
        for (int player= 0; player < n; player++) {
            for (int i = 0; i < 5; i++)
                changeColor(Cells[player][1, i], playerColors[player]);
            changeColor(Cells[player][2, 4], playerColors[player]);
        }
    } 

    private static void changeColor(GameObject obj, Color color) {
        var cubeRenderer = obj.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", color);
    }

    public static void CreateCenter() {
        Vector3 centerPos = Cells[(int)(n / 2)][1, 0].transform.position +  
                            Cells[0][1, 0].transform.position;

        centerPos = centerPos / 2;
        Debug.Log(Cells[(int)(n / 2)][1, 0].transform.position); 

        GameObject centerPeice = (GameObject)GameObject.Instantiate(Resources.Load("PreFabs/Hexagon"),
                                        centerPos,
                                        Quaternion.identity);
    }

    public static void CreateRect() {
        GameObject[] rectParents = new GameObject[n];

        for (int i = 0; i < n; i++) {
            rectParents[i] = new GameObject("Rect Parent " + i.ToString());
            if (i >= 1) {
                rectParents[i].transform.parent = rectParents[i - 1].transform;
            }

            rectParents[i].transform.localPosition = new Vector3(3 * a, 0, 0);
            Cells.Add(CreateGrid(3, 6, rectParents[i]));
            
            if (i >= 1) {
                rectParents[i].transform.localRotation = Quaternion.Euler(0, theta, 0);
            }
        }
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

    private static GameObject InstObj(Vector3 point, string obj, GameObject parent = null) { 
        GameObject newObj;

        if (parent == null) {
            newObj = (GameObject)GameObject.Instantiate(Resources.Load(obj),
                                        point,
                                        Quaternion.identity);
        }
        else {
            newObj = (GameObject)GameObject.Instantiate(Resources.Load(obj),
                                        point,
                                        Quaternion.identity, parent.transform);
        }
        return newObj;
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
