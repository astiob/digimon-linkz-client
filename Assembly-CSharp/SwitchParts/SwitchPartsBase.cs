using System;
using UnityEngine;

namespace SwitchParts
{
	public abstract class SwitchPartsBase : MonoBehaviour
	{
		public abstract void Switch(bool enable);

		public abstract bool Status();
	}
}
