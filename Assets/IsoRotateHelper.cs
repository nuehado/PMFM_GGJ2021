using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoRotateHelper : MonoBehaviour
{
    public IsoLook_Properties properties;

    // Start is called before the first frame update
    void Start()
    {
        var eulerRot = new Vector3(properties.xRot, properties.yRot, properties.zRot);

        transform.rotation = Quaternion.Euler(eulerRot);

    }
}
