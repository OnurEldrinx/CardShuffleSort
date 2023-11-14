using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public List<Card> cardList; //all cards in the slot
    public SlotStatus status;

    private BoxCollider _collider;
    private const float ColliderSizeInc = 0.08f;
    public bool _isEmpty;
    public Colour _topCardColor;
    private Stack<Card> _cards; //selected cards with same color on top of the stack
    private float offset;
    private float cardCount;
    
    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _isEmpty = true;
        _cards = new Stack<Card>();
        
        if (cardList.Count > 0)
        {
            foreach (var c in cardList)
            {
                //_cards.Push(c);
                _collider.size += new Vector3(0, 0, ColliderSizeInc);
            }

            var center = _collider.center;
            _collider.center = new Vector3(center.x, center.y, _collider.size.z / 2);

        }

        if (cardList.Count <= 0) return;
        _isEmpty = false;
        _topCardColor = cardList.Last().color;
    }

    private void Update()
    {
        _isEmpty = cardList.Count <= 0;
        _topCardColor = _isEmpty ? Colour.Empty : _topCardColor;
    }

    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            if (status != SlotStatus.Active) return;
            
            if (PlayerController.Instance.fromSlot == null && !_isEmpty)
            {
                // && !PlayerController.Instance.fromToPair.ContainsValue(this)
                PlayerController.Instance.fromSlot = this;
                PlayerController.Instance.toSlot = null;

                int c = cardList.Count;
                int topColorIndex = -1;
                for (int i = 0; i < c; i++)
                {
                    if (cardList[i].color == _topCardColor)
                    {

                        if (topColorIndex == -1)
                        {
                            topColorIndex = i - 1;
                        }
                        
                        Card temp = cardList[i];
                        _cards.Push(temp);
                        temp.transform.DOMoveY(temp.transform.position.y + 0.1f,0.2f);
                    }
                    
                }
                
                _topCardColor = cardList[topColorIndex].color;
                
                cardList.RemoveRange(topColorIndex+1,c-topColorIndex-1);

            }else if (PlayerController.Instance.fromSlot != null && PlayerController.Instance.fromSlot != this)
            {
                
                // && PlayerController.Instance.toSlot == null
                PlayerController.Instance.toSlot = this;

                if (_topCardColor != PlayerController.Instance.fromSlot._cards.Peek().color && _topCardColor != Colour.Empty)
                {
                    return;
                }
                
                //PlayerController.Instance.fromToPair.Add(PlayerController.Instance.fromSlot,PlayerController.Instance.toSlot);

                GetComponent<BoxCollider>().enabled = false;
                
                
                float d = PlayerController.Instance.totalDuration;
                cardCount = PlayerController.Instance.fromSlot._cards.Count;
                offset = cardList.Count == 0 ? 0 : cardList.Last().transform.position.y + 0.075f;
                float total = PlayerController.Instance.totalDuration;
                //float h = PlayerController.Instance.fromSlot._cards.Peek().transform.position.y;

                //PlayerController.Instance.fromSlot._topCardColor = PlayerController.Instance.fromSlot._cards.Peek().color;
                //_topCardColor = PlayerController.Instance.fromSlot._cards.Peek().color;
                
                
                for (int i = 0; i < cardCount; i++)
                {
                    Card last = PlayerController.Instance.fromSlot._cards.Pop();
                    PlayerController.Instance.fromSlot.cardList.Remove(last);
                    last.PlayAnimation(PlayerController.Instance.toSlot, d,PlayerController.Instance.height ,PlayerController.Instance.ease, offset,0);
                    //_cards.Push(last);
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
        _topCardColor = cardList.Last().color;
        //PlayerController.Instance.fromToPair.Remove(PlayerController.Instance.fromSlot);

    }

    private void UpdateColliderSize(float sign)
    {
        _collider.size += new Vector3(0, 0, sign * ColliderSizeInc);
    }

    private void UpdateColliderCenter()
    {
        var center = _collider.center;
        _collider.center = new Vector3(center.x, center.y, _collider.size.z / 2);
    }
    

}
