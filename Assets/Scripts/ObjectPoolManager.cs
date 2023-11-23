using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{

    [Space(10)]

    [Header("VFX Pool Settings")]
    public List<ParticleSystem> splashPool;
    public ParticleSystem splashPrefab;
    public int splashPoolSize;
    

    private void Start()
    {

        splashPool = new List<ParticleSystem>();
        for(var i = 0;i < splashPoolSize; i++)
        {
            var temp = Instantiate(splashPrefab);
            temp.gameObject.SetActive(false);
            splashPool.Add(temp);
        }

    }
    
    

    public ParticleSystem GetSplash()
    {
        for (int i = 0; i < splashPoolSize; i++)
        {
            if (!splashPool[i].gameObject.activeInHierarchy)
            {
                return splashPool[i];
            }
        }
        return null;
    }
    

}
