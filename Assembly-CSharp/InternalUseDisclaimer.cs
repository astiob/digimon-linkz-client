using System;
using UnityEngine;

public class InternalUseDisclaimer : MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}
}
