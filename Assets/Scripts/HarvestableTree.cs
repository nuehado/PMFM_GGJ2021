﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HarvestableTree : KindlingSource
{
    [SerializeField] private List<GameObject> harvestables;
    [SerializeField] private GameObject stump;

    public override void HarvestKindling()
    {
        foreach (GameObject kindling in harvestables)
        {
            Vector3 spawnLocation = new Vector3(transform.position.x + Random.Range(1.5f, 2.5f), transform.position.y -1 , transform.position.z + Random.Range(1.5f, 2.5f));
            Instantiate(kindling, spawnLocation, Quaternion.identity);

        }
        Instantiate(stump, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
