using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private CharacterInventory inventory;
    private Interactable_Source currentInteractable;

    private void Start()
    {
        inventory = GetComponent<CharacterInventory>();
    }

    public IEnumerator PollInteractionDistance(Interactable_Source interactable)
    {
        Debug.Log("Headed towards interactable");
        
        if (Vector3.Distance(transform.position, interactable.transform.position) < 3)
        {
            if (interactable is Kindling kindling)
            {
                inventory.AddToInventory(kindling);
            }

            else if (interactable is Fire fire)
            {
                inventory.AddAllKindlingToFire(fire);
            }

            else if (interactable is KindlingSource tree)
            {
                currentInteractable = tree;
                tree.HarvestKindling();
            }
            StopAllCoroutines();
            yield break;
        }

        yield return new WaitForSeconds(0.2f);
        Debug.Log("calling coroutine from coroutine");
        StartCoroutine(PollInteractionDistance(interactable));
    }

    internal void ClearTargetInteractables()
    {
        if (currentInteractable is KindlingSource source)
        {
            source.IsInteractedWith = false;
        }
        Debug.Log("Headed towards random spot");
        StopAllCoroutines();
    }
}
