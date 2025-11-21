using UnityEngine;

public class ReturnParticlesToPool : MonoBehaviour
{

    private void OnParticleSystemStopped()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }


}
