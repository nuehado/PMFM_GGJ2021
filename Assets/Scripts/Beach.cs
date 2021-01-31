using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beach : Interactable_Source
{
    public override bool IsEquippable()
    {
        return false;
    }

    public bool hasFire = false; //doesn't count people on fire

    private List<CharacterInventory> charactersOnBeach;

    private void Start()
    {
        charactersOnBeach = new List<CharacterInventory>();
    }

    public bool hasAnyFire() //counts both campfires and people on fire
    {
        return hasFire || hasCharacterOnFire();
    }

    private bool hasCharacterOnFire()
    {
        if (charactersOnBeach.Count == 0)
            return false;

        foreach (CharacterInventory character in charactersOnBeach)
        {
            if (character.onFire)
                return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Character"))
        {
            CharacterInventory newChar = collision.gameObject.GetComponent<CharacterInventory>();
            charactersOnBeach.Add(newChar);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Contains("Character"))
        {
            CharacterInventory oldChar = collision.gameObject.GetComponent<CharacterInventory>();
            charactersOnBeach.Remove(oldChar);
        }
    }
}
