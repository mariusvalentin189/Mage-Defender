using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementMobile : MonoBehaviour
{
    [SerializeField] Transform pivot;
    [SerializeField] float speed;
    [SerializeField] float zoomScale;
    [SerializeField] float spinSpeed;
    [SerializeField] float minY, maxY;
    [SerializeField] float minX, maxX, minZ, maxZ;
    [SerializeField] float minRotX, maxRotX;
    [SerializeField] float rotationSpeed;

    Vector2 initPos1 = Vector2.zero;
    Vector2 initPos2 = Vector2.zero;
    void Update()
    {
        GetInput();
    }
    private void GetInput()
    {
        /*
        //Camera input
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            transform.Translate(speed * Time.deltaTime * pivot.forward, Space.World);
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.Translate(speed * Time.deltaTime * pivot.right, Space.World);
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            transform.Translate(speed * Time.deltaTime * -pivot.forward, Space.World);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.Translate(speed * Time.deltaTime * -pivot.right, Space.World);
        }
        */

        GetTouchInput();
    }

    public void GetTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touchInput1 = Input.GetTouch(0);
            if (touchInput1.position.x < initPos1.x)
            {
                pivot.transform.Rotate(0f, -spinSpeed * Time.deltaTime, 0f);
                transform.Rotate(0f, -spinSpeed * Time.deltaTime, 0f, Space.World);
                initPos1 = touchInput1.position;
            }
            else if (touchInput1.position.x > initPos1.x)
            {
                pivot.transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
                transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
                initPos1 = touchInput1.position;
            }
        }
        else if(Input.touchCount==2)
        {
            Touch touchInput1 = Input.GetTouch(0);
            Touch touchInput2 = Input.GetTouch(1);
            if (touchInput1.phase == TouchPhase.Began || touchInput2.phase == TouchPhase.Began)
            {
                initPos1 = touchInput1.position;
                initPos2 = touchInput2.position;
                if(initPos1.x > initPos2.x)
                {
                    Vector2 aux = initPos1;
                    initPos1 = initPos2;
                    initPos2 = aux;
                }
            }
            else if (touchInput1.phase == TouchPhase.Moved)
            {
                if (touchInput1.position.x < initPos1.x && touchInput2.position.x > initPos2.x)
                {
                    Vector3 position = Time.deltaTime * new Vector3(0f, -zoomScale, 0f);
                    transform.position += position;
                    transform.localEulerAngles += new Vector3(-rotationSpeed, 0f, 0f);
                    transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x, minRotX, maxRotX), transform.localEulerAngles.y, 0f);
                    initPos1 = touchInput1.position;
                    initPos2 = touchInput2.position;
                }
                else if (touchInput1.position.x > initPos1.x && touchInput2.position.x < initPos2.x)
                {
                    Vector3 position = Time.deltaTime * new Vector3(0f, zoomScale, 0f);
                    transform.position += position;
                    transform.localEulerAngles += new Vector3(rotationSpeed, 0f, 0f);
                    transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x, minRotX, maxRotX), transform.localEulerAngles.y, 0f);
                    initPos1 = touchInput1.position;
                    initPos2 = touchInput2.position;
                }
            }
            else if (touchInput1.phase == TouchPhase.Ended || touchInput2.phase == TouchPhase.Ended)
            {
                initPos1 = Vector2.zero;
                initPos2 = Vector2.zero;
            }
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), Mathf.Clamp(transform.position.z, minZ, maxZ));
        }
    }
}
