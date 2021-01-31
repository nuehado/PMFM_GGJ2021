using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplaySwitcher : MonoBehaviour
{
    [SerializeField] GameObject guideInfo;
    [SerializeField] GameObject guideInfo2;
    [SerializeField] GameObject controlsInfo;

    [SerializeField] List<GameObject> tutorials = new List<GameObject>();

    int i = 0;

    public void SwitchTutorialDisplay()
    {
        i ++;

        if (i >= tutorials.Count)
        {
            i = 0;
        }

        for (int j = 0; j < tutorials.Count; j++)
        {
            if (i == j)
            {
                tutorials[j].SetActive(true);
            }
            else
            {
                tutorials[j].SetActive(false);
            }
        }
    }
}
