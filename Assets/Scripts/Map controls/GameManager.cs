using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private GenerateMap map;
    private MapControls controls;

    public bool changeColorOverTime = true;

    public CellScriptable[] scriptableCellsData;
    public string[] ruleInstructions;

    public int deactivationTime = 4;

    public bool activated = false;
    public bool ant = false;
    bool findNodes = false;

    public List<Node> activatedNodes = new List<Node>();
    public List<Node> activatedNodesBuffer = new List<Node>();
    /* 
     * activation > 
     * find white nodes > 
     * save white nodes positions in list > // only for activation
     * add neighboring positions to list >
     * filter duplicates >
     * check neighbors and rules of nodes from list >
     * change deltas to current state, saving white nodes in list
    */
    int[] neighbors;
    int[] neighborsReset;

    public float timerMax = 0.5f;
    float timer = 0.0f;

    private void Awake() {
        map = GetComponent<GenerateMap>();
        controls = GetComponent<MapControls>();

        SetNeighborArrays();
    }

    void SetNeighborArrays() {
        neighbors = new int[scriptableCellsData.Length];
        neighborsReset = new int[scriptableCellsData.Length];

        for (int i = 0; i < scriptableCellsData.Length; i++) {
            neighbors[i] = 0;
            neighborsReset[i] = 0;
        }
    }

    private void Start() {
        map.CreateLevel();
    }

    private void Update() {
        if (activated == true) {
            if (findNodes == false) {
                findNodes = true;

                FindActivatedNodes();
            }

            timer -= Time.deltaTime;

            if (timer <= 0) {
                timer = timerMax;

                ExecuteNodes();
            }
        }
        else {
            findNodes = false;
        }
        SimulationControls();
    }

    void SimulationControls() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            activated = !activated;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            FindActivatedNodes();
            ExecuteNodes();
        }
    }

    public void FindActivatedNodes() {
        for (int x = 0; x < map.maxX; x++) {
            for (int y = 0; y < map.maxY; y++) {
                Node node = controls.GetNode(x, y);

                UpdateGridVisuals(node);
                ResetGridDelta(node);

                if (node.cellType != 0) {
                    activatedNodes.Add(node);
                }
            }
        }
        map.textureInstance.Apply();
    }

    void ExecuteNodes() {
        GetActiveNeighbors();
        RemoveDupes();
        ExecuteRules();
        UpdateGridTypes();

        activatedNodes.Clear();
        activatedNodes = new List<Node>(activatedNodesBuffer);
        activatedNodesBuffer.Clear();
    }

    void GetActiveNeighbors() {
        activatedNodesBuffer.Clear();

        for (int n = 0; n < activatedNodes.Count; n++) {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    activatedNodesBuffer.Add(controls.GetNode(activatedNodes[n].x + x, activatedNodes[n].y + y));
                }
            }
        }

        activatedNodes.Clear();
        activatedNodes = new List<Node>(activatedNodesBuffer);
        activatedNodesBuffer.Clear();
    }

    void RemoveDupes() {
        activatedNodes = activatedNodes.Distinct().ToList();
    }

    void ExecuteRules() { 
        for (int n = 0; n < activatedNodes.Count; n++) {
            Node node = activatedNodes[n];

            ApplyRules(node);
        }
    }

    void ApplyRules(Node node) {
        neighbors = CountNeighbors(node);
        bool found = false;

        // 0 - current state; 1 - state to change to; 2 - state to check for
        // go through all the rules
        for (int i = 0; i < ruleInstructions.Length; i++) {
            if (ruleInstructions[i][0] != '_') {
                if ((int)char.GetNumericValue(ruleInstructions[i][0]) == node.cellType) {

                // direct change
                if (ruleInstructions[i].Length < 3) {
                    int stateToTurnInto = (int)char.GetNumericValue(ruleInstructions[i][1]);
                    found = true;
                    node.cellTypeDelta = stateToTurnInto;

                    break;
                }
                // ruled change
                else {
                    int neighborToCheckFor = (int)char.GetNumericValue(ruleInstructions[i][2]);

                    // check rule for current cell type
                    for (int s = 3; s < ruleInstructions[i].Length; s++) {
                        int neighborsNeeded = (int)char.GetNumericValue(ruleInstructions[i][s]);

                        if (neighbors[neighborToCheckFor] == neighborsNeeded) {
                            int stateToTurnInto = (int)char.GetNumericValue(ruleInstructions[i][1]);
                            found = true;
                            node.cellTypeDelta = stateToTurnInto;

                            break;
                        }
                    }
                }
            }

            if (found == true) { break; }
            }
        }
    }

    int[] CountNeighbors(Node node) {
        for (int i = 0; i < scriptableCellsData.Length; i++) {
            neighbors[i] = 0;
        }

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (node.x > 1 && node.x < map.maxX - 1 && node.y > 1 && node.y < map.maxY - 1) { 
                    neighbors[controls.GetNode(node.x + x, node.y + y).cellType] += 1;
                }
            }
        }

        neighbors[node.cellType] -= 1;

        return neighbors;
    }

    void UpdateGridTypes() {
        for (int n = 0; n < activatedNodes.Count; n++) {
            Node node = controls.GetNode(activatedNodes[n].x, activatedNodes[n].y);

            ResetGridDelta(node);
            UpdateGridVisuals(node);
        }

        // update canvas
        map.textureInstance.Apply();
    }

    public void UpdateGridVisuals(Node node) {
        Color c = new Color(
            scriptableCellsData[node.cellType].cellColor.r,
            scriptableCellsData[node.cellType].cellColor.g,
            scriptableCellsData[node.cellType].cellColor.b
            );
        
        /*Color c = new Color(
            scriptableCellsData[node.cellType].cellColor.r / (400 / (node.unchangedAge + 1)),
            scriptableCellsData[node.cellType].cellColor.g / (120 / (node.unchangedAge + 1)),
            scriptableCellsData[node.cellType].cellColor.b / (40 / (node.unchangedAge + 1))
            );
        nice colors for mushroom
         *         Color c = new Color(
            scriptableCellsData[node.cellType].cellColor.r / (150 / (node.unchangedAge + 1)),
            scriptableCellsData[node.cellType].cellColor.g / ( 40 / (node.unchangedAge + 1)),
            scriptableCellsData[node.cellType].cellColor.b / (400 / (node.unchangedAge + 1))
            );
         */
        node.cellColor = c;

        map.textureInstance.SetPixel(node.x, node.y, c);
    }

    void ResetGridDelta(Node nodeToReset) {
        if (nodeToReset.cellType == nodeToReset.cellTypeDelta) {
            if (activated == true) {
                nodeToReset.unchangedAge += 1;
            }
        }
        else {
            nodeToReset.unchangedAge = 0;
        }

        nodeToReset.cellType = nodeToReset.cellTypeDelta;
        nodeToReset.cellTypeDelta = nodeToReset.cellType;

        if (nodeToReset.cellType != 0 && nodeToReset.unchangedAge < deactivationTime) {
            activatedNodesBuffer.Add(nodeToReset);
        }
    }
}

public class Node {
    public int x;
    public int y;

    public int velX;
    public int velY;
    public int accX;
    public int accY;

    public int antState = 0;

    public int unchangedAge = 0;

    public int cellType;
    public int cellTypeDelta;

    public Color cellColor;
}