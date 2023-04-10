using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Managers : MonoBehaviour
{
    #region Instance
    private static Managers _instance;
    private static Managers Instance { get { Init(); return _instance; } }
    #endregion

    #region CORE
    private UIManager _ui = new UIManager();
    private MapManager _map = new MapManager();
    private PoolManager _pool = new PoolManager();
    private DeckManager _deck = new DeckManager();
    private RuneManager _rune = new RuneManager();
    private GoldManager _gold = new GoldManager();
    private GameManager _game = new GameManager();
    private SoundManager _sound = new SoundManager();
    private CanvasManager _canvas = new CanvasManager();
    private SceneManagerEX _scene = new SceneManagerEX();
    private ResourceManager _resource = new ResourceManager();

    public static UIManager UI {  get { return Instance._ui; } }
    public static MapManager Map { get { return Instance._map; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static DeckManager Deck { get { return Instance._deck; } }
    public static RuneManager Rune { get { return Instance._rune; } }
    public static GoldManager Gold { get { return Instance._gold; } }
    public static GameManager Game { get { return Instance._game; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static CanvasManager Canvas { get { return Instance._canvas; } }
    public static SceneManagerEX Scene { get { return Instance._scene; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    #endregion

    private bool _preparedToQuit = false;

    private static Player _player;

    private void Awake()
    {
        Application.targetFrameRate = 30;
        DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(500, 50);

        Init();
    }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("Managers");
            if (go == null)
            {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();

            _instance._sound.Init();
            _instance._pool.Init();
            _instance._canvas.Init(true);
            _instance._map.Init();
            _instance._deck.Init();

            if(_player == null)
            {
                _player = FindObjectOfType<Player>();
            }
        }
    }

    private void Update()
    {
        BackButtonAction();
    }

    private void BackButtonAction()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_preparedToQuit == false)
            {
                AndroidToast.Instance.ShowToastMessage("뒤로가기 버튼을 한 번 더 누르시면 종료합니다.");
                PreparedToQuit();
            }
            else
            {
                GameQuit();
            }
        }
    }

    private void PreparedToQuit()
    {
        StartCoroutine(PreparedToQuitCoroutine());
    }

    private IEnumerator PreparedToQuitCoroutine()
    {
        _preparedToQuit = true;
        yield return new WaitForSecondsRealtime(2.5f);
        _preparedToQuit = false;
    }

    public static void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static Player GetPlayer()
    {
        return _player;
    }

    public static void Clear()
    {
        UI.Clear();
        Scene.Clear();
        Pool.Clear();
        Canvas.Clear();
    }
}
