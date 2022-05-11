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
        Debug.Log("hello, huatuo. 当你看到这个,就是热更新成功. Jeff 222222222222");

        var go = new GameObject("HotFix2 -- HotFix修改过的");
        go.AddComponent<CreateByHotFix2>();

        return 0;
    }
}
