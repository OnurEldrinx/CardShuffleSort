using DG.Tweening;
using UnityEngine;

public class DealButton : MonoBehaviour
{
    public Slot spawnSlot;
    
    private int _spawnSize = 5;
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (PlayerController.Instance.animationOnPlay) return;

            transform.DOScaleZ(0.5f,0.2f).OnComplete(() => transform.DOScaleZ(1.25f,0.2f));
            
            
            
            
        }
    }
}
