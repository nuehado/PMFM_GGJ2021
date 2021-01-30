using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoRotateHelper : MonoBehaviour
{
    public IsoLook_Properties properties;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        var eulerRot = new Vector3(properties.xRot, properties.yRot, properties.zRot);

        transform.rotation = Quaternion.Euler(eulerRot);

    }

    private void Update()
    {
        if (properties.updateEveryFrame == true)
        {
            transform.forward = cam.transform.forward;
        }
    }
}
