using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlcPooling : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
