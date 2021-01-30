using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KindlingSource : Interactable_Source
{
    public abstract void HarvestKindling();

    public override bool IsEquippable()
    {
        return false;
    }
}
