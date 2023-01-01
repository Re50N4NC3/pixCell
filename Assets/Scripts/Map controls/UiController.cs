using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class UiController : MonoBehaviour{
    public TMP_Text textRuleInput;
    public TMP_InputField textRuleInputParent;
    public Text textRuleCurrent;

    public GameObject errorWindow;
    InputField myField;

    [SerializeField] GameObject sidePanel; 
    float holdTimer = 0.0f;
    public float holdTimerMax = 0.6f;
    public bool sidePanelVisible = false;

    [SerializeField] Transform sidePanelPositionOn; 
    [SerializeField] Transform sidePanelPositionOff; 

    [SerializeField] float panelSpeed = 8.0f;
    float distanceDifference;
    Vector3 distanceStartPoint;
    bool distancePointSet = false;
    float minDistanceDifference = 20.0f;

    public bool editingTextInput = false;

    void Start(){
        UpdateRules(GameManager.Instance.ruleInstructions[0]);
    }

    // Update is called once per frame
    void Update(){
        SidePanelMovement();
        SidePanelControls();
        
        if (Input.GetKeyDown(KeyCode.H)) {
            sidePanelVisible = !sidePanelVisible;
        }
    }

    public void StopEditingInput(){
        editingTextInput = false;
    }
    
    public void StartEditingInput(){
        editingTextInput = true;
    }

    public void ProcessRuleInput(){
        if( Regex.IsMatch(textRuleInput.text, "^\\d{0,10}\\/\\d{0,10}") ){
            Debug.Log("prper input");
            UpdateRules(textRuleInput.text);
        }
        else{
            textRuleInput.text = "";
            textRuleInputParent.text = "";
            Debug.Log("wrong input");
        }
    }

    void UpdateRules(string newRule){
        GameManager.Instance.ruleInstructions[0] = newRule;

        textRuleCurrent.text = "Current rule:\n" + newRule;
    }

    public void SidePanelMovement(){
        Vector3  panelTargetPosition;
        if (sidePanelVisible == true){
            panelTargetPosition = new Vector3(sidePanelPositionOn.transform.position.x, sidePanelPositionOn.transform.position.y, sidePanelPositionOn.transform.position.z);
        }
        else{
            panelTargetPosition = new Vector3(sidePanelPositionOff.transform.position.x, sidePanelPositionOff.transform.position.y, sidePanelPositionOff.transform.position.z);
        }

        sidePanel.transform.position = Vector3.Lerp(sidePanel.transform.position, panelTargetPosition, panelSpeed * Time.deltaTime);
    }

    void SidePanelControls(){
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1)){
            if (distancePointSet == false){
                distancePointSet = true;
                distanceStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            distanceDifference = Vector3.Distance((Camera.main.ScreenToWorldPoint(Input.mousePosition)), distanceStartPoint); 

            if (distanceDifference <= minDistanceDifference){
                holdTimer += Time.deltaTime;
            }
        }
        else{
            holdTimer = 0;
            distancePointSet = false;
        }

        if (holdTimer >= holdTimerMax){
            sidePanelVisible = !sidePanelVisible;
            holdTimer = 0;
            distancePointSet = false;
        }
    }
}
