using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Interactable_Source
{
    //all values that would normally be edited in editor should be in Fire_Properties
    public Fire_Properties properties;

    [SerializeField] private float fuel;

    public enum FireType { Default, Oil, Friend };

    private void Start()
    {
        fuel = properties.startingFuel;
        StartCoroutine(UseFuel());
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

    public void AddFuel(float amount)
    {
        if (fuel <= 0)
            return;

        fuel += amount;
    }

    private void Extinguish()
    {
        Debug.Log($"{gameObject.name} ran out of fuel!");
        GetNearestBeach().hasFire = false;
        if (AllFiresExtinguished())
        {
            Debug.Log("You lost! You Fucked up! You havent handled this inevitable eventuality, you dingus, you absoulte baffooon, you fucking dissapoinment");
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
