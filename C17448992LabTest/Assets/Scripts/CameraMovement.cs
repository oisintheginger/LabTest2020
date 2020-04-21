using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Range(1f, 8f)]
    [SerializeField] float ColorGenerationSpeed, ColorBlendSpeed;
    [SerializeField] float rotX, rotY, Zoom;
    float CameraX;
    float CameraY;
    float zoomAmount = 60;

    float CameraR, CameraG, CameraB;

    Color32 newCol;

    private void Awake()
    {
        newCol = new Color(Random.Range(0, 256), Random.Range(0, 256), Random.Range(0, 256));
        InvokeRepeating("ColorRandomiser", 0f, ColorGenerationSpeed);
    }
    private void Update()
    {
        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, newCol, Time.deltaTime * ColorBlendSpeed);

        CameraX += Time.deltaTime * rotX * Input.GetAxis("Vertical");
        CameraY += Time.deltaTime * rotY * Input.GetAxis("Horizontal");

        CameraX = Mathf.Clamp(CameraX, 15f, 90f);

        zoomAmount += Zoom * -Input.mouseScrollDelta.y;
        zoomAmount = Mathf.Clamp(zoomAmount, 20f, 80f);
        Camera.main.fieldOfView = zoomAmount;
        this.transform.rotation = Quaternion.Euler(CameraX, CameraY, 0f);

    }

    void ColorRandomiser()
    { 
        CameraR = Random.Range(100, 256);
        CameraG = Random.Range(100, 256);
        CameraB = Random.Range(100, 256);
        newCol.r = (byte)CameraR;
        newCol.g = (byte)CameraG;
        newCol.b = (byte)CameraB;
    }
}
