using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationTest : MonoBehaviour
{

    
    public Transform[] followers;

    public float duration;

    public Ease e;

    private List<Transform> _allCards;

    public Transform targetSlot;
    
    // Start is called before the first frame update
    void Start()
    {

        _allCards = new List<Transform>();
      
    }

    public void Test(Transform t)
    {
        
        //transform.DOPath(_pathPoints, duration, PathType.CatmullRom).SetEase(e);
        transform.DOJump(targetSlot.transform.position,2,1,duration).SetEase(e);
        transform.DORotate(new Vector3(270,0,0),duration);

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            
            
            Test(transform);
            Debug.Log(targetSlot.transform.position - transform.position);

        }
        
    }
}
