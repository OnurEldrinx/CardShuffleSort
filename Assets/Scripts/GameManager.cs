using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private List<Slot> activeSlots;

    public List<Colour> cardColors;

    public bool degradeResolution;
    
    public List<Slot> GetActiveSlots()
    {
        return activeSlots;
    }

    




}
