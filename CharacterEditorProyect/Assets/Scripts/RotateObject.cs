using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    private void OnMouseDrag()
    {
        transform.Rotate(-Vector3.up * Input.GetAxis("Mouse X") * 600 * Time.deltaTime);
    }
}
