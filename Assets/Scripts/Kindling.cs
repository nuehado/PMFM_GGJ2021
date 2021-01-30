using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kindling : Interactable_Source
{
    public Kindling_Properties properties;

    public override bool IsEquippable()
    {
        return true;
    }
}
