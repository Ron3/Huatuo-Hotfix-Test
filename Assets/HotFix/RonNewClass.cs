using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 热更新的子类
/// </summary>
public class HotfixLayerSub : OriginLayerBase
{
    public string subName = "";
    public HotfixLayerSub(int id, string subName):base(id)
    {
        this.subName = subName;
    }

    public override void ShowInfo()
    {
        UnityEngine.Debug.Log($"HotfixLayerSub ShowLog. id: {this.id}, subName: {this.subName}");
    }
}

public class RonNewClass
{
    public int id;
    public string textGUID = System.Guid.NewGuid().ToString("N");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public RonNewClass(int id=0)
    {
        this.id = id;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowLog()
    {
        UnityEngine.Debug.Log($"{this.id}: RonNewClass => {this.textGUID}");
    }
}

