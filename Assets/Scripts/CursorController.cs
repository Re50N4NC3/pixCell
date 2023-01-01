using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour{
    [SerializeField] Vector3 mousePos;

    public SpriteRenderer spriteRenderer;
    public CameraDrag cameraDrag;

    public bool invisibleWhenDragging = false;
    public bool snappingToGrid = true;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        mousePos = ray.GetPoint(10);

        float targetX = 0;
        float targetY = 0;

        if (snappingToGrid == true){
            targetX = Mathf.Floor(mousePos.x) + 0.5f;
            targetY = Mathf.Floor(mousePos.y) + 0.5f;
        }
        else{
            targetX = mousePos.x;
            targetY = mousePos.y;
        }
        transform.position = new Vector3(targetX, targetY, 0);

        if (invisibleWhenDragging == true){
            spriteRenderer.enabled = !cameraDrag.drag;
        }
    }
}
