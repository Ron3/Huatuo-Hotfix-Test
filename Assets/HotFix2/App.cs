using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class App
{
    public static int Main()
    {
        Debug.Log("hello, huatuo. 当你看到这个,就是热更新成功. Jeff 3333333");

        var go = new GameObject("HotFix2 -- HotFix修改过的");
        // 改个名字.后面行为树要去搜索用
        go.name = BPDefine.MAIN_GO_NAME;
        go.AddComponent<CreateByHotFix2>();
        go.AddComponent<RonTestHotFixMono>();

        return 0;
    }
}
