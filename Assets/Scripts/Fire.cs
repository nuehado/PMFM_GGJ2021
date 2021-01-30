using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Interactable_Source
{
    //all values that would normally be edited in editor should be in Fire_Properties
    public Fire_Properties properties;

    [SerializeField] private float fuel;

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

    public void AddFuel(float amount)
    {
        if (fuel <= 0)
            return;

        fuel += amount;
    }

    private void Extinguish()
    {
        Debug.Log($"{gameObject.name} ran out of fuel!");
    }

    public float GetFireLife()
    {
        return fuel;
    }
}
