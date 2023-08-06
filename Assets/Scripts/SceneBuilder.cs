using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SceneBuilder : MonoBehaviour{


    public Transform playerTransform;
    public GameObject[] startingPrefabs;
    private float nextPlatformYPos = (float)-1.5;
    private float platformHeightDistance = (float)1.5;
    private float cameraHeight;
    private Vector3 initialPlayerTransformPosition;
    private int stage = 0;
    
    // Start is called before the first frame update
    void Start() {
        initialPlayerTransformPosition = playerTransform.position;
        CreateSet();
        // DrawOneRow();
        // float cameraHeight = SetMainCameraRectColor();
        // Debug.Log("Camera Height: " + cameraHeight);
    }

    // Update is called once per frame
    void Update() {
        if (playerTransform.position.y > (Camera.main.orthographicSize + 1)) {
            playerTransform.position = initialPlayerTransformPosition;
            CreateSet();
        }
    }

    void CreateSet() {
        stage++;
        nextPlatformYPos = (float)-1.5;

        if (stage > 1) {
            foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
                if (o.layer == 6) {
                    Destroy(o);
                }
		    }
        }

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.clear;
        for (int i = 0; i < Camera.main.orthographicSize; i++) 
        {
            // Range -9.5 to 7
            var platform = Instantiate(startingPrefabs[(int)Mathf.Floor(Random.Range(0, startingPrefabs.Length - 0.01f))], new Vector3(Random.Range(platformHeightDistance-9.5f, 7f), nextPlatformYPos, 0), Quaternion.identity);
            platform.layer = 6;
            nextPlatformYPos = nextPlatformYPos + platformHeightDistance;
        }
    }

    // Set Main Camera Rect View Color
    void DrawOneRow(){
        Camera mainCamera = Camera.main;
        float viewportWidth = mainCamera.pixelWidth;
        float viewportHeight = mainCamera.pixelHeight / 4;

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
