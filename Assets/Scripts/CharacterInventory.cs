using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public CharacterInventory_Properties properties;
    public List<Interactable_Source> inventory;
    private InventoryUI ui;

    private void Start()
    {
        inventory = new List<Interactable_Source>();
        ui = transform.GetComponentInChildren<InventoryUI>();
    }

    public void AddToInventory(Interactable_Source newObject)
    {
        if (inventory.Count >= properties.size || 
            inventory.Contains(newObject) ||
            !newObject.IsEquippable()) //inventory full or duplicate or not equippable
            return;

        inventory.Add(newObject);
        newObject.owner = this;
        newObject.GetComponent<SpriteRenderer>().enabled = false;
        ui.RefreshUI();
    }

    public void AddKindlingToFire(int index, Fire fire)
    {
        if (0 > index || index >= inventory.Count)
        {
            Debug.LogError($"TRIED TO REMOVE ITEM #{index} FROM {gameObject}'S INVENTORY, " +
                $"BUT ONLY {inventory.Count} OBJECTS ARE IN THE INVENTORY");
            return;
        }

        if (!fire.IsRefuelable())
            return;

        if (!(inventory[index] is Kindling)) //make sure that object is kindling
            return;

        Kindling currKindling = inventory[index] as Kindling;
        fire.AddFuel(currKindling.properties.fuelValue);
        RemoveFromInventory(inventory[index]);
    }

    public void RemoveFromInventory(Interactable_Source obj, bool dropped = false)
    {
        inventory.Remove(obj);
        if (dropped) //object discarded, leave it on the ground
        {
            Vector3 spawnLocation = new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), 
                transform.position.y - 0.5f, transform.position.z + Random.Range(-1.5f, 1.5f));
            obj.transform.position = spawnLocation;
            obj.owner = null;
            obj.GetComponent<SpriteRenderer>().enabled = true;
        }
        else //object used, destroy it
        {
            Destroy(obj.gameObject);
        }
        ui.RefreshUI();
    }

    public void AddAllKindlingToFire(Fire fire)
    {
        int nonKindlingItems = 0;
        if (!fire.IsRefuelable())
            return;

        while (inventory.Count > nonKindlingItems)
        {
            if (inventory[nonKindlingItems] is Kindling)
                AddKindlingToFire(nonKindlingItems, fire);
            else
                nonKindlingItems++;
        }
    }
}
