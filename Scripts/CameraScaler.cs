using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraScaler : MonoBehaviour
{
    // Use this for initializatio
    void Start()
    {
        float TARGET_WIDTH = 800.0f;
        float TARGET_HEIGHT = 600.0f;
        float PIXELS_TO_UNITS = 50;

        float desiredRatio = TARGET_WIDTH / TARGET_HEIGHT;
        float currentRatio = (float)Screen.width / (float)Screen.height;

        if (currentRatio >= desiredRatio)
        {
            // Our resolution has plenty of width, so we just need to use the height to determine the camera size
            Camera.main.orthographicSize = TARGET_HEIGHT / 4 / PIXELS_TO_UNITS;
        }
        else
        {
            // Our camera needs to zoom out further than just fitting in the height of the image.
            // Determine how much bigger it needs to be, then apply that to our original algorithm.
            float differenceInSize = desiredRatio / currentRatio;
            Camera.main.orthographicSize = TARGET_HEIGHT / 4 / PIXELS_TO_UNITS * differenceInSize;
        }

    }

}
