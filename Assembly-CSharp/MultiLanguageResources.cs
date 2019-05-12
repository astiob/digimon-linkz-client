using System;
using UnityEngine;

public class MultiLanguageResources
{
	public static UnityEngine.Object Load(string path, Type systemTypeInstance)
	{
		return Resources.Load(path, systemTypeInstance);
	}
}
