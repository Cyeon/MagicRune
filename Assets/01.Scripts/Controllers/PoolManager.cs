using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    #region POOL
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform; // 이미 있으면 안되게
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; ++i)
            {
                // 생성
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;

            Poolable component = go.GetComponent<Poolable>();
            if (component == null)
            {
                component = go.AddComponent<Poolable>();
            }
            return component;
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.Reset();
            poolable.gameObject.SetActive(false);
            //poolable.transform.parent = Root;
            poolable.transform.SetParent(Root, false);
            poolable.isUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
            }
            else
            {
                poolable = Create();
            }

            poolable.gameObject.SetActive(true);

            // DontDestroyLoad 해제
            if (parent == null)
            {
                //poolable.transform.parent = SceneManagerEX.Instance.CurrentScene.transform;
                poolable.transform.SetParent(SceneManagerEX.Instance.CurrentScene.transform);
            }

            //poolable.transform.parent = parent;
            poolable.transform.SetParent(parent);
            poolable.isUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (_root == null)
        {
            if (GameObject.Find("@Pool_Root") != null)
            {
                _root = GameObject.Find("@Pool_Root").transform;
                return;
            }
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    public Poolable Pop(string path, Transform parent = null)
    {
        GameObject p = Resources.Load<GameObject>("Prefabs/" + path);
        return Pop(p, parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false) return null;

        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }

        _pool.Clear();
    }
}
