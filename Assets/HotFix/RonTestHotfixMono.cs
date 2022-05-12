using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// 这个脚本是我拿来专门测试huatuo的,看它支持到什么程度
/// </summary>
public class RonTestHotFixMono : MonoBehaviour
{
    private List<RonNewClass> objList = new List<RonNewClass>();
    private List<HotfixLayerSub> hotfixClassObjList = new List<HotfixLayerSub>();

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        // 定义测试的数量
        int countNum = 10000;

        // 1, 测试协程
        this.StartCoroutine(this._TestCoroutine());

        // 2, 测试新增类
        this.objList.Add(new RonNewClass(1));
        this.objList.Add(new RonNewClass(2));
        this.objList.Add(new RonNewClass(3));
        foreach(RonNewClass obj in this.objList)
            obj.ShowLog();
        
        // 3, 测试性能
        Stopwatch sw = new Stopwatch();
        sw.Restart();
        for(int i = 0; i < countNum; ++i)
        {
            GameObject gObj = new GameObject();
            gObj.name = $"{i}";
        }
        sw.Stop();
        UnityEngine.Debug.Log($"hotfix层生成1w个GameObject耗时: {sw.ElapsedMilliseconds}");

        // 4, new跨域继承的类
        this.hotfixClassObjList.Clear();
        sw.Restart();
        for(int i = 0; i < countNum; ++i)
        {
            this.hotfixClassObjList.Add(new HotfixLayerSub(i, $"sub-{i}"));
        }
        sw.Stop();
        UnityEngine.Debug.Log($"[重新修改] hotfix层生成1w个 热更层的子类 耗时: {sw.ElapsedMilliseconds}");
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
}
