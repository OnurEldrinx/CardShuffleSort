using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Stack<Card> _cards;
    public List<Card> cardList;
    public SlotStatus status;
    private bool _isEmpty;
    private Colour _topCardColor;
    public bool isSelectedFirst;
    public bool isTarget;
    public Slot targetSlot;
    
    private void Start()
    {

        _isEmpty = true;
        
        _cards = new Stack<Card>();

        /*if (transform.childCount > 0)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).TryGetComponent<Card>(out var current)) _cards.AddLast(current);
            }
        }*/
        
        if (cardList.Count > 0)
        {
            for (var i = 0; i < cardList.Count; i++)
            {
                _cards.Push(cardList[i]);
            }
        }

        if (_cards.Count > 0)
        {
            _isEmpty = false;
            _topCardColor = _cards.Last().color;
        }
        
        Debug.Log(_cards.Count);

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (status != SlotStatus.Active)
            {
                return;
            }
            
            if (PlayerController.fromSlot == null)
            {
                PlayerController.fromSlot = this;
                PlayerController.toSlot = null;
                isSelectedFirst = true;
                isTarget = false;
                //_cards.Last.Value.PlayAnimation(targetSlot,2.75f,Ease.Unset);
            }else if (PlayerController.fromSlot != null && PlayerController.toSlot == null)
            {
                PlayerController.toSlot = this;
                isSelectedFirst = false;
                isTarget = true;


                var d = 0.5f;
                var count = PlayerController.fromSlot._cards.Count;
                

                float offset = _cards.Count == 0 ? 0 : _cards.Peek().transform.position.y + 0.075f;
                
                for (int i = 0; i < count; i++)
                {
                    Card last = PlayerController.fromSlot._cards.Pop();
                    last.PlayAnimation(PlayerController.toSlot, d, Ease.InOutFlash,offset);
                    _cards.Push(last);
                    d += 0.05f;
                    offset += 0.075f;
                }
                
                /*last.PlayAnimation(PlayerController.toSlot,0.75f,Ease.Unset);
                _cards.AddLast(last);
                PlayerController.fromSlot._cards.RemoveLast();*/
                
                PlayerController.fromSlot.isSelectedFirst = false;
                isTarget = false;
                PlayerController.fromSlot = null;
                PlayerController.toSlot = null;
                
            }
            
            
        }
    }
    

}
