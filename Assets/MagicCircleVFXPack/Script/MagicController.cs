using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_URP
using UnityEngine.Rendering.Universal;
#endif

[Serializable]
[ExecuteInEditMode]
public class MagicController : MonoBehaviour
{
    [System.Serializable]
    public class MagicSet
    {
        [ColorUsage(true, true)] public Color _color = Color.white;
        public float _Size = 1;
        public float _originalRotate = 0;
        public bool _needAutoRotate = false;
        public float _RotateSpeed = 0;
        [Range(0.01f, 0.99f)]
        public float _Light = 0.01f;
        public float _LightGain = 1.0f;
        [ColorUsage(true, true)] public Color _DissolveColor = Color.white;
        public float _DissolveIn = 0;
        public float _DissolveOut = 1;
        private float _Rotate = 0;


        public float Rotate
        {
            get => _Rotate; set => _Rotate = value;
        }
        public void Reset()
        {
            Rotate = 0;
        }
    }

    [SerializeField] private Texture _magicTexture;
    [SerializeField] private Vector4 _magicTextureSt = new Vector4(1, 1, 0, 0);
    [SerializeField] private Texture _DissolveTex;
    [SerializeField] private Vector4 _DissolveTexSt = new Vector4(1, 1, 0, 0);
#if UNITY_URP
    [SerializeField] private DecalProjector _decalProjector;
#endif
    [SerializeField] private Projector _projector;

    [Range(0.0f, 1.0f)] public float _GroundResidue = 0.0f;
    [Header("R Setting")]
    [ColorUsage(true, true)] public Color _Rcolor = Color.white;
    public float _RSize = 1;
    public float _RRotate = 0;
    public bool _RNeedAutoRotate = false;
    public float _RRotateSpeed = 0;
    [Range(0.0f, 1.0f)] public float _RLight = 0.2f;
    [Range(1.0f, 20.0f)] public float _RLightGain = 1.0f;
    [ColorUsage(true, true)] public Color _RDissolveColor = Color.white;
    [Range(0.0f, 1.0f)] public float _RDissolveIn = 0;
    [Range(0.0f, 1.0f)] public float _RDissolveOut = 1;

    [Header("G Setting")]
    [ColorUsage(true, true)] public Color _Gcolor = Color.white;
    public float _GSize = 1;
    public float _GRotate = 0;
    public bool _GNeedAutoRotate = false;
    public float _GRotateSpeed = 0;
    [Range(0.0f, 1.0f)] public float _GLight = 0.2f;
    [Range(1.0f, 20.0f)] public float _GLightGain = 1.0f;
    [ColorUsage(true, true)] public Color _GDissolveColor = Color.white;
    [Range(0.0f, 1.0f)] public float _GDissolveIn = 0;
    [Range(0.0f, 1.0f)] public float _GDissolveOut = 1;

    [Header("B Setting")]
    [ColorUsage(true, true)] public Color _Bcolor = Color.white;
    public float _BSize = 1;
    public float _BRotate = 0;
    public bool _BNeedAutoRotate = false;
    public float _BRotateSpeed = 0;
    [Range(0.0f, 1.0f)] public float _BLight = 0.2f;
    [Range(1.0f, 20.0f)] public float _BLightGain = 1.0f;
    [ColorUsage(true, true)] public Color _BDissolveColor = Color.white;
    [Range(0.0f, 1.0f)] public float _BDissolveIn = 0;
    [Range(0.0f, 1.0f)] public float _BDissolveOut = 1;


    private float _time;
    private MagicSet magicSetR = new MagicSet();
    private MagicSet magicSetG = new MagicSet();
    private MagicSet magicSetB = new MagicSet();
    Material _material;
    public Material _originalMaterial;

    static class BaseShaderIDs
    {
        public static readonly int DissolveTex_ST = Shader.PropertyToID("_DissolveTex_ST");
        public static readonly int MainTex_ST = Shader.PropertyToID("_MainTex_ST");
        public static readonly int GroundResidue = Shader.PropertyToID("_GroundResidue");
    }

    static class RMagicShaderIDs
    {
        public static readonly int Size = Shader.PropertyToID("_RSize");
        public static readonly int Rotate = Shader.PropertyToID("_RRotate");
        public static readonly int DissolveIn = Shader.PropertyToID("_RDissolveIn");
        public static readonly int DissolveOut = Shader.PropertyToID("_RDissolveOut");
        public static readonly int Color = Shader.PropertyToID("_RColor");
        public static readonly int DissolveColor = Shader.PropertyToID("_RDissolveColor");
        public static readonly int Light = Shader.PropertyToID("_RLight");
        public static readonly int LightGain = Shader.PropertyToID("_RLightGain");
    }

