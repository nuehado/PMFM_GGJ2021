using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Image currUI;
    private Image uiNE;
    private Image uiNW;
    private Image uiSW;
    private Image uiSE;

    int boatsSummoned = 0;
    private AudioSource horn_SFX;
    [SerializeField] UIManager uIManager;

    private void Start()
    {
        StartCoroutine(ManageBoats());
        GameObject boatObj = Instantiate(properties.boatPrefab);
        boat = boatObj.transform;
        boatImg = boatObj.GetComponent<SpriteRenderer>();
        boatObj.SetActive(false);
        uiNW = GameObject.Find("BoatLT").GetComponent<Image>();
        uiNE = GameObject.Find("BoatRT").GetComponent<Image>();
        uiSW = GameObject.Find("BoatLB").GetComponent<Image>();
        uiSE = GameObject.Find("BoatRB").GetComponent<Image>();
        uiNW.enabled = false;
        uiNE.enabled = false;
        uiSW.enabled = false;
        uiSE.enabled = false;

        horn_SFX = GetComponent<AudioSource>();
    }

    private IEnumerator ManageBoats()
    {
        yield return new WaitForSeconds(properties.timeBeforeFirstBoat);

        while (true) //every iteration spawns new boat
        {
            boat.gameObject.SetActive(true);
            IslandEdge edge = PickRandomEdge();
            SetUpBoat(edge);
            currUI.enabled = true;
            float startTime = Time.time;
            while (Time.time <= startTime + properties.boatWaitTime)
            {
                float prog = (Time.time - startTime) / properties.boatWaitTime;
                boat.position = Vector3.Lerp(startPos, endPos, prog);
                if (visibleBeach.hasAnyFire())
                {
                    StartCoroutine(Rescue());
                    yield break;
                }
                yield return null;
            }

            //failed to get to boat
            currUI.enabled = false;
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
        if (OverallGameState.currGameState == OverallGameState.State.Ongoing)
        {
            OverallGameState.currGameState = OverallGameState.State.Win;
            Debug.Log("YOU WON!");
            uIManager.DisplayWin();
        }
        
        yield return null;
    }

    private IslandEdge PickRandomEdge()
    {
        int i = Random.Range(0, 4);
        IslandEdge proposedEdge = (IslandEdge)i;
        if (boatsSummoned > 0 || proposedEdge != IslandEdge.SE)
        {
            boatsSummoned++;
            return proposedEdge;
        }
        else
        {
            return PickRandomEdge();
        }
    }

    private void SetUpBoat(IslandEdge edge)
    {

        horn_SFX.Play();
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
        currUI = uiNE;

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
        currUI = uiNW;

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
        currUI = uiSE;

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
        currUI = uiSW;

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