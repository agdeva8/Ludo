using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Profiling.Memory.Experimental;

// TODO Refactoring the code
public class CreateBoard {
    private static float a = 1;
    private static int n = 4;
    private static float Theta = (360 / n);
    private static GameObject[] insideCorners;
    private static List<GameObject[,]> cells;
    private static Vector3 cellScale;
    private static List<Color> playerColors;
    private static List<GameObject> stopPointCubes;
    private static GameObject[] playerPieces;
    private static GameObject playerPiecesParent;
    private static List<GameObject> centerPieces;
    private static GameObject centerPieceParent;
    private static GameObject board;
    
    [MenuItem("Tools/Create Board/Players 4")]
    public static void NPlayers4()
    {
        n = 4;

        board = GameObject.Find("Board");
        if (board != null)
            GameObject.DestroyImmediate(board);
        Main();
    }
    
    [MenuItem("Tools/Create Board/Players 5")]
    public static void NPlayers5()
    {
        n = 5;
        board = GameObject.Find("Board");
        if (board != null)
            GameObject.DestroyImmediate(board);
        Main();
    }
    
    [MenuItem("Tools/Create Board/Players 6")]
    public static void NPlayers6()
    {
        n = 6;
        board = GameObject.Find("Board");
        if (board != null)
            GameObject.DestroyImmediate(board);
        Main();
    }
    
    [MenuItem("Tools/Create Board/Players 7")]
    public static void NPlayers7()
    {
        n = 7;
        board = GameObject.Find("Board");
        if (board != null)
            GameObject.DestroyImmediate(board);
        Main();
    }
    
    [MenuItem("Tools/Create Board/Players 8")]
    public static void NPlayers8()
    {
        n = 8;
        board = GameObject.Find("Board");
        if (board != null)
            GameObject.DestroyImmediate(board);
        Main();
    }
    
    // [MenuItem("Tools/Create Board")]
    public static void Main()
    {
        Theta = (float) (360.0 / n);
        board = new GameObject("Board");
        insideCorners = new GameObject[n];
        cells = new List<GameObject[,]>();
        stopPointCubes = new List<GameObject>();
        cellScale = new Vector3(1, 1, 1);
        playerPieces = new GameObject[n];

        CreateRect();
        // CreateCenter();
        playerColors = new List<Color>() {
            // Sky Blue
            new Color(0.43f, 0.99f, 1f),
            
            // Yellow 
            new Color(1f, 0.99f, 0.59f),
            
            // Baby pink 
            new Color(1f, 0.74f, 0.99f),
            
            // Blood Red 
            new Color(1f, 0.35f, 0.35f),
            
            // Parrot Green 
            new Color(0.51f, 1f, 0.41f),
            
            // Purple 
            new Color(0.63f, 0.31f, 1f),
           
            // Violet
            new Color(0.38f, 0.44f, 1f),
            
            // Grey 
            new Color(0.67f, 0.67f, 0.67f)
        };

        SetBasicColor();
        AddStopPoints();
        PlacePiecesStart();
        CreateCenterPiece();
        CreateStartHomes();
        // CheckMetaData();
        RelateCells();
    }

    // Check Whether Meta Data concept is working or not 
    // For now its true;

    private static void RelateCells()
    {
        // cells[0][2, 4].GetComponent<CellMetaData>().SetNextGameObj(cells[0][2, 3]);
        // cells[0][2, 4].GetComponent<CellMetaData>()._nextObjOtherPlayer = cells[0][2, 3];
        for (int player = 0; player < n; player++)
        {
            for (int i = 5; i > 0; i--)
                cells[player][2, i].GetComponent<CellMetaData>().SetNextGameObj(cells[player][2, i - 1]);
            cells[player][2, 0].GetComponent<CellMetaData>().SetNextGameObj(cells[(player + 1) % n][0, 0]); 
            //
            for (int i = 0; i < 5; i++)
                cells[player][0, i].GetComponent<CellMetaData>().SetNextGameObj(cells[player][0, i + 1]);
            cells[player][0, 5].GetComponent<CellMetaData>().SetNextGameObj(cells[player][1, 5]); 
            //
            cells[player][1, 5].GetComponent<CellMetaData>().SetNextGameObj(cells[player][2, 5]); 
        }
        
    }