    static class GMagicShaderIDs
    {
        public static readonly int Size = Shader.PropertyToID("_GSize");
        public static readonly int Rotate = Shader.PropertyToID("_GRotate");
        public static readonly int DissolveIn = Shader.PropertyToID("_GDissolveIn");
        public static readonly int DissolveOut = Shader.PropertyToID("_GDissolveOut");
        public static readonly int Color = Shader.PropertyToID("_GColor");
        public static readonly int DissolveColor = Shader.PropertyToID("_GDissolveColor");
        public static readonly int Light = Shader.PropertyToID("_GLight");
        public static readonly int LightGain = Shader.PropertyToID("_GLightGain");
    }

    static class BMagicShaderIDs
    {
        public static readonly int Size = Shader.PropertyToID("_BSize");
        public static readonly int Rotate = Shader.PropertyToID("_BRotate");
        public static readonly int DissolveIn = Shader.PropertyToID("_BDissolveIn");
        public static readonly int DissolveOut = Shader.PropertyToID("_BDissolveOut");
        public static readonly int Color = Shader.PropertyToID("_BColor");
        public static readonly int DissolveColor = Shader.PropertyToID("_BDissolveColor");
        public static readonly int Light = Shader.PropertyToID("_BLight");
        public static readonly int LightGain = Shader.PropertyToID("_BLightGain");
    }

#if UNITY_URP
    public DecalProjector decalProjector
    {
        get
        {
            return _decalProjector;
        }

        set => _decalProjector = value;
    }
#endif

    public Projector projector
    {
        get
        {
            return _projector;
        }

        set => _projector = value;
    }



    public MagicSet MagicSetR
    {
        get
        {
            this.magicSetR._color = _Rcolor;
            this.magicSetR._Size = _RSize;
            this.magicSetR._originalRotate = _RRotate;
            this.magicSetR._needAutoRotate = _RNeedAutoRotate;
            this.magicSetR._RotateSpeed = _RRotateSpeed;
            this.magicSetR._Light = _RLight;
            this.magicSetR._LightGain = _RLightGain;
            this.magicSetR._DissolveColor = _RDissolveColor;
            this.magicSetR._DissolveIn = _RDissolveIn;
            this.magicSetR._DissolveOut = _RDissolveOut;
            return magicSetR;
        }
        set => magicSetR = value;
    }

    public MagicSet MagicSetG
    {
        get
        {
            this.magicSetG._color = _Gcolor;
            this.magicSetG._Size = _GSize;
            this.magicSetG._originalRotate = _GRotate;
            this.magicSetG._needAutoRotate = _GNeedAutoRotate;
            this.magicSetG._RotateSpeed = _GRotateSpeed;
            this.magicSetG._Light = _GLight;
            this.magicSetG._LightGain = _GLightGain;
            this.magicSetG._DissolveColor = _GDissolveColor;
            this.magicSetG._DissolveIn = _GDissolveIn;
            this.magicSetG._DissolveOut = _GDissolveOut;
            return magicSetG;
        }
        set => magicSetG = value;
    }
    public MagicSet MagicSetB
    {
        get
        {
            this.magicSetB._color = _Bcolor;
            this.magicSetB._Size = _BSize;
            this.magicSetB._originalRotate = _BRotate;
            this.magicSetB._needAutoRotate = _BNeedAutoRotate;
            this.magicSetB._RotateSpeed = _BRotateSpeed;
            this.magicSetB._Light = _BLight;
            this.magicSetB._LightGain = _BLightGain;
            this.magicSetB._DissolveColor = _BDissolveColor;
            this.magicSetB._DissolveIn = _BDissolveIn;
            this.magicSetB._DissolveOut = _BDissolveOut;
            return magicSetB;
        }
        set => magicSetB = value;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (this.projector != null) this.projector.material = null;
#if UNITY_URP
        if (this.decalProjector != null) this.decalProjector.material = null;
#endif
        this._material = null;
        MagicSetR.Reset();
        MagicSetG.Reset();
        MagicSetB.Reset();
        this.SetMaterial();
    }

    private void OnEnable()
    {

        if (this.projector != null) this.projector.material = null;
#if UNITY_URP
        if (this.decalProjector != null) this.decalProjector.material = null;
#endif
        this._material = null;
        MagicSetR.Reset();
        MagicSetG.Reset();
        MagicSetB.Reset();
        this.SetMaterial();
    }

    private void OnDisable()
    {
        //this._tempMaterial = null;
        //Projector.material =  this._originalMaterial;
    }

    private void SetMaterial()
    {
        if (this.projector != null)
        {
            if (this.projector.material == null)
            {
                if (this._material == null)
                {
                    if (_originalMaterial == null)
                    {
                        this._material = new Material(Shader.Find("LiteMagic/Magic Add"));
                    }
                    else
                    {
                        this._material = new Material(_originalMaterial);
                    }

                    this._material.name = "projectorMaterial";
                    this._material.SetTexture("_MainTex", _magicTexture);
                    this._material.SetVector("_MainTex_ST", _magicTextureSt);
                    this._material.SetTexture("_DissolveTex", _DissolveTex);
                    this._material.SetVector("_DissolveTex_ST", _DissolveTexSt);
                }

                this.projector.material = this._material;
            }
        }


#if UNITY_URP
        if (this.decalProjector != null)
        {
            if (this.decalProjector.material == null)
            {
                if (this._material == null)
                {
                    if (_originalMaterial == null)
                    {
                        this._material = new Material(Shader.Find("LiteMagic/MagicShader_URP"));
                    }
                    else
                    {
                        this._material = new Material(_originalMaterial);
                    }
                    this._material.name = "decalProjectorMaterial";
                    this._material.SetTexture("_MainTex", _magicTexture);
                    this._material.SetVector("_MainTex_ST", _magicTextureSt);
                    this._material.SetTexture("_DissolveTex", _DissolveTex);
                    this._material.SetVector("_DissolveTex_ST", _DissolveTexSt);
                }

                this.decalProjector.material = this._material;
                this.decalProjector.enabled = true;
            }
        }


#endif
    }

