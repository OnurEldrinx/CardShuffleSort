using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Slot fromSlot;
    public Slot toSlot;

    public Dictionary<Slot,Slot> fromToPair;

    private void Start()
    {
        fromToPair = new Dictionary<Slot, Slot>();
    }
}
