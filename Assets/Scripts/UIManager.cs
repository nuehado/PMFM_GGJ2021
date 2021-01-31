using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image fireFillTL;
    [SerializeField] Image fireFillBL;
    [SerializeField] Image fireFillTR;
    [SerializeField] Image fireFillBR;

    bool isPaused = false;

    [SerializeField] Beach beachTL;
    [SerializeField] Beach beachBL;
    [SerializeField] Beach beachTR;
    [SerializeField] Beach beachBR;

    List<Beach> beaches = new List<Beach>();
    List<Fire> fires = new List<Fire>();

    [SerializeField] GameObject optionsMenu;
    [SerializeField] NavMeshController nav_raycaster;

    private void Start()
    {
        beaches.Add(beachTL);
        beaches.Add(beachTR);
        beaches.Add(beachBL);
        beaches.Add(beachBR);

        //GetFires();
        //CheckFiresFuel;
        //UpdateFireUI();
        //BoatsHere();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;

                optionsMenu.SetActive(true);
                nav_raycaster.rayDistance = 10;
            }
            else
            {
                ResumeGame();
            }

        }
        
        
        UpdateFireUI();


    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        optionsMenu.SetActive(false);
        nav_raycaster.rayDistance = 1000;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void UpdateFireUI()
    {
        FindFires();
        GetBeach();
    }

    private void FindFires()
    {
        if (FindObjectsOfType<Fire>() == null)
        {
            Debug.Log("You LOSe!");
            return;
        }

        fires.Clear();
        foreach (Fire fire in FindObjectsOfType<Fire>())
        {
            if (fires.Contains(fire) == false && fire.properties.fireType == Fire.FireType.Default)
            {
                fires.Add(fire);
            }
        }
    }

    private void GetBeach()
    {
        foreach(Fire fire in fires)
        {
            float beachDistance = Mathf.Infinity;
            Beach closestBeach = null;
            for (int i = 0; i < beaches.Count; i++)
            {
                if (Vector3.Distance(fire.transform.position, beaches[i].transform.position) < beachDistance)
                {
                    beachDistance = Vector3.Distance(fire.transform.position, beaches[i].transform.position);
                    closestBeach = beaches[i];
                }
            }

            if (closestBeach == null)
            {
                return;
            }
            else if(closestBeach == beachTL)
            {
                fireFillTL.fillAmount = 0.01f * fire.GetFireLife();
            }
            else if (closestBeach == beachTR)
            {
                fireFillTR.fillAmount = 0.01f * fire.GetFireLife();
            }
            else if (closestBeach == beachBL)
            {
                fireFillBL.fillAmount = 0.01f * fire.GetFireLife();
            }
            else if (closestBeach == beachBR)
            {
                fireFillBR.fillAmount = 0.01f * fire.GetFireLife();
            }
        }
    }
}
