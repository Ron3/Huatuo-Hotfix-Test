using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Networking;
using BPGames;
using UnityEngine.SceneManagement;


/// <summary>
/// 纯粹是挂在热更新场景里面的
/// </summary>
public class RonStartLoad : MonoBehaviour
{
    private void Start() 
    {
        BPResourceManager.Instance.LoadBundle("testRonStartLoad");
    }
}

