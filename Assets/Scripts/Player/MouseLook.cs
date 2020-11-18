using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cam;
    public float mouseSensitivity = 100f;

    Vector3 toLook;

    void Start()
    {
        
    }
    
    void Update()
    {
        Look();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        toLook.y += mouseX;
        toLook.x -= mouseY;

        toLook.x = Mathf.Clamp(toLook.x, -90, 90);

        Quaternion yView = Quaternion.Euler(0, toLook.y, 0);
        this.transform.rotation = yView;

        Quaternion camView = Quaternion.Euler(toLook.x, toLook.y, 0);
        cam.transform.rotation = camView;
    }
}
