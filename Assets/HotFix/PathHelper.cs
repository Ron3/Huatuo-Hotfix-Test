using UnityEngine;

namespace BPGames
{
    public static class PathHelper
    {    
        
        public static string VersionTxtName
        {
            get{
                return "Version.txt";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public static string OriginVersionTxtPath
        {
            get{
                string streamingABPath = System.IO.Path.Combine(PathHelper.AppResPath4Web, VersionTxtName);
                return streamingABPath;
            }
        }

        /// <summary>
        /// 热更新目录的
        /// </summary>
        /// <value></value>
        public static string LocalHotfixVersionTxtPath
        {
            get{
#if UNITY_EDITOR && BP_UNITY_EDITOR
                string versionPath = Application.dataPath + $"/StreamingAssets/{VersionTxtName}";                
#else
                string versionPath = System.IO.Path.Combine(PathHelper.AppHotfixResPath, VersionTxtName);
#endif               
                return versionPath;
            }
        }

        /// <summary>
        /// 本机versionTxt的路径
        /// </summary>
        /// <value></value>
        public static string ServerVersionTxtPath
        {
            get{
                string versionUrl = $"{GlobalProto.GetUrl()}/StreamingAssets/{VersionTxtName}";
                return versionUrl;
            }
        }

        /// <summary>
        ///应用程序外部资源路径存放路径(热更新资源路径)
        /// </summary>
        public static string AppHotfixResPath
        {
            get
            {
                string game = Application.productName;
                string path = AppResPath;
                if (Application.isMobilePlatform)
                {
                    path = $"{Application.persistentDataPath}/{game}/";
                }
                return path;
            }
        }

        /// <summary>
        /// 热更新临时目录
        /// </summary>
        /// <value></value>
        public static string AppHotfixResTempPath
        {
            get
            {
                string dirName = "hotfixTemp";
                string path = AppResPath;
                // if (Application.isMobilePlatform)
                // {
                //     path = $"{Application.persistentDataPath}/{dirName}/";
                // }
                // TODO: Ron 测试,先这样
                path = $"{Application.persistentDataPath}/{dirName}/";
                return path;
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath
        {
            get
            {
                return Application.streamingAssetsPath;       
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径(www/webrequest专用)
        /// </summary>
        public static string AppResPath4Web
        {
            get
            {
// by Ron 2021-04-21 不能出现 UNITY_STANDALONE_OSX 这个标记(目前我们只有iOS/Android平台.如果以后有OSX平台再说吧)
// #if UNITY_IOS || UNITY_STANDALONE_OSX
#if BP_UNITY_IOS
                // UnityEngine.Debug.Log($"Ron AppResPath4Web ios3平台");
                return $"file://{Application.streamingAssetsPath}";
#else
                // UnityEngine.Debug.Log($"Ron AppResPath4Web android3平台 {Application.streamingAssetsPath}");
                return Application.streamingAssetsPath;
#endif

            }
        }
        
        /// <summary>
        /// 获得 StandaloneOSXUniversal/IOS/ANDROID ab 包名字
        /// </summary>
        public static string StreamingABName
        {
            get
            {
                // 所有都用这个
                return "StreamingAssets"; 
// #if UNITY_ANDROID || UNITY_IOS
//                 return "StreamingAssets";
// #elif UNITY_STANDALONE_OSX
//                 return "StandaloneOSXUniversal";
// #elif UNITY_STANDALONE
//                 return "StandaloneWindows";
// #else
                // return "StreamingAssets";
// #endif

            }
        }
        
    }


    /// <summary>
    /// 
    /// </summary>
    public static class GlobalProto
	{
        public static string AssetBundleServerUrl = "";
		// public static string AssetBundleServerUrl = BPDefine.AB_SERVER_URL;
		// public static string Address;

        /// <summary>
        /// 获取热更新的url
        /// </summary>
        /// <returns></returns>
		public static string GetUrl()
		{
			string url = AssetBundleServerUrl;
#if BP_UNITY_ANDROID
			url += "Android/";
#elif BP_UNITY_IOS
			url += "IOS/";
#elif UNITY_WEBGL
			url += "WebGL/";
#elif UNITY_STANDALONE_OSX
			url += "MacOS/";
#else
			url += "PC/";
#endif
			// Log.Debug(url);
			return url;
		}
	}
}
