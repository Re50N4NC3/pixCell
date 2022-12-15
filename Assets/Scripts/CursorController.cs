using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour{
    [SerializeField] Vector3 mousePos;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        mousePos = ray.GetPoint(10);

        float targetX = Mathf.Floor(mousePos.x) + 0.5f;
        float targetY = Mathf.Floor(mousePos.y) + 0.5f;

        transform.position = new Vector3(targetX, targetY, 0);
    }
}
