using System;
using UnityEngine;

namespace Monster.Model
{
	public sealed class EggModel : MonoBehaviour
	{
		[SerializeField]
		private Transform centerTarget;

		public Transform GetCenter()
		{
			return this.centerTarget;
		}
	}
}
