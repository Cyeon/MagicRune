using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    private static Managers Instance { get { Init(); return _instance; } }

    #region CORE
    private UIManager _ui = new UIManager();
    private PoolManager _pool = new PoolManager();
    private SoundManager _sound = new SoundManager();
    private CanvasManager _canvas = new CanvasManager();
    private SceneManagerEX _scene = new SceneManagerEX();
    private ResourceManager _resource = new ResourceManager();

    public static UIManager UI {  get { return Instance._ui; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static CanvasManager Canvas { get { return Instance._canvas; } }
    public static SceneManagerEX Scene { get { return Instance._scene; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    #endregion

    private void Awake()
    {
        Init();
    }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();

            //_instance._sound.Init();
            _instance._pool.Init();
            _instance._canvas.Init(true);
        }
    }

    public static void Clear()
    {
        UI.Clear();
        Scene.Clear();
        Pool.Clear();
        Canvas.Clear();
    }
}
