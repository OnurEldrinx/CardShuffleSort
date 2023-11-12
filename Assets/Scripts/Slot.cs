using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public List<Card> cardList;
    public SlotStatus status;

    private BoxCollider _collider;
    private const float ColliderSizeInc = 0.08f;
    public bool _isEmpty;
    private Colour _topCardColor;
    private Stack<Card> _cards;
    
    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _isEmpty = true;
        _cards = new Stack<Card>();
        
        if (cardList.Count > 0)
        {
            foreach (var c in cardList)
            {
                _cards.Push(c);
                _collider.size += new Vector3(0, 0, ColliderSizeInc);
            }

            var center = _collider.center;
            _collider.center = new Vector3(center.x, center.y, _collider.size.z / 2);

        }

        if (_cards.Count <= 0) return;
        _isEmpty = false;
        _topCardColor = _cards.Last().color;

    }

    private void Update()
    {
        _isEmpty = _cards.Count <= 0;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            if (status != SlotStatus.Active) return;
            
            if (PlayerController.Instance.fromSlot == null && !_isEmpty && !PlayerController.Instance.fromToPair.ContainsValue(this))
            {
                
                PlayerController.Instance.fromSlot = this;
                PlayerController.Instance.toSlot = null;
                
            }else if (PlayerController.Instance.fromSlot != null && PlayerController.Instance.toSlot == null && PlayerController.Instance.fromSlot != this)
            {
                PlayerController.Instance.toSlot = this;
                
                PlayerController.Instance.fromToPair.Add(PlayerController.Instance.fromSlot,PlayerController.Instance.toSlot);
                
                var d = 0.5f;
                var count = PlayerController.Instance.fromSlot._cards.Count;

                float offset = _cards.Count == 0 ? 0 : _cards.Peek().transform.position.y + 0.075f;

                for (int i = 0; i < count; i++)
                {
                    Card last = PlayerController.Instance.fromSlot._cards.Pop();
                    last.PlayAnimation(PlayerController.Instance.toSlot, d, Ease.InOutSine, offset);
                    _cards.Push(last);
                    d += 0.05f;
                    offset += 0.075f;

                    UpdateColliderSize(1);
                    PlayerController.Instance.fromSlot.UpdateColliderSize(-1);
                    
                }
                
                
                UpdateColliderCenter();
                PlayerController.Instance.fromSlot.UpdateColliderCenter();
                
                Invoke(nameof(UpdatePairDictionary),d);

                
                
                
            }
            
        }
        
        
    }

    private void UpdatePairDictionary()
    {

        PlayerController.Instance.fromToPair.Remove(PlayerController.Instance.fromSlot);
        PlayerController.Instance.fromSlot = null;
        PlayerController.Instance.toSlot = null;
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
