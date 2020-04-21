using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float rotX, rotY, Zoom;
    float CameraX;
    float CameraY;
    float zoomAmount = 60;
    private void Update()
    {
        CameraX += Time.deltaTime * rotX * Input.GetAxis("Vertical");
        CameraY += Time.deltaTime * rotY * Input.GetAxis("Horizontal");

        CameraX = Mathf.Clamp(CameraX, 30f, 90f);

        zoomAmount += Zoom * -Input.mouseScrollDelta.y;
        zoomAmount = Mathf.Clamp(zoomAmount, 40f, 80f);
        Camera.main.fieldOfView = zoomAmount;
        this.transform.rotation = Quaternion.Euler(CameraX, CameraY, 0f);

    }
}
