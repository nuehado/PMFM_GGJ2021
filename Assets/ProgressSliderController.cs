using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSliderController : MonoBehaviour
{
    private Canvas canvas;
    private KindlingSource source;
    private Slider progressSlider;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        source = GetComponentInParent<KindlingSource>();
        progressSlider = GetComponentInChildren<Slider>();
        progressSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (source.IsInteractedWith == true)
        {
            progressSlider.gameObject.SetActive(true);
            progressSlider.maxValue = source.properties.interactionTime;
            progressSlider.value = source.properties.interactionTime - source.interactTimer;
        }
        else
        {
            progressSlider.gameObject.SetActive(false);
        }
    }
}
