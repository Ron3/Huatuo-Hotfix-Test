using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Linq;


#if UNITY_EDITOR && BP_UNITY_EDITOR
using UnityEditor;
#endif


namespace BPGames
{
    // public class ABInfo
    // {
    //     private int refCount;

    //     public string Name { get; }
    //     public AssetBundle AssetBundle { get; }

	// 	public ABInfo(string name, AssetBundle ab)
	// 	{
	// 		this.Name = name;
	// 		this.AssetBundle = ab;
	// 		this.refCount = 1;
	// 		//Log.Debug($"load assetbundle: {this.Name}");
	// 	}
    // }


    /// <summary>
    /// 资源管理的类.把接口实现出来.以后即使使用ET框架,那么也很容易
    /// </summary>
    public class BPResourceManager
    {
        private static BPResourceManager _ins = null;

        // 缓存ab包
        private Dictionary<string, ABInfo> abInfoDict;

        // 缓存ab包里的资源.
        private Dictionary<string, Dictionary<string, UnityEngine.Object>> resourceCacheDict;

        // 2021-4-1-jeff,  先不处理依赖
		public static AssetBundleManifest AssetBundleManifestObject { get; set; }
		public static AssetBundle dependAssetBundle;

		// 专门为异步加载回来的缓存在这里
		private Dictionary<string, AssetBundle> cacheAssetBundleDict;

