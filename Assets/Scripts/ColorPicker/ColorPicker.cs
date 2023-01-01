using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour{
    public Slider[] hsvSliders = new Slider[3];
    public Image samplerImage;
    public Color resultColor;

    void Awake(){
        CreateColor();
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        CreateColor();
        
        if (samplerImage != null){
            samplerImage.color = resultColor;
        }
    }

    void CreateColor(){
        resultColor = Color.HSVToRGB(hsvSliders[0].value, hsvSliders[1].value, hsvSliders[2].value);

    }
}
