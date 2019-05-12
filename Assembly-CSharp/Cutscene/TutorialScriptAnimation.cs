using System;
using UnityEngine;

namespace Cutscene
{
	public sealed class TutorialScriptAnimation : MonoBehaviour
	{
		public bool IsPlaying()
		{
			return base.gameObject.activeSelf;
		}

		public void StartAnimation()
		{
			base.gameObject.SetActive(true);
		}
	}
}
