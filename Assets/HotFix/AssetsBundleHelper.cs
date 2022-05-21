using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR && BP_UNITY_EDITOR
using UnityEditor;
#endif

namespace BPGames
{
	public class ABInfo
	{
		private int refCount;
		public string Name { get; }

		public int RefCount
		{
			get
			{
				return this.refCount;
			}
			set
			{
				//Log.Debug($"{this.Name} refcount: {value}");
				this.refCount = value;
			}
		}

		public AssetBundle AssetBundle { get; }

		public ABInfo(string name, AssetBundle ab)
		{
			this.Name = name;
			this.AssetBundle = ab;
			this.RefCount = 1;
			//Log.Debug($"load assetbundle: {this.Name}");
		}

        /// <summary>
        /// 释放ab包
        /// </summary>
		public void Release()
		{
			this.AssetBundle?.Unload(true);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsNeedRelease()
        {
            if(this.refCount <= 0)
                return true;
            
            return false;
        }
	}
	
	
	// 用于字符串转换，减少GC
	public static class AssetBundleHelper
	{
		public static readonly Dictionary<int, string> IntToStringDict = new Dictionary<int, string>();
		
		public static readonly Dictionary<string, string> StringToABDict = new Dictionary<string, string>();

		public static readonly Dictionary<string, string> BundleNameToLowerDict = new Dictionary<string, string>() 
		{
			{ "StreamingAssets", "StreamingAssets" }
		};
		
		// 缓存包依赖，不用每次计算
		public static Dictionary<string, string[]> DependenciesCache = new Dictionary<string, string[]>();

		public static string IntToString(this int value)
		{
			string result;
			if (IntToStringDict.TryGetValue(value, out result))
			{
				return result;
			}

			result = value.ToString();
			IntToStringDict[value] = result;
			return result;
		}
		
		public static string StringToAB(this string value)
		{
			string result;
			if (StringToABDict.TryGetValue(value, out result))
			{
				return result;
			}

			result = value + ".unity3d";
			StringToABDict[value] = result;
			return result;
		}

		public static string IntToAB(this int value)
		{
			return value.IntToString().StringToAB();
		}
		
		public static string BundleNameToLower(this string value)
		{
			string result;
			if (BundleNameToLowerDict.TryGetValue(value, out result))
			{
				return result;
			}

			result = value.ToLower();
			BundleNameToLowerDict[value] = result;
			return result;
		}
		
		public static string[] GetDependencies(string assetBundleName)
		{
			string[] dependencies = new string[0];
			if (DependenciesCache.TryGetValue(assetBundleName, out dependencies))
			{
				// BPLog.Log($"load depend in cache ==> {assetBundleName}");
				return dependencies;
			}

// 			// 2020-07-09 by Ron. 这里暂不考虑异步的情况.
// 			if (Define.IsAsync == false)
// 			{
// #if UNITY_EDITOR && BP_UNITY_EDITOR
// 				dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, true);
// #else
//                 // 真机走这个.逻辑我在编辑器模式下测试过.没问题了
// 				// 2021-3-25-jeff,  先不处理依赖
// 				if (BPResourceManager.AssetBundleManifestObject.IsNotNull())
// 				{
// 					dependencies = BPResourceManager.AssetBundleManifestObject.GetAllDependencies(assetBundleName);
// 				}
// #endif
// 			}

			// if (BPDefine.IsUseAssetBundle)
            if(true)
			{
				if (BPResourceManager.AssetBundleManifestObject.IsNotNull())
				{
					dependencies = BPResourceManager.AssetBundleManifestObject.GetAllDependencies(assetBundleName);
				}
			}
			else
			{
#if UNITY_EDITOR && BP_UNITY_EDITOR
				dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, true);
#endif
			}

			// 缓存起来
			DependenciesCache.Add(assetBundleName, dependencies);
			return dependencies;
		}


		public static string[] GetSortedDependencies(string assetBundleName)
		{
			Dictionary<string, int> info = new Dictionary<string, int>();
			List<string> parents = new List<string>();
			
            UnityEngine.Debug.Log($"GetSortedDependencies: {assetBundleName}   CollectDependencies 之前");
            CollectDependencies(parents, assetBundleName, info);

            UnityEngine.Debug.Log($"GetSortedDependencies: {assetBundleName}  CollectDependencies 之后");
			string[] ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            
            UnityEngine.Debug.Log($"GetSortedDependencies OrderBy 成功");
			return ss;
		}


		public static void CollectDependencies(List<string> parents, string assetBundleName, Dictionary<string, int> info)
		{
			parents.Add(assetBundleName);
			string[] deps = GetDependencies(assetBundleName);
			foreach (string parent in parents)
			{
				if (!info.ContainsKey(parent))
				{
					info[parent] = 0;
				}
				info[parent] += deps.Length;
			}

			foreach (string dep in deps)
			{
				// BPLog.LogGreen($"----AssetBundleHelper.CollectDependencies assetBundleName = {assetBundleName}, dep = {dep}");
				if (parents.Contains(dep))
				{
					UnityEngine.Debug.Log($"----AssetBundleHelper.CollectDependencies 包有循环依赖 请重新标记 assetBundleName = {assetBundleName}, dep = {dep}");
					throw new Exception($"包有循环依赖，请重新标记: {assetBundleName} {dep}");
				}
				CollectDependencies(parents, dep, info);
			}
			parents.RemoveAt(parents.Count - 1);
		}
	}
}
