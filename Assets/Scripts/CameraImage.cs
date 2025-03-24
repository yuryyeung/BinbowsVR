using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CameraImage : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] RawImage _leftImage, _rightimage;
    [SerializeField] RenderTexture _currentRT;
    [SerializeField] Text _text;

    private Texture2D _previousTexture;

    void Start() 
    {
        float aspect = Screen.width / Screen.height;
        _currentRT = new RenderTexture((int)(Screen.width), Screen.height, 32, RenderTextureFormat.ARGB32);
        _currentRT.Create();

        _camera.targetTexture = _currentRT;
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.Blit(null, _currentRT);
    }
    
    [SerializeField] ARCameraManager cameraManager = null;
 
    bool firstFrameReceived;
 
 
    void OnEnable() {
        cameraManager.frameReceived += frameReceived;
    }
 
    void OnDisable() {
        cameraManager.frameReceived -= frameReceived;
    }
 
    void frameReceived(ARCameraFrameEventArgs _) {
        Destroy(_previousTexture);

        Texture2D tex = new Texture2D(Screen.width / 2, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = _currentRT;
        tex.ReadPixels(new Rect((int)Screen.width / 4, 0, (int)Screen.width / 2, Screen.height), 0, 0);
        tex.Apply();

        _previousTexture = tex;
        
        _leftImage.texture = tex;
        _rightimage.texture = tex;
    }
}
