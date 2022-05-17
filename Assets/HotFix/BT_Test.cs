using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using BPGames;

namespace BPBTree{
	[Name("Scene事件_显示对话")]
	[Description("在Scene上显示对话")]
	public class BT_Test : ActionTask
    {
		public string text;
		
		public string textGUID = System.Guid.NewGuid().ToString("N");

		protected override string info {

			get { 
                return text;
			}
		}
		
		// Use for initialization. This is called only once in the lifetime of the task.
		// Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}
		
		// public override void AfterDuplicate()
		// {
		// 	this.textGUID = System.Guid.NewGuid().ToString("N");
		// }


		// This is called once each time the task is enabled.
		// Call EndAction() to mark the action as finished, either in success or failure.
		// EndAction can be called from anywhere.
		protected override void OnExecute()
		{
            GameObject go = GameObject.Find(BPDefine.MAIN_GO_NAME);
            if(go != null)
            {
                RonTestHotFixMono mono = go.GetComponent<RonTestHotFixMono>();
                UnityEngine.Debug.Log($"行为树获取mono里面的数据 ==> {mono.btText}");
            }
            else
            {
                UnityEngine.Debug.Log($"获取 {BPDefine.MAIN_GO_NAME} 为null");
            }
            EndAction(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			
		}



		// Called when the task is disabled.
		protected override void OnStop()
        {
			
		}

		// Called when the task is paused.
		protected override void OnPause(){
			
		}
	}
}