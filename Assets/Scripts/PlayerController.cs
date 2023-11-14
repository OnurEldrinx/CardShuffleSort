using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Slot fromSlot;
    public Slot toSlot;
    
    // Animation Settings
    public float totalDuration;
    public float height;
    public Ease ease;
    
    
}