                /// <summary>
        /// 单例
        /// </summary>
        public static BPResourceManager Instance
        {
            get
            {
                if(_ins == null)
                {
                    _ins = new BPResourceManager();

                    // 2021-3-31-jeff,  先不处理依赖
// #if !UNITY_EDITOR
					// 真机才需要判定.编辑器模式下根本不是这样获取包的依赖信息
					// _ins.LoadOneBundle("StreamingAssets");
					// AssetBundleManifestObject = (AssetBundleManifest)_ins.GetAsset("StreamingAssets", "AssetBundleManifest");
					// if(AssetBundleManifestObject == null)
					// {
					// 	throw new Exception("Load AssetBundleManifestObject Error");
					// }
				
					// 1, 开始逻辑.指定是包内的StreamingAssets. 遍历ab包
					string streamingABName = PathHelper.StreamingABName;
					// 先找 更新目录下
					string streamingABPath = Path.Combine(PathHelper.AppHotfixResPath, streamingABName);
					// if (File.Exists(streamingABPath))
					// {
					// 	BPLog.LogGreen($"----BPResourceManager.Instance---- streamingABPath = {streamingABPath}");
					// 	dependAssetBundle = AssetBundle.LoadFromFile(streamingABPath);
					// }
					// else
					// {
					// 	streamingABPath = Path.Combine(PathHelper.AppResPath, streamingABName);
					// 	BPLog.LogGreen($"----BPResourceManager.Instance---- streamingABPath = {streamingABPath}");
					// 	dependAssetBundle = AssetBundle.LoadFromFile(streamingABPath);
					// }
					// 更新目录下 没有， 找 streamingAssetsPath 下的

					// if(File.Exists(Path.Combine(PathHelper.AppResPath, streamingABName)) == true)
					// {
					// 	BPLog.Log($"原生路径判定成功 ==> {Path.Combine(PathHelper.AppResPath, streamingABName)}!");
					// }
					// else
					// {
					// 	BPLog.Log($"原生路径判定失败 ==> {Path.Combine(PathHelper.AppResPath, streamingABName)}!");
					// }

					// if(File.Exists(streamingABPath) == true)
					// {
					// 	BPLog.Log($"热更新目录判定成功 ==> {streamingABPath}");
					// }
					// else
					// {
					// 	BPLog.Log($"热更新目录判定失败 ==> {streamingABPath}");
					// }

					// if (BPDefine.IsUseAssetBundle)
                    if(true)
					{
						dependAssetBundle = AssetBundle.LoadFromFile(streamingABPath);
					}
					if(dependAssetBundle == null)
					{
						UnityEngine.Debug.Log($"dependAssetBundle 111 热更新目录不存在.尝试原生目录加载. 热更新Path => {streamingABPath}");
						streamingABPath = Path.Combine(PathHelper.AppResPath, streamingABName);
						dependAssetBundle = AssetBundle.LoadFromFile(streamingABPath);
						if(dependAssetBundle == null)
							UnityEngine.Debug.Log($"dependAssetBundle 222 原生目录也不存在.原生Path => {streamingABPath}");
					}

					if (dependAssetBundle.IsNotNull())
					{
						AssetBundleManifestObject = dependAssetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
						if(AssetBundleManifestObject != null)
							UnityEngine.Debug.Log($"AssetBundleManifestObject is ok");
						else
							UnityEngine.Debug.Log($"AssetBundleManifestObject is null");
					}

	                // if(File.Exists(streamingABPath) == false)
					// {
					// 	streamingABPath = Path.Combine(PathHelper.AppResPath, streamingABName);
					// }
	                
	                // if(File.Exists(streamingABPath))
	                // {
		            //     BPLog.LogGreen($"----BPResourceManager.Instance---- 111 streamingABPath = {streamingABPath}");
					// 	dependAssetBundle = AssetBundle.LoadFromFile(streamingABPath);
	                // }
					// else
					// {
					// 	dependAssetBundle = AssetBundle.LoadFromFile(streamingABPath);
					// 	BPLog.LogGreen($"----BPResourceManager.Instance---- 2222 streamingABPath = {streamingABPath}");
					// }

					// if (dependAssetBundle.IsNotNull())
					// {
					// 	AssetBundleManifestObject = dependAssetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
					// }
					
					// ios: Ron streamingABPath ===> /var/containers/Bundle/Application/660C8FCB-3AEF-4126-A8A4-8A517CCE9ECD/t.app/Data/Raw/StreamingAssets
					// BPLog.Log($"Ron streamingABPath ===> {streamingABPath}");
					// BPResourceManager.dependAssetBundle = AssetBundle.LoadFromFile(streamingABPath);
					// BPResourceManager.AssetBundleManifestObject = (AssetBundleManifest)BPResourceManager.dependAssetBundle.LoadAsset("AssetBundleManifest");


					// if(BPResourceManager.dependAssetBundle.IsNull() == true)
					// {
					// 	BPLog.Log($" BPResourceManager.dependAssetBundle.IsNull ");
					// }

					// if(BPResourceManager.AssetBundleManifestObject.IsNull() == true)
					// {
					// 	BPLog.Log($" BPResourceManager.AssetBundleManifestObject.IsNull ");
					// }

					// if(BPResourceManager.dependAssetBundle.IsNull() || BPResourceManager.AssetBundleManifestObject.IsNull())
					// {
					// 	BPLog.LogRed("----BPResourceManager.Instance---- manifestObject is null ==> ");
					// 	// return null;
					// 	// assetBundle.Unload(true);
					// }
					// else
					// {
					// 	BPLog.LogGreen("----BPResourceManager.Instance---- manifestObject is ok ==> ");
					// }
// #endif					
                }
                
                return _ins;
            }
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public BPResourceManager()
        {
            this.abInfoDict = new Dictionary<string, ABInfo>();
            this.resourceCacheDict = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();
			this.cacheAssetBundleDict = new Dictionary<string, AssetBundle>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="sp"></param>
        public void LoadBundle(string assetBundleName, Stopwatch sp = null)
        {
            AssetBundle bundle = GetAssetBundle(assetBundleName);
			if(bundle.IsNotNull())
	        {
		        return;
	        }

            UnityEngine.Debug.Log($"GetSortedDependencies 之前");
            assetBundleName = assetBundleName.ToLower();
            string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);
            UnityEngine.Debug.Log($"GetSortedDependencies 之后");

            if(sp.IsNotNull())
            {
                sp.Stop();
                UnityEngine.Debug.Log($"sp关闭 assetBundleName: {assetBundleName}");
            }
            else
            {
                UnityEngine.Debug.Log($"sp是null");
            }
        }
    

        /// <summary>
        /// 获取ab包
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        public AssetBundle GetAssetBundle(string assetBundleName)
        {
			assetBundleName = assetBundleName.ToLower();
			UnityEngine.Debug.Log($"GetAssetBundle this.abInfoDict.Count ==> {this.abInfoDict.Count}");

			ABInfo abInfo = null;
			this.abInfoDict.TryGetValue(assetBundleName, out abInfo);
            if(abInfo.IsNotNull())
            {
                UnityEngine.Debug.Log($"abInfo 已经加载!");
                return abInfo.AssetBundle;
            }
            else
            {
                UnityEngine.Debug.Log($"abInfo 尚未加载!");
                return null;
            }
        }
	}
}


