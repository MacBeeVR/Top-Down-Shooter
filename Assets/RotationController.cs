using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour {

    //Rotates the object 5 degrees horizontally every update.
    void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(0f, 5f, 0f);
    }
}
