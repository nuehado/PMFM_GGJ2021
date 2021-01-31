using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventoryUI_Properties properties;
    private Canvas canvas;
    private CharacterInventory inventory;
    private InventoryIcon[] icons;
    private Button[] iconButtons;
    private Image[] iconImages;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        DetectUI();
        ShowUI(false);
    }

    private void DetectUI()
    {
        inventory = transform.parent.GetComponent<CharacterInventory>();
        icons = new InventoryIcon[inventory.properties.size];
        iconButtons = new Button[icons.Length];
        iconImages = new Image[icons.Length];
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i] = transform.GetChild(i).GetComponent<InventoryIcon>();
            iconButtons[i] = icons[i].GetComponent<Button>();
            iconImages[i] = icons[i].GetComponent<Image>();
        }
        RefreshUI();
    }

    public void ShowUI(bool toggle)
    {
        canvas.enabled = toggle;
    }

    public void RefreshUI()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (inventory.inventory.Count <= i)
            {
                iconButtons[i].interactable = false;
                iconImages[i].sprite = properties.empty;
            }
            else
            {
                iconButtons[i].interactable = true;
                iconImages[i].sprite = GetImageSprite(inventory.inventory[i]);
            }
        }
    }

    private Sprite GetImageSprite(Interactable_Source obj)
    {
        if (obj is Fire)
        {
            return properties.oilLamp;
        }
        else if (obj is Kindling)
        {
            Kindling kindling = obj as Kindling;
            if (kindling.properties.name == "Log")
                return properties.log;
            else if (kindling.properties.name == "Twig")
                return properties.twig;
            else if (kindling.properties.name == "Coal")
                return properties.coal;
        }

        Debug.Log($"UNKNOWN INVENTORY ITEM: {obj.gameObject.name}");
        return null;
    }
}
