using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DealButton : MonoBehaviour
{
    public Slot spawnSlot;

    private const int SpawnSize = 4;

    public void HandleTap()
    {

        if (PlayerController.Instance.fromSlot is not null)
        {
            foreach (var card in PlayerController.Instance.fromSlot.GetSelectedCards())
            {
                float tempY = card.transform.position.y;
                card.transform.DOMoveY(tempY - 0.1f, 0.2f);
            }
                    
            PlayerController.Instance.fromSlot.GetSelectedCards().Clear();
            PlayerController.Instance.fromSlot.UpdateSlotState();
            //PlayerController.Instance.toSlot.UpdateSlotState();
            PlayerController.Instance.fromSlot = null;
            PlayerController.Instance.toSlot = null;
        }
        
        transform.DOScaleZ(0.5f,0.2f).OnComplete(() => transform.DOScaleZ(1.25f,0.2f));

        float timer = 0.25f;
        
        foreach (var slot in GameManager.Instance.GetActiveSlots())
        {
            StartCoroutine(SendCards(slot,timer));
            timer += 0.25f;
        }
        
    }

    IEnumerator SendCards(Slot slot,float timer)
    {
        
        yield return new WaitForSeconds(timer);
        SendCardsTo(slot);

    }

    IEnumerator UpdateSlotState(Slot target,float time)
    {

        yield return new WaitForSeconds(time);
        target.UpdateSlotState();
    }

    private void SendCardsTo(Slot target)
    {
        float d = PlayerController.Instance.totalDuration;
        float offset = target.cardList.Count == 0 ? 0 : target.cardList.Last().transform.position.y + 0.075f;
        
        target.SetColliderAvailability(false);
        
        PlayerController.Instance.animationOnPlay = true;

        Colour targetColor = target.GetTopColor();

        List<Colour> colourOptions = new List<Colour>(GameManager.Instance.cardColors);
        colourOptions.Remove(targetColor);

        int colorIndex = Random.Range(0, colourOptions.Count);

        Colour spawnColour = colourOptions[colorIndex];
        
        for (int i = 0; i < SpawnSize; i++)
        {
            Card temp = ObjectPoolManager.Instance.GetCard(spawnColour);
            temp.transform.position = spawnSlot.transform.position;
            temp.gameObject.SetActive(true);

            temp.PlayAnimation(target, d, PlayerController.Instance.height*2, PlayerController.Instance.ease, offset, 0);
            target.cardList.Add(temp);
            d += 0.075f;
            offset += 0.075f;
            target.UpdateColliderSize(1);
            
        }
        
        target.UpdateColliderCenter();

        StartCoroutine(UpdateSlotState(target, d));


    }


}
