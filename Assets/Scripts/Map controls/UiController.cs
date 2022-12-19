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

    // Update is called once per frame
    void Update(){
        
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
}
