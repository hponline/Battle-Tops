using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private float spinSpeed = 0.2f;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed);
    }
}
