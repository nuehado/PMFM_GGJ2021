﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HarvestableTree : KindlingSource
{
    [SerializeField] private List<GameObject> harvestables;
    [SerializeField] private GameObject stump;

    private AudioSource chopping_SFX;
    private AudioSource fellTree_SFX;

    public override bool IsInteractedWith { get; set; } = false;

    private void Start()
    {
        chopping_SFX = GetComponent<AudioSource>();
        fellTree_SFX = GetComponentInParent<TreeLocation>().GetComponentInParent<AudioSource>();
    }

    public override void HarvestKindling()
    {
        interactTimer = properties.interactionTime;
        IsInteractedWith = true;
        StartCoroutine(ChopItSmaller());
    }

    private void OnEnable()
    {
        IsInteractedWith = false;
    }

    IEnumerator ChopItSmaller()
    {
        if (IsInteractedWith == false)
        {
            yield break;
        }

        yield return new WaitForSeconds(1);
        interactTimer--;
        
        if (interactTimer <= 0f)
        {
            fellTree_SFX.Play();
            foreach (GameObject kindling in harvestables)
            {
                Vector3 spawnLocation = new Vector3(transform.position.x + Random.Range(0.5f, 1.5f), 0.5f, transform.position.z + Random.Range(0.5f, 1.5f));
                Instantiate(kindling, spawnLocation, Quaternion.identity);

            }
            Vector3 stumpSpawn = new Vector3(transform.position.x, 0.5f, transform.position.z);
            Instantiate(stump, stumpSpawn, Quaternion.identity, GetComponentInParent<TreeLocation>().transform);
            gameObject.SetActive(false);
        }
        else
        {
            chopping_SFX.Play();
            StartCoroutine(ChopItSmaller());
        }
    }
}
