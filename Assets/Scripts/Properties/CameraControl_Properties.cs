using UnityEngine;

[CreateAssetMenu]
public class CameraControl_Properties : ScriptableObject
{
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode rotateLeft;
    public KeyCode rotateRight;

    public float drag;
    public float acceleration;
    public float maxSpeed;
}
