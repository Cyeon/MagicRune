using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathParticle : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
