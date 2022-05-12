using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// 原生层基类
/// </summary>
public class OriginLayerBase
{
    public int val;
    public int id = 0;
    public OriginLayerBase(int id=0)
    {
        this.id = id;
    }

    public virtual void ShowInfo()
    {
        UnityEngine.Debug.Log($"OriginLayerBase ShowLog id: {this.id} val: {this.val}");
    }
}

public class LoadDll : MonoBehaviour
{
    

    void Start()
    {
        BetterStreamingAssets.Initialize();
        LoadGameDll();
        RunMain();
    }

    private System.Reflection.Assembly gameAss;

    private void LoadGameDll()
    {
        AssetBundle dllAB = BetterStreamingAssets.LoadAssetBundle("common");
#if !UNITY_EDITOR
        TextAsset dllBytes1 = dllAB.LoadAsset<TextAsset>("HotFix.dll.bytes");
        System.Reflection.Assembly.Load(dllBytes1.bytes);
        TextAsset dllBytes2 = dllAB.LoadAsset<TextAsset>("HotFix2.dll.bytes");
        gameAss = System.Reflection.Assembly.Load(dllBytes2.bytes);
#else
        gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "HotFix2");
#endif

        GameObject testPrefab = GameObject.Instantiate(dllAB.LoadAsset<UnityEngine.GameObject>("HotUpdatePrefab.prefab"));
    }

    public void RunMain()
    {
        if (gameAss == null)
        {
            UnityEngine.Debug.LogError("dll未加载");
            return;
        }
        var appType = gameAss.GetType("App");
        var mainMethod = appType.GetMethod("Main");
        mainMethod.Invoke(null, null);

        // 3, 测试性能
        Stopwatch sw = new Stopwatch();
        sw.Restart();
        for(int i = 0; i < 10000; ++i)
        {
            GameObject gObj = new GameObject();
            gObj.name = $"AOT-{i}";
        }
        sw.Stop();
        UnityEngine.Debug.Log($"原生层生成1w个GameObject耗时: {sw.ElapsedMilliseconds}");

        // 如果是Update之类的函数，推荐先转成Delegate再调用，如
        //var updateMethod = appType.GetMethod("Update");
        //var updateDel = System.Delegate.CreateDelegate(typeof(Action<float>), null, updateMethod);
        //updateMethod(deltaTime);
    }
}
