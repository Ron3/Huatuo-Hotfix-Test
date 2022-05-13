using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.CompilerServices;


namespace BPGames
{
    public class AcceptAllCertificate: CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }

    
    /// <summary>
    /// https://gist.github.com/krzys-h/9062552e33dd7bd7fe4a6c12db109a1a
    /// 作者最初的是有bug的, 参考这个人的修复 nickyonge commented on 20 Aug
    /// </summary>
    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        private UnityWebRequestAsyncOperation asyncOp;
        private Action continuation = null;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this.asyncOp = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }

        public bool IsCompleted { get { return asyncOp.isDone; } }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }
        
        private void OnRequestCompleted(AsyncOperation obj)
        {
            this.continuation?.Invoke();
        }
    }
}


