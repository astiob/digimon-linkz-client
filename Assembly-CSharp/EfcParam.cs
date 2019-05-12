using System;
using UnityEngine;

[AddComponentMenu("GUI/EffectParam")]
public class EfcParam : MonoBehaviour
{
	public Vector3 vOffset;

	public Vector3 vOffsetEnd;

	public float time;

	public float delay;

	public iTween.EaseType type;

	public iTween.EaseType typeEnd;

	public Vector3 vLP_StartScale = new Vector3(1f, 1f, 1f);

	public Vector3 vLP_EndScale = new Vector3(1f, 1f, 1f);

	public float endAlpha;

	public iTween.EaseType scaleType;

	public iTween.EaseType scaleTypeEnd;

	public bool isDoubleList;

	public bool isTopDelayStart = true;

	public bool isTopDelayEnd = true;

	public float lengPerSec = 2000f;
}
