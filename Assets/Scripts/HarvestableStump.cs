using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvestableStump : KindlingSource
{
    [SerializeField] private List<GameObject> harvestables;

    public override bool IsInteractedWith { get; set; }

    public override void HarvestKindling()
    {
        interactTimer = properties.interactionTime;
        IsInteractedWith = true;
        StartCoroutine(ChopItDown());
    }

    IEnumerator ChopItDown()
    {
        if (IsInteractedWith == false)
        {
            yield break;
        }
        yield return new WaitForSeconds(1);
        interactTimer--;

        if (interactTimer <= 0f)
        {
            foreach (GameObject kindling in harvestables)
            {
                Vector3 spawnLocation = new Vector3(transform.position.x + Random.Range(1.5f, 2.5f), 0.5f, transform.position.z + Random.Range(1.5f, 2.5f));
                Instantiate(kindling, spawnLocation, Quaternion.identity);

            }
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(ChopItDown());
        }
        
    }
}
