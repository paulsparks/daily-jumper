using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SceneBuilder : MonoBehaviour{


    public GameObject startingPrefab;
    private float nextPlatformYPos = (float)-1.5;
    private float platformHeightDistance = (float)1.5;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.clear;
        for (int i = 0; i < 5; i++) 
        {
            // Range -9.5 to 7
            Instantiate(startingPrefab, new Vector3(Random.Range(platformHeightDistance-9.5f, 7f), nextPlatformYPos, 0), Quaternion.identity);
            nextPlatformYPos = nextPlatformYPos + platformHeightDistance;
        }

        // DrawOneRow();
        // float cameraHeight = SetMainCameraRectColor();
        // Debug.Log("Camera Height: " + cameraHeight);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
    }

    // Set Main Camera Rect View Color
    void DrawOneRow(){
        Camera mainCamera = Camera.main;

        float viewportWidth = mainCamera.pixelWidth;
        float viewportHeight = mainCamera.pixelHeight / 4;

        Debug.Log("Viewport Width: " + viewportWidth);
        Debug.Log("Viewport Height: " + viewportHeight);

        GameObject rectangleObject = new GameObject("Rectangle");

        // Add a RectTransform to control the position and size
        RectTransform rectTransform = rectangleObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(viewportWidth, viewportHeight);

        // Set the parent canvas for the rectangle
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            rectangleObject.transform.SetParent(canvas.transform, false);
        }
        else
        {
            Debug.LogWarning("No Canvas found in the scene. Create a Canvas and attach the rectangle to it.");
        }

        // Add an Image component to render the rectangle
        Image image = rectangleObject.AddComponent<Image>();
        image.color = Color.red;
    }
}
