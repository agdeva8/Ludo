using UnityEngine;
using UnityEditor;

public class NewCreateBoard { 
    // Start is called before the first frame update
    private static int numRows = 19;
    private static int numCols = 19;
    
    private static GameObject fatherCell = new GameObject();

    private static Vector3 cellScale = 
                        Resources.Load<GameObject>("PreFabs/Cell").transform.localScale;

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
