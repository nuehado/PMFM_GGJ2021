using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public CharacterInventory_Properties properties;
    [SerializeField] private List<Kindling> inventory;

    private void Start()
    {
        inventory = new List<Kindling>();
    }

    public void AddToInventory(Kindling newKindling)
    {
        if (inventory.Count >= properties.size || inventory.Contains(newKindling)) //inventory full or duplicate
            return;

        inventory.Add(newKindling);
        newKindling.gameObject.SetActive(false);
    }

    public void AddKindlingToFire(int index, Fire fire)
    {
        if (0 > index || index >= inventory.Count)
        {
            Debug.LogError($"TRIED TO REMOVE ITEM #{index} FROM {gameObject}'S INVENTORY, " +
                $"BUT ONLY {inventory.Count} OBJECTS ARE IN THE INVENTORY");
            return;
        }

        Kindling currKindling = inventory[index];
        fire.AddFuel(currKindling.properties.fuelValue);
        inventory.RemoveAt(index);
        Destroy(currKindling.gameObject);
    }

    public void AddAllKindlingToFire(Fire fire)
    {
        while (inventory.Count > 0)
        {
            AddKindlingToFire(0, fire);
        }
    }
}
