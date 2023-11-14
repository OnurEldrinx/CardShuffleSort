using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Slot fromSlot;
    public Slot toSlot;

    public Dictionary<Slot,Slot> fromToPair;

    // Animation Settings
    public float totalDuration;
    public float height;
    public Ease ease;
    
    
    private void Start()
    {
        fromToPair = new Dictionary<Slot, Slot>();
    }
}
