using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResonanceGradient
{
    public AttributeType type;
    public List<Gradient> gradients;
}

public class Resonance : MonoBehaviour
{
    private readonly string[] _effectNameArr = new string[] { "MagicAura", "MagicSparkles", "Waves" };

    [SerializeField]
    private List<ResonanceGradient> _gradiantList = new List<ResonanceGradient>();

    private Dictionary<string, List<ParticleSystem>> _effectParticleDictionary = new Dictionary<string, List<ParticleSystem>>();

    private GameObject[] _particleObjectArr = new GameObject[3];

    private const int MAX_PARTICLE_COUNT = 3;

    private void Start()
    {

        for (int i = 0; i < MAX_PARTICLE_COUNT; i++)
        {
            _effectParticleDictionary.Add(_effectNameArr[i], new List<ParticleSystem>());
        }

        // 공명 파티클 생성
        for (int i = 0; i < MAX_PARTICLE_COUNT; i++)
        {
            GameObject gameObject = Managers.Resource.Instantiate("Effects/MagicAura", transform);
            gameObject.transform.localPosition = new Vector3(0f, 4f - (1.2f * i), 0f);
            _particleObjectArr[i] = gameObject;

            _effectParticleDictionary[_effectNameArr[0]].Add(gameObject.GetComponent<ParticleSystem>());

            for (int j = 0; j < gameObject.transform.childCount; j++)
            {
                Transform child = gameObject.transform.GetChild(j);
                _effectParticleDictionary[_effectNameArr[j + 1]].Add(child.GetComponent<ParticleSystem>());
            }
            gameObject.SetActive(false);
        }
    }

    public void Invocation(AttributeType resonanceType)
    {
        Invoke(resonanceType + "Resonance", 0);
        ResonanceEffect(AttributeType.None, false);
    }

    public void FireResonance()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, 5);
    }

    public void IceResonance()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, 3);
    }

    public void GroundResonance()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Impact, 5);
    }

    public void ElectricResonance()
    {
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.Recharging, 5);
    }

    public void ResonanceEffect(AttributeType type, bool isActive = true)
    {
        ActiveAllEffectObject(isActive);

        if (!isActive)
            return;

        List<Gradient> gradients = new List<Gradient>();

        for (int i = 0; i < _gradiantList.Count; i++)
        {
            if (_gradiantList[i].type == type)
            {
                gradients = _gradiantList[i].gradients;
                break;
            }
        }

        for (int i = 0; i < MAX_PARTICLE_COUNT; i++)
        {
            for (int j = 0; j < _effectNameArr.Length; j++)
            {
                ParticleSystem.ColorOverLifetimeModule color = _effectParticleDictionary[_effectNameArr[i]][j].colorOverLifetime;
                color.color.gradient.SetKeys(gradients[i].colorKeys, gradients[i].alphaKeys);
            }
        }
    }

    private void ActiveAllEffectObject(bool isActive)
    {
        for (int i = 0; i < _particleObjectArr.Length; i++)
        {
            _particleObjectArr[i].SetActive(isActive);
        }

    }
}