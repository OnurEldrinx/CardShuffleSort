using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public List<Card> cardList; //all cards in the slot
    public SlotStatus status;

    protected BoxCollider _collider;
    protected float colliderSizeInc = 0.08f;
    protected bool isEmpty;
    protected Colour topCardColor;
    protected Stack<Card> cards; //selected cards with same color on top of the stack
    protected float offset;
    protected float cardCount;
    
    private void Start()
    {
        SetStart();
    }

    protected void SetStart()
    {
        _collider = GetComponent<BoxCollider>();
        isEmpty = true;
        cards = new Stack<Card>();
        
        if (cardList.Count <= 0) return;

        foreach (var dummy in cardList)
        {
            _collider.size += new Vector3(0, 0, colliderSizeInc);
        }

        var center = _collider.center;
        _collider.center = new Vector3(center.x, center.y, _collider.size.z / 2);
        
        isEmpty = false;
        topCardColor = cardList.Last().color;
    }
    
    private void Update()
    {
        isEmpty = cardList.Count <= 0;
        topCardColor = isEmpty ? Colour.Empty : topCardColor;
    }

    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            if (status != SlotStatus.Active) return;
            
            if (PlayerController.Instance.fromSlot == null && !isEmpty)
            {
                PlayerController.Instance.fromSlot = this;
                PlayerController.Instance.toSlot = null;

                int c = cardList.Count;
                int topColorIndex = -1;
                for (int i = 0; i < c; i++)
                {
                    if (cardList[i].color == topCardColor)
                    {

                        if (topColorIndex == -1)
                        {
                            topColorIndex = i - 1;
                        }
                        
                        Card temp = cardList[i];
                        cards.Push(temp);
                        temp.transform.DOMoveY(temp.transform.position.y + 0.1f,0.2f);
                    }
                    
                }
                
                topCardColor = cardList[topColorIndex].color;
                
            }else if (PlayerController.Instance.fromSlot != null && PlayerController.Instance.fromSlot != this)
            {
                
                PlayerController.Instance.toSlot = this;

                if (topCardColor != PlayerController.Instance.fromSlot.cards.Peek().color && topCardColor != Colour.Empty)
                {
                    
                    foreach (var card in PlayerController.Instance.fromSlot.cards)
                    {
                        float tempY = card.transform.position.y;
                        card.transform.DOMoveY(tempY - 0.1f, 0.2f);
                    }
                    
                    PlayerController.Instance.fromSlot.cards.Clear();
                    
                    UpdateSlotState();
                    PlayerController.Instance.fromSlot = null;
                    PlayerController.Instance.toSlot = null;
                    
                    
                    return;
                }
                
                foreach (var c in PlayerController.Instance.fromSlot.cards)
                {
                    PlayerController.Instance.fromSlot.cardList.Remove(c);
                }
                
                GetComponent<BoxCollider>().enabled = false;
                
                float d = PlayerController.Instance.totalDuration;
                cardCount = PlayerController.Instance.fromSlot.cards.Count;
                offset = cardList.Count == 0 ? 0 : cardList.Last().transform.position.y + 0.075f;
                //float h = PlayerController.Instance.fromSlot._cards.Peek().transform.position.y;
                
                
                for (int i = 0; i < cardCount; i++)
                {
                    Card last = PlayerController.Instance.fromSlot.cards.Pop();
                    PlayerController.Instance.fromSlot.cardList.Remove(last);
                    last.PlayAnimation(PlayerController.Instance.toSlot, d,PlayerController.Instance.height ,PlayerController.Instance.ease, offset,0).OnComplete(
                        () =>
                        {

                            if (gameObject.name.Equals("DealTableSlot"))
                            {
                                transform.parent.GetComponent<DealTable>().fillImage.fillAmount += 0.1f;
                            }
                            
                        });
                    cardList.Add(last);
                    d += 0.075f;
                    offset += 0.075f;
                    UpdateColliderSize(1);
                    PlayerController.Instance.fromSlot.UpdateColliderSize(-1);
                }
                
                
                UpdateColliderCenter();
                PlayerController.Instance.fromSlot.UpdateColliderCenter();
                
                PlayerController.Instance.fromSlot = null;
                PlayerController.Instance.toSlot = null;
                
                Invoke(nameof(UpdateSlotState),d);

                
                
                
            }
            
        }
        
        
    }

    private void UpdateSlotState()
    {
        //Animation is done
        GetComponent<BoxCollider>().enabled = true;
        topCardColor = cardList.Last().color;
    }

    private void UpdateColliderSize(float sign)
    {
        _collider.size += new Vector3(0, 0, sign * colliderSizeInc);
    }

    private void UpdateColliderCenter()
    {
        var center = _collider.center;
        _collider.center = new Vector3(center.x, center.y, _collider.size.z / 2);
    }
    

}
