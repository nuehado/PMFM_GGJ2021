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
                        agent.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
                    }
                    agent = character; // if we click a character we select them
                    agent.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                }
                else if (agent != null)
                {
                    agent.SetDestination(hit.point); //a character currently selected, move them to hit

                    if (hit.collider.TryGetComponent(out Kindling kindling))
                    {
                        Debug.Log($"{gameObject} is getting some kindling");
                        //TODO start checking if we've reached the kindling
                    }

                    else if (hit.collider.TryGetComponent(out Fire fire))
                    {
                        Debug.Log($"{gameObject} is going to a fire");
                        //TODO start checking if we've reached the fire
                    }
                }
            }
        }
    }
}
