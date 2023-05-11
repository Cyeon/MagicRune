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
        // Set Dictionary 
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
        ActiveAllEffectObject(false);
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

    /// <summary>
    /// 공명 이펙트 세팅해주는 함수 
    /// </summary>
    /// <param name="type">공명 속성</param>
    public void ResonanceEffect(AttributeType type)
    {
        if (type == AttributeType.None || type == AttributeType.NonAttribute) { return; }

        ActiveAllEffectObject(true);

        List<Gradient> gradients = GetGradients(type);

        for (int i = 0; i < _effectNameArr.Length; i++)
        {
            for (int j = 0; j < MAX_PARTICLE_COUNT; j++)
            {
                ParticleSystem.ColorOverLifetimeModule col = _effectParticleDictionary[_effectNameArr[i]][j].colorOverLifetime;
                col.color = gradients[i];
            }
        }
    }

    /// <summary>
    /// inspecter에서 설정해둔 그라디언트 가져오는 함수 
    /// </summary>
    /// <param name="type">찾을 속성</param>
    /// <returns>속성별 그라디언트를 반환</returns>
    private List<Gradient> GetGradients(AttributeType type)
    {
        for (int i = 0; i < _gradiantList.Count; i++)
        {
            if (_gradiantList[i].type == type)
            {
                return _gradiantList[i].gradients;
            }
        }

        return null;
    }

    public void ActiveAllEffectObject(bool isActive)
    {
        for (int i = 0; i < _particleObjectArr.Length; i++)
        {
            if (_particleObjectArr[i] != null)
                _particleObjectArr[i].SetActive(isActive);
        }
    }
}