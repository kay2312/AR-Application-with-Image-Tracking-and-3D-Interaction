using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TigerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private FixedJoystick fixedJoystick;
    private Rigidbody rigidBody;
    private void OnEnable()
    {
        fixedJoystick = FindAnyObjectByType<FixedJoystick>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        float xVal = fixedJoystick.Horizontal;
        float yVal = fixedJoystick.Vertical;
        Vector3 movement = new Vector3(xVal, 0, yVal);
        rigidBody.linearVelocity = movement * speed;
        if (xVal !=0 && yVal !=0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
            Mathf.Atan2(xVal, yVal) * Mathf.Rad2Deg,
            transform.eulerAngles.z);
        } 
    }
}