using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NodeCanvas.Framework;
using NodeCanvas.BehaviourTrees;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine.Rendering;


#if UNITY_EDITOR && BP_UNITY_EDITOR
using UnityEditor;
#endif


namespace BPGames
{
	public static class BPUtility
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNotNull(this UnityEngine.Object target)
        {
	        return target != null;
        }


        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNotNull(this System.Object target)
        {
	        return target != null;
        }
    }
}
