using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    public BoatManager_Properties properties;
    
    private enum IslandEdge { NE, SE, SW, NW };
    private Transform boat;
    private SpriteRenderer boatImg;

    private Vector3 startPos;
    private Vector3 endPos;
    private Beach visibleBeach;
    private Vector3 dockingPoint;

    private void Start()
    {
        StartCoroutine(ManageBoats());
        GameObject boatObj = Instantiate(properties.boatPrefab);
        boat = boatObj.transform;
        boatImg = boatObj.GetComponent<SpriteRenderer>();
        boatObj.SetActive(false);
    }

    private IEnumerator ManageBoats()
    {
        yield return new WaitForSeconds(properties.timeBeforeFirstBoat);

        while (true) //every iteration spawns new boat
        {
            boat.gameObject.SetActive(true);
            IslandEdge edge = PickRandomEdge();
            SetUpBoat(edge);
            float startTime = Time.time;
            while (Time.time <= startTime + properties.boatWaitTime)
            {
                float prog = (Time.time - startTime) / properties.boatWaitTime;
                boat.position = Vector3.Lerp(startPos, endPos, prog);
                if (visibleBeach.hasFire)
                {
                    StartCoroutine(Rescue());
                    yield break;
                }
                yield return null;
            }

            //failed to get to boat
            Vector3 direction = (endPos - startPos).normalized;
            float timeBeforeNextBoat = Random.Range(properties.minTimeBetweenBoats, properties.maxTimeBetweenBoats);
            float speed = (endPos - startPos).magnitude / properties.boatWaitTime;
            startTime = Time.time;
            while (Time.time <= startTime + timeBeforeNextBoat)
            {
                boat.position += direction * Time.deltaTime * speed;
                yield return null;
            }
            yield return null;
        }
    }

    private IEnumerator Rescue()
    {
        //flying boat
        float startTime = Time.time;
        while (Time.time < startTime + properties.timeToReachShore / 2f)
        {
            boat.position += Vector3.up * Time.deltaTime * properties.verticalSpeed;
            yield return null;
        }

        boat.position = new Vector3(dockingPoint.x, boat.position.y, dockingPoint.z);

        //falling boat
        while (Time.time < startTime + properties.timeToReachShore)
        {
            boat.position += Vector3.down * Time.deltaTime * properties.verticalSpeed;
            yield return null;
        }

        boat.position = dockingPoint;
        Debug.Log("YOU WON!");
        yield return null;
    }

    private IslandEdge PickRandomEdge()
    {
        int i = Random.Range(0, 4);
        return (IslandEdge)i;
    }

    private void SetUpBoat(IslandEdge edge)
    {
        float n = properties.distFromCoast + properties.islandRadius;

        switch (edge)
        {
            case IslandEdge.NE:
                SetUpNE(n);
                break;
            case IslandEdge.NW:
                SetUpNW(n);
                break;
            case IslandEdge.SE:
                SetUpSE(n);
                break;
            case IslandEdge.SW:
                SetUpSW(n);
                break;
        }
    }

    private void SetUpNE(float n)
    {
        boatImg.sprite = properties.beachNE;
        startPos = new Vector3(-n, 2.2f, -n);
        endPos = new Vector3(-n, 2.2f, n);
        boat.eulerAngles = properties.rotNE;
        dockingPoint = properties.dockingLocationNE;

        Beach[] beaches = FindObjectsOfType<Beach>();
        visibleBeach = beaches[0];
        float lowestX = beaches[0].transform.position.x;
        for (int i = 1; i < beaches.Length; i++)
        {
            if (beaches[i].transform.position.x < lowestX)
            {
                visibleBeach = beaches[i];
                lowestX = beaches[i].transform.position.x;
            }
        }
    }

    private void SetUpNW(float n)
    {
        boatImg.sprite = properties.beachNW;
        startPos = new Vector3(n, 0.3f, -n);
        endPos = new Vector3(-n, 0.3f, -n);
        boat.eulerAngles = properties.rotNW;
        dockingPoint = properties.dockingLocationNW;

        Beach[] beaches = FindObjectsOfType<Beach>();
        visibleBeach = beaches[0];
        float lowestZ = beaches[0].transform.position.z;
        for (int i = 1; i < beaches.Length; i++)
        {
            if (beaches[i].transform.position.z < lowestZ)
            {
                visibleBeach = beaches[i];
                lowestZ = beaches[i].transform.position.z;
            }
        }
    }

    private void SetUpSE(float n)
    {
        boatImg.sprite = properties.beachSE;
        startPos = new Vector3(-n, 2.6f, n);
        endPos = new Vector3(n, 2.6f, n);
        boat.eulerAngles = properties.rotSE;
        dockingPoint = properties.dockingLocationSE;

        Beach[] beaches = FindObjectsOfType<Beach>();
        visibleBeach = beaches[0];
        float highestZ = beaches[0].transform.position.z;
        for (int i = 1; i < beaches.Length; i++)
        {
            if (beaches[i].transform.position.x > highestZ)
            {
                visibleBeach = beaches[i];
                highestZ = beaches[i].transform.position.z;
            }
        }
    }

    private void SetUpSW(float n)
    {
        boatImg.sprite = properties.beachSW;
        startPos = new Vector3(n, 1.9f, n);
        endPos = new Vector3(n, 1.9f, -n);
        boat.eulerAngles = properties.rotSW;
        dockingPoint = properties.dockingLocationSW;

        Beach[] beaches = FindObjectsOfType<Beach>();
        visibleBeach = beaches[0];
        float highestX = beaches[0].transform.position.x;
        for (int i = 1; i < beaches.Length; i++)
        {
            if (beaches[i].transform.position.x > highestX)
            {
                visibleBeach = beaches[i];
                highestX = beaches[i].transform.position.x;
            }
        }
    }
}

/* NOTES:
 * 
 * The NE beach has the lowest x-value
 * The SE beach has the highest z-value
 * The SW beach has the highest x-value
 * The NW beach has the lowest z-value
 * 
 */