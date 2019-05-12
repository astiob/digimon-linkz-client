using System;
using UnityEngine;

namespace NGUI.Extensions
{
	public interface IUISafeAreaChildren
	{
		void SetAnchorTarget(Transform safeArea);
	}
}
