using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    
    Camera c;
    bool canRotateCam = true;
    float speed = 10f;
    Vector3 angle;
    float zoom = 20;
    void Start()
    {
        c = Camera.main;
        startPos = transform.position;
        rot = transform.rotation;
    }
    void Update()
    {
        if (c.orthographic)
        {
            zoom += Input.mouseScrollDelta.y;
            zoom = Mathf.Min(Mathf.Max(zoom, 1), 100);
            c.orthographicSize = zoom;
        }

        speed += Input.mouseScrollDelta.y;
        Vector3 v = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Upward"), Input.GetAxis("Vertical"));
        Vector3 r = new Vector3(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
        }
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            angle.x += r.x * 90 * Time.deltaTime;
            angle.y += r.y * 90 * Time.deltaTime;

            transform.rotation = Quaternion.Euler(angle.y, angle.x, 0);
        }
        transform.position += (transform.rotation * v) * speed * Time.deltaTime;
    }
    public void doRotateCam(bool b) { 
        canRotateCam = b;
    }
    Vector3 startPos;
    Quaternion rot;
    public void ResetPos()
    {
        transform.position = startPos;
        transform.rotation = rot;
        canRotateCam = false;
    }
    
    
   
}
