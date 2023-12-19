using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DealButton : MonoBehaviour
{

    public Transform spawnPoint;
    private const int SpawnSize = 4;

    [SerializeField] private float delayBetweenCards = 0.075f;
    [SerializeField] private float delayBetweenSlots = 0.25f;
    
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

        float timer = 0.25f;// start delay, bigger than delay
        
        foreach (var slot in GameManager.Instance.GetActiveSlots())
        {
            slot.SetIsDealButtonTarget(true);
            StartCoroutine(SendCards(slot,timer));
            timer += delayBetweenSlots;
        }
        
        Invoke(nameof(SetDealButtonActive),timer + PlayerController.Instance.totalDuration);

    }

    private void SetDealButtonActive()
    {

        PlayerController.Instance.dealButtonActive = false;

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
        float offset = target.cardList.Count == 0 ? 0 : target.cardList.Last().transform.position.y + PlayerController.Instance.cardPositionOffsetY;
        
        target.SetColliderAvailability(false);
        
        PlayerController.Instance.animationOnPlay = true;

        Colour targetColor = target.GetTopColor();

        List<Colour> colourOptions = new List<Colour>(GameManager.Instance.cardColors);
        colourOptions.Remove(targetColor);

        int colorIndex = Random.Range(0, colourOptions.Count);

        Colour spawnColour = colourOptions[colorIndex];

        float delay = 0;
        
        for (int i = 0; i < SpawnSize; i++)
        {
            Card temp = ObjectPoolManager.Instance.GetCard(spawnColour);
            temp.transform.position = spawnPoint.position;
            temp.gameObject.SetActive(true);

            temp.PlayAnimation(target, d, PlayerController.Instance.height, PlayerController.Instance.ease, offset, delay);
            target.cardList.Add(temp);
            delay += delayBetweenCards;
            offset += PlayerController.Instance.cardPositionOffsetY;
            target.UpdateColliderSize(1);
            
        }
        
        target.UpdateColliderCenter();

        StartCoroutine(UpdateSlotState(target, delay+d));


    }


}
