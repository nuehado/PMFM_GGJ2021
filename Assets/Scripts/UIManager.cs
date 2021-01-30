using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    List<Fire> fires = new List<Fire>();
    [SerializeField] TMP_Text fire1Life;
    [SerializeField] TMP_Text fire2Life;
    [SerializeField] TMP_Text gameTimer;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Fire fire in FindObjectsOfType<Fire>())
        {
            fires.Add(fire);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        fire1Life.text = $"fire1 Life {Mathf.Round(fires[0].GetFireLife())}";
        fire2Life.text = $"fire2 Life {Mathf.Round(fires[1].GetFireLife())}";
        timer += Time.deltaTime;
        gameTimer.text = Mathf.Round(timer).ToString();
    }
}