    // Move Piece using menu bar option
    // [MenuItem(itemName: "Tools/Move Player")]
    public static void MovePlayerFromMenu()
    {
        // for (int i = 1; i < 4; i++)
        // {
        int i = 1;
            GameObject currCell = GameObject.Find(name: $"C{i}");
            GameObject nextCell = GameObject.Find(name: $"C{(i + 1)}");
            GameObject player = GameObject.Find(name: "P1");
            MovePlayer(player: player, currCell: currCell, nextCell: nextCell);
        // }
        
    } 
    
    private static void MovePlayer(GameObject player, GameObject currCell, GameObject nextCell)
    {
        // GameObject nextCell = currCell;
        if (nextCell == null)
            return;

        Rigidbody rb = player.transform.GetComponent<Rigidbody>();
        float jumpVel = 1000f;

        rb.velocity = Vector3.up * jumpVel;
        // player.transform.Translate(new Vector3(0.0f , 1.5f, 0.0f));
        // PlacePeice(player, nextCell);
    } 
     // [MenuItem("Tools/Reset Player")]
    public static void ResetPlayer()
    {
        GameObject currCell = GameObject.Find("C1");
        GameObject nextCell = GameObject.Find("C2");
        GameObject player = GameObject.Find("P1");

        player.transform.position = NewPiecePostion(currCell);
    } 
    
    // moving player one step ahead
    // This function can be shifted to different script 
    // Next Cell var is temporary
    private static void CheckMetaData() {
        var metaData = cells[0][0, 0].GetComponent<CellMetaData>();
        metaData.isStop = false;
        Debug.Log($"Changing variable to false {metaData.isStop}");
        // Debug.Log($"metadata is {metaData}");
        metaData.isStop = true;
        Debug.Log($"Changing variable to true {metaData.isStop}");
    }