    private void LateUpdate()
    {
        _time += Time.deltaTime;
        SettingBaseMaterial();
        SettingMaterial("R", MagicSetR);
        SettingMaterial("G", MagicSetG);
        SettingMaterial("B", MagicSetB);
    }

    private void SettingBaseMaterial()
    {
        if (this._material == null) return;

        if (_material.HasProperty(BaseShaderIDs.DissolveTex_ST)) _material.SetVector(BaseShaderIDs.DissolveTex_ST, _DissolveTexSt); ;
        if (_material.HasProperty(BaseShaderIDs.MainTex_ST)) _material.SetVector(BaseShaderIDs.MainTex_ST, _magicTextureSt);
        if (_material.HasProperty(BaseShaderIDs.GroundResidue)) _material.SetFloat(BaseShaderIDs.GroundResidue, _GroundResidue);
    }

    private void SettingMaterial(string _name, MagicSet _magicSet)
    {
        if (this._material == null) return;

        int _SizeName = 0;
        int _RotateName = 0;
        int _DissolveInName = 0;
        int _DissolveOutName = 0;
        int _ColorName = 0;
        int _DissolveColorName = 0;
        int _LightName = 0;
        int _LightGainName = 0;

        switch (_name)
        {
            case "R":
                _SizeName = RMagicShaderIDs.Size;
                _RotateName = RMagicShaderIDs.Rotate;
                _DissolveInName = RMagicShaderIDs.DissolveIn;
                _DissolveOutName = RMagicShaderIDs.DissolveOut;
                _ColorName = RMagicShaderIDs.Color;
                _DissolveColorName = RMagicShaderIDs.DissolveColor;
                _LightName = RMagicShaderIDs.Light;
                _LightGainName = RMagicShaderIDs.LightGain;
                break;
            case "G":
                _SizeName = GMagicShaderIDs.Size;
                _RotateName = GMagicShaderIDs.Rotate;
                _DissolveInName = GMagicShaderIDs.DissolveIn;
                _DissolveOutName = GMagicShaderIDs.DissolveOut;
                _ColorName = GMagicShaderIDs.Color;
                _DissolveColorName = GMagicShaderIDs.DissolveColor;
                _LightName = GMagicShaderIDs.Light;
                _LightGainName = GMagicShaderIDs.LightGain;
                break;
            case "B":
                _SizeName = BMagicShaderIDs.Size;
                _RotateName = BMagicShaderIDs.Rotate;
                _DissolveInName = BMagicShaderIDs.DissolveIn;
                _DissolveOutName = BMagicShaderIDs.DissolveOut;
                _ColorName = BMagicShaderIDs.Color;
                _DissolveColorName = BMagicShaderIDs.DissolveColor;
                _LightName = BMagicShaderIDs.Light;
                _LightGainName = BMagicShaderIDs.LightGain;
                break;
            default:
                break;
        }


        this.SetMaterial();

        if (_magicSet._needAutoRotate)
        {
            _magicSet.Rotate = (_magicSet.Rotate + _magicSet._RotateSpeed * Time.deltaTime) % 360.0f;
        }
        else
        {
            _magicSet.Rotate = _magicSet._originalRotate;
        }


        if (_material.HasProperty(_SizeName)) _material.SetFloat(_SizeName, _magicSet._Size);
        if (_material.HasProperty(_RotateName)) _material.SetFloat(_RotateName, _magicSet.Rotate);
        if (_material.HasProperty(_LightName)) _material.SetFloat(_LightName, Mathf.Clamp(_magicSet._Light, 0.0f, 1.0f));
        if (_material.HasProperty(_LightGainName)) _material.SetFloat(_LightGainName, Mathf.Clamp(_magicSet._LightGain, 1.0f, 20.0f));
        if (_material.HasProperty(_DissolveInName)) _material.SetFloat(_DissolveInName, _magicSet._DissolveIn);
        if (_material.HasProperty(_DissolveOutName)) _material.SetFloat(_DissolveOutName, _magicSet._DissolveOut);
        if (_material.HasProperty(_ColorName)) _material.SetColor(_ColorName, _magicSet._color);
        if (_material.HasProperty(_DissolveColorName)) _material.SetColor(_DissolveColorName, _magicSet._DissolveColor);

    }
}
