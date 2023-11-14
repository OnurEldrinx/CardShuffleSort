using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Sprite blackFill;
    public Sprite greenFill;
    public Sprite redFill;
    public Sprite blueFill;
    public Sprite yellowFill;

    public Dictionary<Colour, Sprite> fillerSprites;

    private void Start()
    {
        fillerSprites = new Dictionary<Colour, Sprite>
        {
            { Colour.Black, blackFill },
            { Colour.Green, greenFill },
            { Colour.Red, redFill },
            { Colour.Blue, blueFill },
            { Colour.Yellow, yellowFill }
        };
    }
}
