using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationControl : MonoBehaviour
{
    Camera cam;
    Vector3 isoDirection;
    NavMeshAgent agent;
    Animator animator;
    SpriteRenderer sprite;
    private void Start()
    {
        cam = Camera.main;
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        AnimateCharacter();
    }

    private void AnimateCharacter()
    {
        isoDirection = cam.transform.forward;
        transform.forward = isoDirection;
        var heading = transform.InverseTransformPoint(agent.nextPosition);
        Debug.Log(heading);
        animator.SetFloat("Speed", agent.velocity.magnitude);
        if (heading.x > 0.01)
        {
            sprite.flipX = false;
        }
        else if (heading.x < 0.01)
        {
            sprite.flipX = true;
        }
    }
}
