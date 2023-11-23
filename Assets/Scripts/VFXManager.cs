using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    public void PlayParticleAtPosition(ParticleSystem p,Vector3 position)
    {

        p.transform.position = position;
        p.Play();
        
    }
    
}
