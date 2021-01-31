using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    private NavMeshAgent agent = null;
    private Camera cam;

    [HideInInspector] public Interactable_Source heldItem;
    [HideInInspector] public int heldItemIndex = -1;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    private void Update()
    {
        //TODO Add right click task queue system
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.Log("Raycast target: " + hit.collider.gameObject.name);
                Debug.DrawLine(ray.origin, hit.point);

                if (hit.collider.TryGetComponent(out NavMeshAgent character))
                {
                    if ((agent != null))
                    {
                        SelectAgent(agent, false);
                    }
                    agent = character; // if we click a character we select them
                    SelectAgent(agent, true);
                }
                else if (agent != null)
                {
                    agent.SetDestination(hit.point); //a character currently selected, move them to hit
                    
                    if (hit.collider.TryGetComponent(out Interactable_Source interactable))
                    {
                        Debug.Log("calling coroutine from navcontroller");
                        Vector3 targetPos = GetTargetPos(interactable, hit.point);
                        StartCoroutine(agent.GetComponent<InteractionManager>().PollInteractionDistance(interactable, heldItem, targetPos, heldItemIndex));
                    }
                    else if (heldItem == null)
                    {
                        agent.GetComponent<InteractionManager>().ClearTargetInteractables();
                    }
                    else
                    {
                        Vector3 targetPos = GetTargetPos(interactable, hit.point);
                        StartCoroutine(agent.GetComponent<InteractionManager>().PollInteractionDistance(null, heldItem, targetPos, heldItemIndex));
                    }
                }
            }

            if (heldItem != null)
                FindObjectOfType<ActionIconControl>().Disable();
        }
    }

    private void SelectAgent(NavMeshAgent agent, bool toggle)
    {
        if (toggle)
        {
            agent.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            agent.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        agent.GetComponentInChildren<InventoryUI>().ShowUI(toggle);
        heldItem = null;
        heldItemIndex = -1;
    }

    private Vector3 GetTargetPos(Interactable_Source interactable, Vector3 hitPoint)
    {
        if (interactable == null || interactable is Beach)
            return hitPoint;
        else
            return interactable.transform.position;
    }
}
