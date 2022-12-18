using UnityEngine;

public class MapControls : MonoBehaviour {
    private GenerateMap map;
    private GameManager gameManager;

    Node curNode;
    Vector3 mousePos;

    public int pickedType = 1;
    public int brushSize = 3;

    float leftClickTimer = 0.0f;

    private void Awake() {
        map = GetComponent<GenerateMap>();
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        HandleMouseInput();
        PickPlacedType();
        ResetMap();
    }

    public void HandleMouseInput() {
        //clicked with mouse
        if (Input.GetMouseButton(0)) {
            leftClickTimer += Time.deltaTime;

            if (leftClickTimer > 0.03f && !Input.GetMouseButton(1)){
                for (int x = 0; x < brushSize; x++) {
                    for (int y = 0; y < brushSize; y++) {
                        GetMousePos();
                        if (curNode == null){
                            continue;
                        }
                        
                        int targetX = x + curNode.x;
                        int targetY = y + curNode.y;

                        Node node = GetNode(targetX, targetY);

                        if (node == null) {
                            continue;
                        }

                        // change cell type
                        node.cellType = pickedType;
                        node.cellTypeDelta = node.cellType;
                        node.unchangedAge = 0;
                        map.chunkAgeCounter[Mod(targetX, map.chunkSize), Mod(targetY, map.chunkSize)] = 0;

                        gameManager.UpdateGridVisuals(node);
                    }
                }
            }

            gameManager.FindActivatedNodes();
            map.textureInstance.Apply();
        }
        else{
            leftClickTimer = 0.0f;
        }
    }

    public void ResetMap() {
        if (Input.GetKeyDown(KeyCode.R)) {
            for (int x = 0; x < map.maxX; x++) {
                for (int y = 0; y < map.maxY; y++) {
                    Node node = GetNode(x, y);
                    node.cellType = 0;
                    node.cellTypeDelta = 0;
                    node.unchangedAge = 0;

                    gameManager.UpdateGridVisuals(node);
                }
            }

            map.textureInstance.Apply();
        }
    }


    public void PickPlacedType() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) { pickedType = 0; }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { pickedType = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { pickedType = 2; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { pickedType = 3; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { pickedType = 4; }
    }

    public void GetMousePos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        mousePos = ray.GetPoint(10);

        curNode = GetNodeFromWorldPos(mousePos);
    }

    public Node GetNodeFromWorldPos(Vector3 worldPos) {
        int targetX = Mathf.FloorToInt(worldPos.x);
        int targetY = Mathf.FloorToInt(worldPos.y);

        return GetNode(targetX, targetY);
    }

    public Node GetNode(int x, int y) {
        if (x < 0 || y < 0 || x > map.maxX - 1 || y > map.maxY - 1) {
            return null;
        }
        else {
            return map.grid[x, y];
        }
    }

    int Mod(int a, int b) {
        if (a < b) {
            return 0;
        }

        return (a - (a % b)) / b;
    }
}
