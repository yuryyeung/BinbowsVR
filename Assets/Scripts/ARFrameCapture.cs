using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARFrameCapture : MonoBehaviour
{
    [SerializeField] RawImage _leftImage, _rightImage;
    [SerializeField] CameraImageExample _camImage;

    // Start is called before the first frame update
    void Update() 
    {
        _leftImage.texture = _camImage.CurrentArImage;
        _rightImage.texture = _camImage.CurrentArImage;
    }
}
