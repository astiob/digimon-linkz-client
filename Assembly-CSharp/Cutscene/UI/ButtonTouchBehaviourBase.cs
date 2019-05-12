using System;
using System.Diagnostics;
using UnityEngine;

namespace Cutscene.UI
{
	[RequireComponent(typeof(BoxCollider))]
	public abstract class ButtonTouchBehaviourBase : MonoBehaviour
	{
		protected BoxCollider selfCollider;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event Action onTouch;

		protected void DoAction()
		{
			if (this.onTouch != null)
			{
				this.onTouch();
			}
		}

		protected abstract void OnInitialize();

		public void Initialize()
		{
			this.selfCollider = base.GetComponent<BoxCollider>();
			this.OnInitialize();
		}

		public void AddAction(Action action)
		{
			this.onTouch += action;
		}

		public void RemoveAction(Action action)
		{
			this.onTouch -= action;
		}
	}
}
