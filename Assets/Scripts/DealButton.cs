using DG.Tweening;
using UnityEngine;

public class DealButton : MonoBehaviour
{
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.DOScaleZ(0.5f,0.2f).OnComplete(() => transform.DOScaleZ(1f,0.2f));
        }
    }
}
