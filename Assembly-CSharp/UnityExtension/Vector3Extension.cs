using System;
using UnityEngine;

namespace UnityExtension
{
	public static class Vector3Extension
	{
		public static float GetMaxFloat(Vector3 value)
		{
			return Mathf.Max(new float[]
			{
				value.x,
				value.y,
				value.z
			});
		}
	}
}
