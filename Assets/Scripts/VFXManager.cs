using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{

    public Transform coinMesh;
    
    
    private Camera _mainCamera;

    public readonly Dictionary<Colour, Color> colorMap = new();
    

    private void Start()
    {
        _mainCamera = Camera.main;
        
        colorMap.Add(Colour.Red,Color.red);
        colorMap.Add(Colour.Yellow,Color.yellow);
        colorMap.Add(Colour.Blue,Color.blue);
        colorMap.Add(Colour.Green,Color.green);
        colorMap.Add(Colour.Black,Color.black);
        
    }

    public void PlayParticleAtPosition(ParticleSystem p,Vector3 position)
    {

        p.transform.position = position;
        p.Play();
        
    }

    public void PlayCoinCollectAnimation(Vector3 position)
    {

        coinMesh.position = position;
        coinMesh.gameObject.SetActive(true);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(UIManager.Instance.coinCounterText.rectTransform,UIManager.Instance.coinCounterText.transform.position, _mainCamera, out var worldSpacePoint);

        Vector3 coinRotation = coinMesh.transform.localRotation.eulerAngles;
        coinMesh.DOLocalRotate(new Vector3(coinRotation.x, coinRotation.y, 720), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.InQuad);
        

        coinMesh.DOMove(worldSpacePoint,0.5f).SetDelay(0.25f).OnComplete(() =>
        {
                
            coinMesh.localRotation = Quaternion.Euler(coinRotation);
            coinMesh.gameObject.SetActive(false);
                
        });
        
    }
    
    
}
