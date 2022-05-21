using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Networking;
using BPGames;
using UnityEngine.SceneManagement;


/// <summary>
/// 这个脚本是我拿来专门测试huatuo的,看它支持到什么程度
/// </summary>
public class RonTestHotFixMono : MonoBehaviour
{
    private List<RonNewClass> objList = new List<RonNewClass>();
    private List<HotfixLayerSub> hotfixClassObjList = new List<HotfixLayerSub>();
    private AssetBundle ronAB = null;
    private AssetBundle sceneAB = null;

    public string btText = "这句话是要显示的在行为树里的!";

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        // 底层已经初始化过.这里就不在初始化了
        // BetterStreamingAssets.Initialize();
        this.ronAB = BetterStreamingAssets.LoadAssetBundle("ronAB");
        if(this.ronAB != null)
        {
            UnityEngine.Debug.Log($"Hotfix层加载RonAB包成功~~~");
        }
        else
        {
            UnityEngine.Debug.Log($"Hotfix层加载RonAB包失败~~~");
        }

        this.sceneAB = BetterStreamingAssets.LoadAssetBundle("scene");
        if(this.sceneAB != null)
        {
            UnityEngine.Debug.Log($"Hotfix层加载SceneAB包成功~~~");
        }
        else
        {
            UnityEngine.Debug.Log($"Hotfix层加载SceneAB包失败~~~");
        }
    }

    private void OnDestroy() 
    {
        // 暂时先这样
        this.ronAB.Unload(false);
        this.sceneAB.Unload(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        BPResourceManager.Instance.LoadBundle("test");

        // // 定义测试的数量
        // int countNum = 10000;
        
        // // 1, 测试协程
        // this.StartCoroutine(this._TestCoroutine());

        // // 2, 测试新增类
        // this.objList.Add(new RonNewClass(1));
        // this.objList.Add(new RonNewClass(2));
        // this.objList.Add(new RonNewClass(3));
        // foreach(RonNewClass obj in this.objList)
        //     obj.ShowLog();
        
        // // 3, 测试性能
        // Stopwatch sw = new Stopwatch();
        // sw.Restart();
        // for(int i = 0; i < countNum; ++i)
        // {
        //     GameObject gObj = new GameObject();
        //     gObj.name = $"{i}";
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log($"hotfix层生成1w个GameObject耗时: {sw.ElapsedMilliseconds}");

        // // 4, new跨域继承的类
        // this.hotfixClassObjList.Clear();
        // sw.Restart();
        // for(int i = 0; i < countNum; ++i)
        // {
        //     this.hotfixClassObjList.Add(new HotfixLayerSub(i, $"sub-{i}"));
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log($"[重新修改] hotfix层生成1w个 热更层的子类 耗时: {sw.ElapsedMilliseconds}");

        // // 4, AssetBundle dllAB = BetterStreamingAssets.LoadAssetBundle("common");
        // if(this.ronAB != null)
        // {
        //     GameObject ronPrefab = GameObject.Instantiate(this.ronAB.LoadAsset<UnityEngine.GameObject>("ron_1.prefab"));
        //     if(ronPrefab != null)
        //     {
        //         UnityEngine.Debug.Log($"生成ronPrefab成功~~~");
        //     }
        //     else
        //     {
        //         UnityEngine.Debug.Log($"生成ronPrefab失败~~~");
        //     }
        // }

        // // 5, 测试异步
        // this._TestAsync();

        // 测试加载场景
        // this.sceneAB.LoadAsset<UnityEngine.Scene>();
        // SceneManager.LoadScene("HotUpdateScene");

    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {

    }

    /// <summary>
    /// 测试协程
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator _TestCoroutine()
    {
        UnityEngine.Debug.Log($"进入协程函数 ...");
        yield return new WaitForSeconds(2);
        UnityEngine.Debug.Log($"离开协程函数 ...");
    }


    /// <summary>
    /// 测试异步的
    /// </summary>
    /// <returns></returns>
    private async void _TestAsync()
    {
        UnityEngine.Debug.Log($"进入 _TestAsync");
        await this._TestCopyAB();
        UnityEngine.Debug.Log($"离开 _TestAsync");
    }


    private async System.Threading.Tasks.Task<bool> _TestCopyAB()
    {
        using(UnityWebRequest request = UnityWebRequest.Get("https://baidu.com"))
        {
            await request.SendWebRequest();
            UnityEngine.Debug.Log($"response Code ==> {request.responseCode}");
            UnityEngine.Debug.Log($"response Text ==> {request.downloadHandler.text.Length}");
        }
        
        return true;
    }
}
