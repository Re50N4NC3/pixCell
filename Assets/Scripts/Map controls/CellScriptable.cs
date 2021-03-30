using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cell Data", menuName = "Cell Data")]
public class CellScriptable : ScriptableObject{
    public string cellName;
    public Color cellColor;
}
