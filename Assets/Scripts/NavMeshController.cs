using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    private NavMeshAgent agent = null;
    private Camera cam;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    private void Update()
    {
        //TODO Add right click task queue system
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000))
            {
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
                        StartCoroutine(agent.GetComponent<InteractionManager>().PollInteractionDistance(interactable));
                    }
                    else
                    {
                        agent.GetComponent<InteractionManager>().ClearTargetInteractables();
                    }
                }
            }
        }
    }

    private void SelectAgent(NavMeshAgent agent, bool toggle)
    {
        if (toggle)
        {
            agent.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }
        else
        {
            agent.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        agent.GetComponentInChildren<InventoryUI>().ShowUI(toggle);
    }
    
}
