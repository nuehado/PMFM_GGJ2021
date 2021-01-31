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

    public IEnumerator PollInteractionDistance(Interactable_Source interactable, Interactable_Source heldItem, Vector3 targetPos, int i)
    {
        Debug.Log("Headed towards interactable");
        
        if (Vector3.Distance(transform.position, targetPos) < 3)
        {
            Debug.Log($"Destination reached. Interactable: {interactable}. Held item: {heldItem}");
            if (heldItem == null)
            {
                InteractWithObject(interactable);
            }
            else
            {
                UseObjectOnObject(interactable, heldItem, i);
            }
            StopAllCoroutines();
            yield break;
        }

        yield return new WaitForSeconds(0.2f);
        Debug.Log("calling coroutine from coroutine");
        StartCoroutine(PollInteractionDistance(interactable, heldItem, targetPos, i));
    }

    private void InteractWithObject(Interactable_Source interactable)
    {
        if (interactable is Kindling kindling)
        {
            inventory.AddToInventory(kindling);
        }

        else if (interactable is Fire fire)
        {
            if (fire.properties.fireType == Fire.FireType.Oil)
            {
                inventory.AddToInventory(interactable);
            }
            else if (fire.properties.fireType == Fire.FireType.Default)
            {
                inventory.AddAllKindlingToFire(fire);
            }
        }

        else if (interactable is KindlingSource tree)
        {
            currentInteractable = tree;
            tree.HarvestKindling();
        }
    }

    private void UseObjectOnObject(Interactable_Source interactable, Interactable_Source heldItem, int i)
    {
        if (heldItem is Fire)
        {
            if (interactable is Beach) //set up new fire if on beach
            {
                Beach beach = interactable as Beach;
                Fire fire = heldItem as Fire;
                if (!beach.hasFire)
                {
                    Vector3 spawnLocation = new Vector3(transform.position.x + UnityEngine.Random.Range(-1.5f, 1.5f),
                        transform.position.y, transform.position.z + UnityEngine.Random.Range(-1.5f, 1.5f));
                    fire.StartNewFire(spawnLocation);
                    beach.hasFire = true;
                }
                else
                {
                    GetComponent<CharacterInventory>().RemoveFromInventory(heldItem, true);
                }
            }
            else //drop item if not on beach
            {
                GetComponent<CharacterInventory>().RemoveFromInventory(heldItem, true);
            }
        }
        else if (heldItem is Kindling)
        {
            if (interactable is Fire) //add kindling to fire
            {
                Fire fire = interactable as Fire;
                GetComponent<CharacterInventory>().AddKindlingToFire(i, fire);
            }
            else //drop kindling if not by fire
            {
                GetComponent<CharacterInventory>().RemoveFromInventory(heldItem, true);
            }
        }
    }

    internal void ClearTargetInteractables()
    {
        if (currentInteractable is KindlingSource source)
        {
            source.IsInteractedWith = false;
        }

        StopAllCoroutines();
    }
}