    private static void CreateStartHomes()
    {
        // Main Parent of Start Home
        GameObject homeParent = new GameObject("Start Home");
        homeParent.transform.parent = board.transform;
        
        GameObject outParent = new GameObject("Out Home");
        outParent.transform.parent = homeParent.transform;
        
        GameObject inParent = new GameObject("In Home");
        inParent.transform.parent = homeParent.transform;
        
        Vector3[,] outCorners = new Vector3[n, 2];

        GameObject dummy = new GameObject("Dummy");
        for (int player = 0; player < n; player++)
        {
            dummy.transform.parent = cells[player][2, 5].transform;
            dummy.transform.localPosition = new Vector3(0, 0, 0);
            dummy.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            dummy.transform.Translate(new Vector3(0.5f, 0.5f, 0.5f));
            outCorners[player, 0] = dummy.transform.position; 
            
            dummy.transform.parent = cells[player][0, 5].transform;
            dummy.transform.localPosition = new Vector3(0, 0, 0);
            dummy.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            dummy.transform.Translate(new Vector3(-0.5f, 0.5f, 0.5f));
            outCorners[player, 1] = dummy.transform.position;
        }

        for (int player = 0; player < n; player++)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            Vector3 pos = insideCorners[(player + 1) % n].transform.position;
            pos.y = (float) (pos.y + 0.5);
            vertices.Add(pos);

            vertices.Add(outCorners[player, 0]);

            // only  for n = 4

            if (n == 4)
            {
                dummy.transform.parent = cells[player][2, 5].transform;
                dummy.transform.localPosition = new Vector3(0, 0, 0);
                dummy.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                dummy.transform.Translate(new Vector3(6.5f, 0.5f, 0.5f));
                vertices.Add(dummy.transform.position);
            }

            vertices.Add(outCorners[(player + 1) % n, 1]);

            GameObject outPiece = new GameObject($"OutHome {player}");
            outPiece.transform.parent = outParent.transform;
            MeshFilter outPieceMf = outPiece.AddComponent<MeshFilter>();
            MeshRenderer outPieceMr = outPiece.AddComponent<MeshRenderer>();

            if (n == 4)
            {
                triangles.AddRange(new List<int>() {0, 1, 2});
                triangles.AddRange(new List<int>() {2, 3, 0});
            }
            else
            {
                triangles.AddRange(new List<int>() {0, 1, 2});
            }

            Mesh mesh = new Mesh();
            outPieceMf.sharedMesh = mesh;

            outPieceMf.sharedMesh.vertices = vertices.ToArray();
            outPieceMf.sharedMesh.triangles = triangles.ToArray();
            outPieceMf.sharedMesh.RecalculateNormals();

            // Adding Material 
            outPieceMr.material = (Material)Resources.Load("Material/CenterPeice");
            ChangeColor(outPiece, playerColors[player]);

            
            // Adding InPiece
            // Finding center vertex

            Vector3 sum = new Vector3(0, 0,0);
            foreach (var vertex in vertices)
                sum += vertex;

            Vector3 centroidVertex = (sum / vertices.Count);
            
            GameObject inPiece = GameObject.Instantiate(outPiece);
            inPiece.name = $"InHome {player}";
        
            GameObject inPieceParent = new GameObject($"InHome {player} Parent");
            inPieceParent.transform.position = centroidVertex; 
            inPieceParent.transform.parent = inParent.transform;
        
            inPiece.transform.parent = inPieceParent.transform;
            ChangeColor(inPiece, playerColors[player] * 0.6f);
        
            inPiece.transform.Translate(new Vector3(0, 0.01f, 0));
            inPieceParent.transform.localScale = new Vector3(0.6f, 1, 0.6f);

            outPiece.transform.parent = board.transform;
        }
    }

    private static Vector3 IntersectionPoint(Vector3 p1, Vector3 p2, Vector3 p1Bar, Vector3 p2Bar) 
    {
        Vector3 p;

        float m1 = GetSlope(p1, p1Bar);
        float m2 = GetSlope(p2, p2Bar);
        
        Debug.Log("m1 is " + m1);
        Debug.Log("m2 is " + m2);
        

        if (Math.Abs(m1) < 0.01f && float.IsInfinity(Mathf.Abs(m2)))
        {
            p.x = p2.x;
            p.y = p1.y;
            p.z = p1.z;
            return p;
        }
        
        if (float.IsInfinity(Mathf.Abs(m1)) && Math.Abs(m2) < 0.01f)
        {
            p.x = p1.x;
            p.y = p1.y;
            p.z = p2.z;
            return p;
        }

            p.x = (p2.z - p1.z + m1 * p1.x - m2 * p2.x) / (m1 - m2);
        p.y = 1; 
        p.z = (p2.x - p1.x + m1 * p1.z - m2 * p2.z) / (1.0f / m1 -  1.0f / m2);

        return p;
    }
    
    private static void CreateCenterPiece() {
        Vector3 centerPos = GetCenterPos();

        centerPieceParent = new GameObject("Center Piece");
        centerPieceParent.transform.position = centerPos;
        centerPieceParent.transform.parent = board.transform;

        GameObject outPp = new GameObject("Out Parent");
        outPp.transform.parent = centerPieceParent.transform;
        GameObject outParent = new GameObject("Out Piece");
        outParent.transform.parent = outPp.transform;

        centerPieces = new List<GameObject>();
        for (int player = 0; player < n; player++) {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            vertices.Add(centerPos);
            vertices.Add(insideCorners[player].transform.position);
            vertices.Add(insideCorners[(player + 1) % n].transform.position);

            for (int i = 0; i < 3; i++) {
                vertices[i] =  new Vector3(vertices[i].x, 0.5f, vertices[i].z);
            }

            GameObject centerPeice = new GameObject("Peice " + player);
            centerPeice.transform.parent = outParent.transform;
            MeshFilter centerPeiceMf = centerPeice.AddComponent<MeshFilter>(); 
            MeshRenderer centerPeiceMr = centerPeice.AddComponent<MeshRenderer>();

            triangles.AddRange(new List<int>() {0, 1, 2});

            Mesh mesh = new Mesh();
            centerPeiceMf.sharedMesh = mesh;

            centerPeiceMf.sharedMesh.vertices = vertices.ToArray();
            centerPeiceMf.sharedMesh.triangles = triangles.ToArray();
            centerPeiceMf.sharedMesh.RecalculateNormals();

            // Adding Material 
            centerPeiceMr.material = (Material)Resources.Load("Material/CenterPeice");
            ChangeColor(centerPeice, playerColors[player]);

            // Adding to the list of center peices
            centerPieces.Add(centerPeice);
        }

        // create inside center Piece 
        // GameObject inPp = new GameObject("In Parent");
        // inPp.transform.parent = centerPieceParent.transform; 
        // inPp.transform.position = centerPos;
        // GameObject inParent = GameObject.Instantiate(outParent);
        // inParent.name = "In Peice";
        // inParent.transform.parent = inPp.transform;
        // inParent.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        //
        // inPp.transform.localScale = new Vector3(0.5f, 0.2f, 0.5f);
        //
        // MeshRenderer[] inChildrenMr = inParent.GetComponentsInChildren<MeshRenderer>();
        // for (int i = 0; i < inChildrenMr.Length; i++)
        //     inChildrenMr[i].material = (Material)Resources.Load("Material/InCenter");
    }

    private static Vector3 GetCenterPos() {
        Vector3 centerPos = cells[(int)(n / 2)][1, 0].transform.position +  
                            cells[0][1, 0].transform.position;

        centerPos = centerPos / 2;
        return centerPos;
    }

    public static Vector3 NewPiecePostion(GameObject cell) {
        Vector3 retPosition = cell.transform.position;
        retPosition.y = retPosition.y + 0.5f;

        return retPosition;
    }
    
    private static void PlacePiecesStart() {
        playerPiecesParent = new GameObject("Players");
        playerPiecesParent.transform.parent = board.transform;
        
        for (int player = 0; player < n; player++) {
            playerPieces[player] = InstObj(new Vector3(0, 0, 0),
                                     "PreFabs/PlayerPeice", playerPiecesParent);
            
            // finding first cell
            GameObject cell = cells[player][2, 4];
            
            // adding meta data 
            playerPieces[player].GetComponent<PlayerMetaData>().currCell = cell;
            
            // placing player piece
            playerPieces[player].transform.position = NewPiecePostion(cell);
            playerPieces[player].transform.localRotation = Quaternion.Euler(-90, 0, 0);
            
            // changing primary attributes
            playerPieces[player].name = "Player " + player.ToString();
            ChangeColor(playerPieces[player], playerColors[player]);
        }
    }

    // private static GameObject PlacePeice(GameObject player, GameObject Cell)
    // {
    //     player.transform.parent = parent.transform;
    //     player.transform.localPosition = new Vector3(0, 0.5f, 0); 
    //     player.transform.localRotation = Quaternion.Euler(-90, 0, 0);
    //     return player;
    // }
    // private static GameObject PlacePeice(GameObject parent) {
    //     GameObject playerPeice = InstObj(new Vector3(0, 0.5f, 0), "PreFabs/PlayerPeice", parent);
    //     return PlacePeice(playerPeice, parent);
    // }
    
    private static void AddStopPoints() {
        for (int player = 0; player < n; player++) {
            PlaceStopCube(player, 0, 3);
            PlaceStopCube(player, 2, 4);
        }
    }

    private static void PlaceStopCube(int player, int r, int c) {
        GameObject parent = cells[player][r, c];
        GameObject stopPointCube = InstObj(new Vector3(0, 0, 0), "PreFabs/StopCube", parent); 
        stopPointCube.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        stopPointCube.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        stopPointCubes.Add(stopPointCube);
    }

    private static void SetBasicColor(){
        for (int player= 0; player < n; player++) {
            for (int i = 0; i < 5; i++)
                ChangeColor(cells[player][1, i], playerColors[player]);
            ChangeColor(cells[player][2, 4], playerColors[player]);
        }
    } 

    private static void ChangeColor(GameObject obj, Color color) {
        var cubeRenderer = obj.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", color);
    }

    public static void CreateCenter() {
        Vector3 centerPos = cells[(int)(n / 2)][1, 0].transform.position +  
                            cells[0][1, 0].transform.position;

        centerPos = centerPos / 2;
        Debug.Log(cells[(int)(n / 2)][1, 0].transform.position); 

        GameObject centerPiece = (GameObject)GameObject.Instantiate(Resources.Load("PreFabs/Hexagon"),
                                        centerPos,
                                        Quaternion.identity);
    }

    public static void CreateRect() {
        for (int i = 0; i < n; i++) {
            insideCorners[i] = new GameObject("Rect Parent " + i.ToString());
            if (i >= 1) {
                insideCorners[i].transform.parent = insideCorners[i - 1].transform;
            }

            insideCorners[i].transform.localPosition = new Vector3(3 * a, 0, 0);
            cells.Add(CreateGrid(3, 6, insideCorners[i]));
            
            if (i >= 1) {
                insideCorners[i].transform.localRotation = Quaternion.Euler(0, Theta, 0);
            }
        }

        insideCorners[0].transform.parent = board.transform;
    }

    
    public static GameObject[,] CreateGrid(int numRows, int numCols, GameObject parent = null) {
        GameObject[,] cells = new GameObject[numRows, numCols];
        for (int i = 0; i < numRows; i++) {
            for (int j = 0; j < numCols; j++) {
                cells[i, j] = InstNewCell(0, 0, 0, parent);

                cells[i, j].transform.localPosition = 
                                new Vector3(i * cellScale[2] + a / 2, 0, j * cellScale[0] + a / 2); 
            }
        }

        return cells;
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
