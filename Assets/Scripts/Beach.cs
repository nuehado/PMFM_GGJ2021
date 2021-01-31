using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beach : Interactable_Source
{
    public override bool IsEquippable()
    {
        return false;
    }

    public bool hasFire = false;
}
