using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangtonsAnt : MonoBehaviour {
    private GenerateMap map;
    private GameManager gameManager;
    private MapControls controls;

    public int steps = 0;

    public int posX = 120;
    public int posY = 60;
    public int direction = 0;  // N W S E

    public int antState = 0;  // for turmites
    int cellState = 0;
    Node currentNode;

    public string[] instruction;
    public Color[] stateColors;
    char currentInstruction = 'L';
    int instructionAmount;

    public float timerMax = 0.5f;
    float timer = 0.0f;

    private void Awake() {
        map = GetComponent<GenerateMap>();
        gameManager = GetComponent<GameManager>();
        controls = GetComponent<MapControls>();

        instructionAmount = instruction[0].Length;
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (gameManager.ant == true) {
            timer += Time.deltaTime;

            if (timer >= timerMax) {
                AntBehavior();
                steps += 1;
            }
        }
    }

    void AntBehavior() {
        GetCell();
        TurnAround();
        MoveForward();
        UpdateCellState();
        UpdateCellVisuals();
    }

    void GetCell() {
        currentNode = controls.GetNode(posX, posY);

        cellState = currentNode.antState;
    }

    void TurnAround() {
        currentInstruction = instruction[antState][cellState];

        if (currentInstruction == 'L') { direction -= 90; }
        if (currentInstruction == 'R') { direction += 90; }
        if (currentInstruction == 'N') { direction += 0; }
        if (currentInstruction == 'U') { direction += 180; }

        if (direction == -90) { direction = 270; }
        if (direction == -180) { direction = 180; }
        if (direction == 360) { direction = 0; }
        if (direction == 450) { direction = 90; }
    }

    void MoveForward() {
        switch (direction) {
            case 0:
                posY += 1;
                break;
            case 90:
                posX += 1;
                break;
            case 180:
                posY -= 1;
                break;
            case 270:
                posX -= 1;
                break;
        }
    }

    void UpdateCellState() {
        currentNode.antState += 1;

        if (currentNode.antState >= instructionAmount) {
            currentNode.antState = 0;
        }
    }

    void UpdateCellVisuals() {
        Color c = new Color(
            stateColors[currentNode.antState].r,
            stateColors[currentNode.antState].g,
            stateColors[currentNode.antState].b
            );

        currentNode.cellColor = c;
        map.textureInstance.SetPixel(posX, posY, c);

        map.textureInstance.Apply();

    }
}
