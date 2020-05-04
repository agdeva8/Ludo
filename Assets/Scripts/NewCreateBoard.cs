using System.Collections;
using UnityEngine;
using UnityEditor;

public class NewCreateBoard { 
    // Start is called before the first frame update
    private static int numRows = 6;
    private static int numCols = 3;
    private static GameObject fatherCell = GameObject.Find("Cells");

    // private static Vector3 cellScale = 
    //                     Resources.Load<GameObject>("PreFabs/Cells").transform.localScale;

    private static Vector3 cellScale = new Vector3(1, 1, 1);
    [MenuItem("Tools/CreateGrid")]
    public static void createGrid() {
        GameObject myFirstCell = (GameObject)GameObject.Instantiate(Resources.Load("PreFabs/Cell"),
                            new Vector3(-4, 0, 0), Quaternion.identity);

    GameObject[,] Cells = new GameObject[numRows, numCols];
    for (int i = 0; i < numRows; i++) {
        for (int j = 0; j < numCols; j++) {
            Cells[i, j] = (GameObject)GameObject.Instantiate(Resources.Load("PreFabs/Cell"),
                                        new Vector3(i * cellScale[2], 0, j * cellScale[0]),
                                        Quaternion.identity);
            Cells[i, j].name = "Cells_" + i.ToString() + "_" + j.ToString(); 
            Cells[i, j].transform.parent = fatherCell.transform;
        }
    }
}
}
