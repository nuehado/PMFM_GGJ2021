using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour
{
    private CharacterInventory inventory;

    void Start()
    {
        inventory = transform.parent.parent.GetComponent<CharacterInventory>();
    }
}
