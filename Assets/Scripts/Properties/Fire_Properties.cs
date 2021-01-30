using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Fire_Properties : ScriptableObject
{
    public float startingFuel;
    public float fuelUsageRate; //fuel used per second
    public Fire.FireType fireType;
    public GameObject newFirePrefab; //only necessary for oil lamp
}
