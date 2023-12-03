using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public List<Card> cardList; //all cards in the slot
    public SlotStatus status;
    [SerializeField] private bool isDealTable;

    private BoxCollider _collider;
    private const float ColliderSizeInc = 0.08f;
    private bool _isEmpty;
    private Colour _topCardColor;
    private Stack<Card> _selectedCards; //selected cards with same color on top of the stack
    private float _offset;
    private DealTable _dealTable;
    private static int _cardCounter; //for deal table animations

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _selectedCards = new Stack<Card>();
        _isEmpty = true;

        if (isDealTable)
        {
            _dealTable = transform.parent.GetComponent<DealTable>();
        }
        
        if (cardList.Count == 0) return;

        foreach (var dummy in cardList)
        {
            UpdateColliderSize(1);
        }
        
        UpdateColliderCenter();
        _isEmpty = false;
        _topCardColor = cardList.Last().color;
    }

    private void Update()
    {
        _isEmpty = cardList.Count == 0;
        _topCardColor = _isEmpty ? Colour.Empty : _topCardColor;
    }

    public void HandleTap()
    {
        if (status != SlotStatus.Active) return;
            
            if (PlayerController.Instance.fromSlot is null && !_isEmpty)
            {
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
                        _selectedCards.Push(temp);
                        temp.transform.DOMoveY(temp.transform.position.y + 0.1f,0.2f);
                    }
                    
                }

                _topCardColor = topColorIndex != -1 ? cardList[topColorIndex].color : Colour.Empty;
                
            }else if (PlayerController.Instance.fromSlot is not null && PlayerController.Instance.fromSlot != this)
            {
                
                PlayerController.Instance.toSlot = this;

                // Checking if the top colors are matched. If not matched, cancel the move
                if (_topCardColor != PlayerController.Instance.fromSlot._selectedCards.Peek().color && _topCardColor != Colour.Empty)
                {
                    
                    foreach (var card in PlayerController.Instance.fromSlot._selectedCards)
                    {
                        float tempY = card.transform.position.y;
                        card.transform.DOMoveY(tempY - 0.1f, 0.2f);
                    }
                    
                    PlayerController.Instance.fromSlot._selectedCards.Clear();
                    PlayerController.Instance.fromSlot.UpdateSlotState();
                    UpdateSlotState();
                    PlayerController.Instance.fromSlot = null;
                    PlayerController.Instance.toSlot = null;
                    
                    return;
                }
                
                foreach (var c in PlayerController.Instance.fromSlot._selectedCards)
                {
                    PlayerController.Instance.fromSlot.cardList.Remove(c);
                }
                
                _collider.enabled = false;
                
                float d = PlayerController.Instance.totalDuration;
                float cardCount = PlayerController.Instance.fromSlot._selectedCards.Count;
                _offset = cardList.Count == 0 ? 0 : cardList.Last().transform.position.y + 0.075f;
                
                // Animation on play
                PlayerController.Instance.animationOnPlay = true;

                // Set Fill Color
                if (isDealTable)
                {
                    switch (PlayerController.Instance.fromSlot._selectedCards.Peek().color)
                    {
                        case Colour.Red:
                            _dealTable.fillImage.color = Color.red;
                            break;
                        case Colour.Yellow:
                            _dealTable.fillImage.color = Color.yellow;
                            break;
                        case Colour.Green:
                            _dealTable.fillImage.color = Color.green;
                            break;
                        case Colour.Black:
                            _dealTable.fillImage.color = Color.black;
                            break;
                        case Colour.Blue:
                            _dealTable.fillImage.color = Color.blue;
                            break;
                    }
                }
                
                for (int i = 0; i < cardCount; i++)
                {
                    Card last = PlayerController.Instance.fromSlot._selectedCards.Pop();
                    PlayerController.Instance.fromSlot.cardList.Remove(last);
                    last.PlayAnimation(PlayerController.Instance.toSlot, d,PlayerController.Instance.height ,PlayerController.Instance.ease, _offset,0).OnComplete(
                        () =>
                        {

                            if (isDealTable)
                            {
                                _dealTable.fillImage.fillAmount += 0.1f;
                            }
                            
                        });
                    cardList.Add(last);
                    d += 0.075f;
                    _offset += 0.075f;
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
    
    /*private void OnMouseOver()
    {
        
        Debug.Log(gameObject.name);
        
        if (Input.GetMouseButtonDown(0))
        {
            
            if (status != SlotStatus.Active) return;
            
            if (PlayerController.Instance.fromSlot == null && !_isEmpty)
            {
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
                
            }else if (PlayerController.Instance.fromSlot != null && PlayerController.Instance.fromSlot != this)
            {
                
                PlayerController.Instance.toSlot = this;

                if (_topCardColor != PlayerController.Instance.fromSlot._cards.Peek().color && _topCardColor != Colour.Empty)
                {
                    
                    foreach (var card in PlayerController.Instance.fromSlot._cards)
                    {
                        float tempY = card.transform.position.y;
                        card.transform.DOMoveY(tempY - 0.1f, 0.2f);
                    }
                    
                    PlayerController.Instance.fromSlot._cards.Clear();
                    PlayerController.Instance.fromSlot.UpdateSlotState();
                    UpdateSlotState();
                    PlayerController.Instance.fromSlot = null;
                    PlayerController.Instance.toSlot = null;
                    
                    
                    return;
                }
                
                foreach (var c in PlayerController.Instance.fromSlot._cards)
                {
                    PlayerController.Instance.fromSlot.cardList.Remove(c);
                }
                
                GetComponent<BoxCollider>().enabled = false;
                
                float d = PlayerController.Instance.totalDuration;
                _cardCount = PlayerController.Instance.fromSlot._cards.Count;
                _offset = cardList.Count == 0 ? 0 : cardList.Last().transform.position.y + 0.075f;
                //float h = PlayerController.Instance.fromSlot._cards.Peek().transform.position.y;
                
                // Animation on play
                PlayerController.Instance.animationOnPlay = true;
                for (int i = 0; i < _cardCount; i++)
                {
                    Card last = PlayerController.Instance.fromSlot._cards.Pop();
                    PlayerController.Instance.fromSlot.cardList.Remove(last);
                    last.PlayAnimation(PlayerController.Instance.toSlot, d,PlayerController.Instance.height ,PlayerController.Instance.ease, _offset,0).OnComplete(
                        () =>
                        {

                            if (gameObject.name.Equals("DealTableSlot"))
                            {
                                transform.parent.GetComponent<DealTable>().fillImage.fillAmount += 0.1f;
                                //VFXManager.Instance.PlayParticleAtPosition(last.transform.position);
                            }
                            
                        });
                    cardList.Add(last);
                    d += 0.075f;
                    _offset += 0.075f;
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
        
        
    }*/

    
    
    private void UpdateSlotState()
    {
        //Animation is done
        PlayerController.Instance.animationOnPlay = false;
        _collider.enabled = true;
        _topCardColor = cardList.Last().color;


        if (!isDealTable) return;

        int c = cardList.Count;
        _cardCounter = c;
        
        if (c < 10) return;
        
        _collider.enabled = false;
        PlayerController.Instance.totalCoin += c * 30;

        float timer = 0;
        for (int i = 0; i < c; i++)
        {
            Invoke(nameof(SplashAndDisableCard),timer);
            timer += PlayerController.Instance.cardDisableTime;
        }
        
        
        Invoke(nameof(LevelUp),timer+PlayerController.Instance.cardDisableTime);
        
    }

    private void SplashAndDisableCard()
    {
        Card last = cardList.Last();
        ParticleSystem splash = ObjectPoolManager.Instance.GetSplash();
        var mainModule = splash.main;
        
        switch (last.color)
        {
            case Colour.Red:
                mainModule.startColor = Color.red;
                break;
            case Colour.Yellow:
                mainModule.startColor = Color.yellow;
                break;
            case Colour.Green:
                mainModule.startColor = Color.green;
                break;
            case Colour.Black:
                mainModule.startColor = Color.black;
                break;
            case Colour.Blue:
                mainModule.startColor = Color.blue;
                break;
        }
        
        splash.gameObject.SetActive(true);
        if (cardList.Remove(last))
        {
            last.gameObject.SetActive(false); //return to pool
            VFXManager.Instance.PlayParticleAtPosition(splash,last.transform.position);
            transform.parent.GetComponent<DealTable>().fillImage.fillAmount -= 0.1f;
            UpdateColliderSize(-1);
            UpdateColliderCenter();
        }
    }

    private void LevelUp()
    {
        
        VFXManager.Instance.PlayCoinCollectAnimation(transform.position + new Vector3(0,ColliderSizeInc*2,0));

        
        _collider.enabled = true;
        UIManager.Instance.coinCounterText.text = PlayerController.Instance.totalCoin.ToString();
        float f = UIManager.Instance.levelProgressBar.fillAmount + (_cardCounter * 0.1f);
        float extra = f > 1 ? f-1 : 0;

        float d = 0.5f;
        
        DOTween.To(()=> UIManager.Instance.levelProgressBar.fillAmount, x=> UIManager.Instance.levelProgressBar.fillAmount = x, 1, d).
            OnComplete(
            () =>
            {
                
                if (extra < 0) return;
                UIManager.Instance.levelProgressBar.fillAmount = 0;
                DOTween.To(() => UIManager.Instance.levelProgressBar.fillAmount,
                    x => UIManager.Instance.levelProgressBar.fillAmount = x, extra,
                    d);

                PlayerController.Instance.levelNo += 1;
                UIManager.Instance.levelNoText.text = PlayerController.Instance.levelNo.ToString();
                
            }
            );
        
        
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
