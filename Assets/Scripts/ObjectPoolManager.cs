using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{

    [Space(10)]

    [Header("VFX Pool Settings")]
    public List<ParticleSystem> splashPool;
    public ParticleSystem splashPrefab;
    public int splashPoolSize;

    [Space(10)] [Header("Card Pool Settings")]
    public List<Card> cardPrefabs; // Red, Yellow, Blue, Green, Black -> Index Order
    public List<Card> redCardPool;
    public List<Card> yellowCardPool;
    public List<Card> blueCardPool;
    public List<Card> greenCardPool;
    public List<Card> blackCardPool;
    public int cardPoolSize;
    
    private void Start()
    {

        splashPool = new List<ParticleSystem>();
        for(var i = 0;i < splashPoolSize; i++)
        {
            var temp = Instantiate(splashPrefab);
            temp.gameObject.SetActive(false);
            splashPool.Add(temp);
        }
        
        redCardPool = new List<Card>();
        for(var i = 0;i < cardPoolSize; i++)
        {
            var temp = Instantiate(cardPrefabs[0]);
            temp.gameObject.SetActive(false);
            redCardPool.Add(temp);
        }
        
        yellowCardPool = new List<Card>();
        for(var i = 0;i < cardPoolSize; i++)
        {
            var temp = Instantiate(cardPrefabs[1]);
            temp.gameObject.SetActive(false);
            yellowCardPool.Add(temp);
        }
        
        blueCardPool = new List<Card>();
        for(var i = 0;i < cardPoolSize; i++)
        {
            var temp = Instantiate(cardPrefabs[2]);
            temp.gameObject.SetActive(false);
            blueCardPool.Add(temp);
        }
        
        greenCardPool = new List<Card>();
        for(var i = 0;i < cardPoolSize; i++)
        {
            var temp = Instantiate(cardPrefabs[3]);
            temp.gameObject.SetActive(false);
            greenCardPool.Add(temp);
        }
        
        blackCardPool = new List<Card>();
        for(var i = 0;i < cardPoolSize; i++)
        {
            var temp = Instantiate(cardPrefabs[4]);
            temp.gameObject.SetActive(false);
            blackCardPool.Add(temp);
        }

    }
    
    

    public ParticleSystem GetSplash()
    {
        for (int i = 0; i < splashPoolSize; i++)
        {
            if (!splashPool[i].gameObject.activeInHierarchy)
            {
                return splashPool[i];
            }
        }
        return null;
    }
    
    public Card GetRedCard()
    {
        for (int i = 0; i < redCardPool.Count; i++)
        {
            if (!redCardPool[i].gameObject.activeInHierarchy)
            {
                return redCardPool[i];
            }
        }
        return null;
    }
    
    public Card GetYellowCard()
    {
        for (int i = 0; i < yellowCardPool.Count; i++)
        {
            if (!yellowCardPool[i].gameObject.activeInHierarchy)
            {
                return yellowCardPool[i];
            }
        }
        return null;
    }
    
    public Card GetBlueCard()
    {
        for (int i = 0; i < blueCardPool.Count; i++)
        {
            if (!blueCardPool[i].gameObject.activeInHierarchy)
            {
                return blueCardPool[i];
            }
        }
        return null;
    }
    
    public Card GetGreenCard()
    {
        for (int i = 0; i < greenCardPool.Count; i++)
        {
            if (!greenCardPool[i].gameObject.activeInHierarchy)
            {
                return greenCardPool[i];
            }
        }
        return null;
    }
    
    public Card GetBlackCard()
    {
        for (int i = 0; i < blackCardPool.Count; i++)
        {
            if (!blackCardPool[i].gameObject.activeInHierarchy)
            {
                return blackCardPool[i];
            }
        }
        return null;
    }

    public Card GetCard(Colour colour)
    {
        Card result = colour switch
        {
            Colour.Red => GetRedCard(),
            Colour.Yellow => GetYellowCard(),
            Colour.Blue => GetBlueCard(),
            Colour.Green => GetGreenCard(),
            Colour.Black => GetBlackCard(),
            _ => null
        };

        return result;
    }

}
