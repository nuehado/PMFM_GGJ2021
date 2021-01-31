using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Interactable_Source
{
    //all values that would normally be edited in editor should be in Fire_Properties
    public Fire_Properties properties;

    [SerializeField] private float fuel;

    private AudioSource fire_SFX;

    public enum FireType { Default, Oil, Friend };

    private void Start()
    {
        fire_SFX = GetComponent<AudioSource>();
        fuel = properties.startingFuel;
        StartCoroutine(UseFuel());
        if(properties.fireType == FireType.Default)
        {
            fire_SFX.Play();
        }
    }

    private IEnumerator UseFuel()
    {
        while (fuel > 0)
        {
            fuel -= properties.fuelUsageRate * Time.deltaTime;
            yield return null;
        }

        fuel = 0;
        Extinguish();
        yield return null;
    }

    public bool IsRefuelable()
    {
        return fuel > 0 &&
            properties.fireType != FireType.Friend;
    }

    public override bool IsEquippable()
    {
        return properties.fireType == FireType.Oil;
    }

    public void AddFuel(float amount, bool overrideExtinguished = false)
    {
        if (fuel <= 0 && !overrideExtinguished)
            return;

        fuel += amount;

        if (fuel == amount) //brought back from dead
        {
            StartCoroutine(UseFuel());
        }
    }

    private void Extinguish()
    {
        Debug.Log($"{gameObject.name} ran out of fuel!");
        GetNearestBeach().hasFire = false;
        if (AllFiresExtinguished())
        {
            if (OverallGameState.currGameState == OverallGameState.State.Ongoing)
            {
                OverallGameState.currGameState = OverallGameState.State.Lose;
                FindObjectOfType<UIManager>().DisplayLoseScreen();
            }
            Debug.Log("You lost! You Fucked up! You havent handled this inevitable eventuality, you dingus, you absoulte baffooon, you fucking dissapoinment");
        }
        if (properties.fireType == FireType.Default)
        {
            Destroy(this.gameObject);
        }
        else if (properties.fireType == FireType.Oil)
        {
            if (owner != null)
            {
                owner.RemoveFromInventory(this);
            }
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private Beach GetNearestBeach()
    {
        Beach[] beaches = FindObjectsOfType<Beach>();
        Beach closestBeach = beaches[0];
        float closestBeachDist = (transform.position - beaches[0].transform.position).magnitude;
        for (int i = 1; i < beaches.Length; i++)
        {
            float dist = (transform.position - beaches[i].transform.position).magnitude;
            if (dist < closestBeachDist)
            {
                closestBeach = beaches[i];
                closestBeachDist = dist;
            }
        }
        return closestBeach;
    }

    private bool AllFiresExtinguished()
    {
        foreach (Fire fire in FindObjectsOfType<Fire>())
        {
            if (fire.GetFireLife() > 0)
                return false;
        }

        return true;
    }

    public float GetFireLife()
    {
        return fuel;
    }

    public void StartNewFire(Vector3 location)
    {
        if (properties.fireType != FireType.Oil || fuel <= 0)
            return;

        Instantiate(properties.newFirePrefab, location, Quaternion.identity);
        owner.RemoveFromInventory(this as Interactable_Source);
    }
}
