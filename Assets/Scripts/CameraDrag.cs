using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour{
    private Vector3 origin;
    private Vector3 difference;
    private Vector3 resetCamera;

    float smoothSpeed = 2.0f;

    private bool drag = false;

    private void Start(){
        resetCamera = Camera.main.transform.position;
    }

    private void LateUpdate(){
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1)){
            difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if(drag == false){
                drag = true;
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else{
            drag = false;
        }

        if (drag){
            Camera.main.transform.position = Vector3.Lerp(transform.position, origin - difference * 1.5f, smoothSpeed * Time.deltaTime);
        }

        Cursor.visible = !drag;
    }
}

