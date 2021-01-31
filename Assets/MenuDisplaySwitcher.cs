using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplaySwitcher : MonoBehaviour
{
    [SerializeField] GameObject guideInfo;
    [SerializeField] GameObject controlsInfo;

    public bool isControlsUp = false;

    public void SwitchDisplay()
    {
        guideInfo.SetActive(isControlsUp);
        controlsInfo.SetActive(!isControlsUp);

        isControlsUp = !isControlsUp;
    }
}
