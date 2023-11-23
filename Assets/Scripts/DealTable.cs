using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DealTable : MonoBehaviour
{
    public Slot dealSlot;
    public Transform fillFrame;
    public Image fillImage;
    public float fillFramePositionOffset;

    void Start()
    {
        
        fillFrame.position = transform.position - new Vector3(0, 0, fillFramePositionOffset);
        fillImage.transform.position = fillFrame.position;
        
    }

    private void Update()
    {

        int cardCount = dealSlot.cardList.Count;
        
        if (cardCount > 0)
        {
            fillImage.sprite = UIManager.Instance.fillerSprites[dealSlot.cardList.Last().color];
            
        }
        else
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount,0,5f * Time.deltaTime);
            if (fillImage.fillAmount < 0.01)
            {
                fillImage.fillAmount = 0;
            }
        }

        

    }
}
