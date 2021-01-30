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
            addedVelocity += Vector3.left * Time.deltaTime * properties.acceleration;
        if (Input.GetKey(properties.right))
            addedVelocity += Vector3.right * Time.deltaTime * properties.acceleration;
        if (Input.GetKey(properties.forward))
            addedVelocity += Vector3.forward * Time.deltaTime * properties.acceleration;
        if (Input.GetKey(properties.backward))
            addedVelocity += Vector3.back * Time.deltaTime * properties.acceleration;

        addedVelocity = RotateAboutYAxis(addedVelocity, transform.eulerAngles.y * Mathf.Deg2Rad);

        rb.velocity += addedVelocity;
        rb.velocity = new Vector3(
            Mathf.Clamp(rb.velocity.x, -properties.maxSpeed, properties.maxSpeed),
            Mathf.Clamp(rb.velocity.y, -properties.maxSpeed, properties.maxSpeed),
            Mathf.Clamp(rb.velocity.z, -properties.maxSpeed, properties.maxSpeed)
            );
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
