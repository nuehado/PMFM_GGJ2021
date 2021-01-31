using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterInventory_Properties : ScriptableObject
{
    public int size; //max number of objects allowed in inventory
    public float burnRecuperationSpeed; //how quickly do they regain health after being burned compared to the speed of losing health during burning?
}
