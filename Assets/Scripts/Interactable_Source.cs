using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable_Source : MonoBehaviour
{
    public abstract bool IsEquippable();
    [HideInInspector] public CharacterInventory owner; //which character equipped this object?
}
