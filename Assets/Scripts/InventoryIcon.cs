using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour
{
    public InventoryIcon_Properties properties;
    private CharacterInventory inventory;
    private Transform canvas;
    private NavMeshAgent agent;

    void Start()
    {
        inventory = transform.parent.parent.GetComponentInParent<CharacterInventory>();
        canvas = GameObject.Find("Canvas").transform;
        agent = GetComponentInParent<NavMeshAgent>();
    }

    public void UseInventoryItem(int index)
    {
        if (inventory.inventory.Count <= index)
            return;

        Interactable_Source obj = inventory.inventory[index];
        GameObject newIcon = Instantiate(properties.iconInteractionPrefab);
        newIcon.transform.SetParent(canvas);
        newIcon.GetComponent<Image>().sprite = GetSprite(obj);
        ActionIconControl actionIcon = newIcon.GetComponent<ActionIconControl>();
        actionIcon.obj = obj;
        actionIcon.inventoryIndex = index;
        actionIcon.Enable();
    }

    private Sprite GetSprite(Interactable_Source obj)
    {
        if (obj is Fire)
            return properties.oilLamp;
        else if (obj is Kindling)
        {
            Kindling kindling = obj as Kindling;
            if (kindling.properties.name == "Log")
                return properties.log;
            else if (kindling.properties.name == "Twig")
                return properties.twig;
            else if (kindling.properties.name == "Coal")
                return properties.coal;
        }

        return null;
    }

    public void StopMoving()
    {
        agent.SetDestination(agent.transform.position);
    }
}
