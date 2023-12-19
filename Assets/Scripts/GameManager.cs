using System.Collections.Generic;
using System.Linq;
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

    public void UnlockNewColor()
    {
        var allColors = VFXManager.Instance.colorMap.Keys.ToList();

        allColors.RemoveAll(colour => cardColors.Contains(colour));

        if (allColors.Count <= 0) return;

        int random = Random.Range(0, allColors.Count);

        cardColors.Add(allColors[random]);

    }
    




}
