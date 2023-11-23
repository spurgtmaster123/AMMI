using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Acceleration : MonoBehaviour
{
    public InputActionProperty accelerationProperty;

    public Vector3 acceleration { get; private set; } = Vector3.zero;

    private void Update()
    {
        acceleration = accelerationProperty.action.ReadValue<Vector3>();

        Debug.Log(acceleration);
    }



}
