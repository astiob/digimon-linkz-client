using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class Util
{
	public static Color convertColor(float _r, float _g, float _b, float _a)
	{
		return new Color(_r / 255f, _g / 255f, _b / 255f, _a / 255f);
	}

	public static void SetX(this Transform transform, float x)
	{
		Vector3 position = new Vector3(x, transform.position.y, transform.position.z);
		transform.position = position;
	}

	public static void SetY(this Transform transform, float y)
	{
		Vector3 position = new Vector3(transform.position.x, y, transform.position.z);
		transform.position = position;
	}

	public static void SetZ(this Transform transform, float z)
	{
		Vector3 position = new Vector3(transform.position.x, transform.position.y, z);
		transform.position = position;
	}

	public static void SetLocalX(this Transform transform, float x)
	{
		Vector3 localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
		transform.localPosition = localPosition;
	}

	public static void SetLocalY(this Transform transform, float y)
	{
		Vector3 localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
		transform.localPosition = localPosition;
	}

	public static void SetLocalZ(this Transform transform, float z)
	{
		Vector3 localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
		transform.localPosition = localPosition;
	}

	public static void Clear(this Transform transform)
	{
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
	}

	public static string Slice(this string aString, int num)
	{
		return aString.Substring(num, aString.Length - 1);
	}

	public static int ToInt32(this string s)
	{
		return int.Parse(s);
	}

	public static float ToFloat(this string s)
	{
		return float.Parse(s);
	}

	public static int ToInt32OrDefault(this string s, int defaultValue = 0)
	{
		int result;
		if (int.TryParse(s, out result))
		{
			return result;
		}
		return defaultValue;
	}

	public static void SetLayer(GameObject gO, int mask)
	{
		gO.layer = mask;
		foreach (object obj in gO.transform)
		{
			Transform transform = (Transform)obj;
			Util.SetLayer(transform.gameObject, mask);
		}
	}

	public static void SetSpriteColor(GameObject gO, Color col)
	{
		foreach (object obj in gO.transform)
		{
			Transform transform = (Transform)obj;
			Util.SetSpriteColor(transform.gameObject, col);
		}
	}

	public static void PauseAnimations(Animation aO, bool flag)
	{
		float speed = 1f;
		if (flag)
		{
			speed = 0f;
		}
		foreach (object obj in aO)
		{
			AnimationState animationState = (AnimationState)obj;
			animationState.speed = speed;
		}
		aO.Sample();
	}

	public static void SwitchCompornents(GameObject gO, bool flag)
	{
		MonoBehaviour[] componentsInChildren = gO.GetComponentsInChildren<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in componentsInChildren)
		{
			monoBehaviour.enabled = flag;
		}
	}

	private static byte[] GetHash(string inputString)
	{
		HashAlgorithm hashAlgorithm = SHA1.Create();
		return hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
	}

	public static string GetHashString(string inputString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in Util.GetHash(inputString))
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public static int ToInteger(this Enum anEnum)
	{
		return int.Parse(anEnum.ToString("d"));
	}

	public static IEnumerator WaitForRealTime(float delay)
	{
		float pauseEndTime = RealTime.time + delay;
		while (RealTime.time < pauseEndTime)
		{
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}
}
