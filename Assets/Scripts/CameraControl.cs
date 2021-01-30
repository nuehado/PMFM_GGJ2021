using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public CameraControl_Properties properties;

    private Rigidbody rb;
    private bool movementAllowed = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = properties.drag;
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (!movementAllowed)
            return;

        Vector3 addedVelocity = Vector3.zero;
        if (Input.GetKey(properties.left))
            addedVelocity += Vector3.left;
        if (Input.GetKey(properties.right))
            addedVelocity += Vector3.right;
        if (Input.GetKey(properties.forward))
            addedVelocity += Vector3.forward;
        if (Input.GetKey(properties.backward))
            addedVelocity += Vector3.back;
        addedVelocity = addedVelocity.normalized * Time.deltaTime * properties.acceleration;

        addedVelocity = RotateAboutYAxis(addedVelocity, transform.eulerAngles.y * Mathf.Deg2Rad);

        rb.velocity += addedVelocity;
        if (rb.velocity.magnitude >properties.maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * properties.maxSpeed;
        }
    }

    Vector3 RotateAboutYAxis(Vector3 v, float angle)
    {
        float r = v.magnitude;
        float currAngle = Mathf.Atan2(v.z, v.x);
        float newAngle = currAngle - angle;
        float x = r * Mathf.Cos(newAngle);
        float z = r * Mathf.Sin(newAngle);
        return new Vector3(x, v.y, z);
    }
}
