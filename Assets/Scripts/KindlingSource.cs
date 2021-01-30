using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KindlingSource : Interactable_Source
{
    public abstract void HarvestKindling();
    public abstract bool IsInteractedWith { get; set; }
    public float interactTimer;

    public KindlingSource_Properties properties;
    public override bool IsEquippable()
    {
        return false;
    }
}
