using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform pivot;
    [SerializeField] float speed;
    [SerializeField] float zoomScale;
    [SerializeField] float spinSpeed;
    [SerializeField] float minY, maxY;
    [SerializeField] float minX, maxX, minZ, maxZ;
    [SerializeField] float minRotX, maxRotX;
    [SerializeField] float rotationSpeed;
    void Update()
    {
        GetInput();
    }
    private void GetInput()
    {
        //Camera input
        if (Input.GetAxisRaw("Vertical")>0)
        {
            transform.Translate(speed * Time.deltaTime * pivot.forward, Space.World);
        }
        if (Input.GetAxisRaw("Horizontal")>0)
        {
            transform.Translate(speed * Time.deltaTime * pivot.right, Space.World);
        }
        if (Input.GetAxisRaw("Vertical")<0)
        {
            transform.Translate(speed * Time.deltaTime * -pivot.forward, Space.World);
        }
        if (Input.GetAxisRaw("Horizontal")<0)
        {
            transform.Translate(speed * Time.deltaTime * -pivot.right, Space.World);
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            Vector3 position = Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime * new Vector3(0f, -zoomScale, 0f);
            transform.position += position;
            transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x - rotationSpeed * Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime, minRotX, maxRotX), transform.localEulerAngles.y, 0f);
        }
        if(Input.GetKey(KeyCode.Q))
        {
            pivot.transform.Rotate(0f, -spinSpeed * Time.deltaTime, 0f);
            transform.Rotate(0f, -spinSpeed * Time.deltaTime, 0f, Space.World);
        }
        else if(Input.GetKey(KeyCode.E))
        {
            pivot.transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
            transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), Mathf.Clamp(transform.position.z, minZ, maxZ));
    }
}
