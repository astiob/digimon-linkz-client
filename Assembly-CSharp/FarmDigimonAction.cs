using System;
using System.Collections;
using UnityEngine;

public sealed class FarmDigimonAction : MonoBehaviour
{
	private IEnumerator enumerator;

	private int MASK_LAYER;

	private void Awake()
	{
		this.MASK_LAYER = (1 << LayerMask.NameToLayer("Farm.NoEntry") | 1 << LayerMask.NameToLayer("IgnoreShadow"));
	}

	public void StopAction()
	{
		if (this.enumerator != null)
		{
			base.StopCoroutine(this.enumerator);
			this.enumerator = null;
		}
	}

	public IEnumerator AppearanceNormal(GameObject digimon, Action completed)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		FarmField farmField = farmRoot.Field;
		int gridIndex = FarmDigimonUtility.GetPassableGridIndex();
		if (gridIndex == -1)
		{
			digimon.SetActive(false);
		}
		else
		{
			base.transform.position = farmField.Grid.GetPositionGridCenter(gridIndex, false);
			base.transform.localScale = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			Vector3 angles = base.transform.localEulerAngles;
			angles.y = farmRoot.Camera.transform.localEulerAngles.y + 180f;
			base.transform.localEulerAngles = angles;
			float scale = 0.99f;
			while (1f > scale)
			{
				scale += Time.deltaTime;
				scale = Mathf.Clamp01(scale);
				float adjScale = scale * 2f;
				base.transform.localScale = new Vector3(adjScale, adjScale, adjScale);
				yield return null;
			}
		}
		if (completed != null)
		{
			completed();
		}
		yield break;
	}

	public IEnumerator Walk()
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		FarmField farmField = farmRoot.Field;
		FarmDigimonAI ai = base.GetComponent<FarmDigimonAI>();
		FarmDigimonAI.ActionParam param = ai.GetActionParam();
		float speed = 1.6f;
		if (param.pathGridIndexs != null && 0 < param.pathGridIndexs.Length)
		{
			int nowIndex = param.pathGridIndexs.Length;
			int checkIndex = 0;
			while (checkIndex < nowIndex)
			{
				Vector3 digimonPos3D = base.transform.localPosition;
				digimonPos3D.y = 0f;
				int gridIndex = param.pathGridIndexs[checkIndex];
				Vector3 gridPos3D = farmField.Grid.GetPositionGridCenter(gridIndex, false);
				gridPos3D.y = 0f;
				Vector3 rayDirection = gridPos3D - digimonPos3D;
				digimonPos3D.y = farmRoot.transform.localPosition.y + 0.1f;
				gridPos3D.y = farmRoot.transform.localPosition.y + 0.1f;
				if (!Physics.Raycast(digimonPos3D, rayDirection, rayDirection.magnitude, this.MASK_LAYER))
				{
					digimonPos3D.y = 0f;
					gridPos3D.y = 0f;
					Vector3 digimonDirection = Quaternion.AngleAxis(base.transform.localEulerAngles.y, Vector3.up) * Vector3.forward;
					float angle = Vector3.Angle(digimonDirection, rayDirection);
					Vector3 digimonRightDirection = Quaternion.AngleAxis(90f, Vector3.up) * digimonDirection;
					if (0f > Vector3.Dot(digimonRightDirection, rayDirection))
					{
						angle *= -1f;
					}
					this.enumerator = this.Turn(angle, speed);
					Coroutine coroutine = base.StartCoroutine(this.enumerator);
					if (40f <= Mathf.Abs(angle))
					{
						yield return coroutine;
					}
					this.enumerator = this.Movement(digimonPos3D, gridPos3D, speed);
					yield return base.StartCoroutine(this.enumerator);
					this.enumerator = null;
					nowIndex = checkIndex;
					checkIndex = 0;
				}
				else
				{
					checkIndex += Mathf.Max(1, (nowIndex - checkIndex) / 2);
				}
			}
		}
		yield break;
	}

	public IEnumerator TouchAction()
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		Camera farmCamera = farmRoot.Camera;
		Vector3 cameraDirection = farmCamera.transform.forward * -1f;
		cameraDirection.y = 0f;
		yield return null;
		Vector3 digimonDirection = Quaternion.AngleAxis(base.transform.localEulerAngles.y, Vector3.up) * Vector3.forward;
		float angle = Vector3.Angle(digimonDirection, cameraDirection);
		Vector3 digimonRightDirection = Quaternion.AngleAxis(90f, Vector3.up) * digimonDirection;
		if (0f > Vector3.Dot(digimonRightDirection, cameraDirection))
		{
			angle *= -1f;
		}
		this.enumerator = this.Turn(angle, 1f);
		yield return base.StartCoroutine(this.enumerator);
		this.enumerator = null;
		yield break;
	}

	private IEnumerator Turn(float angle, float walkSpeed)
	{
		float startAngle = base.transform.localEulerAngles.y;
		float targetAngle = startAngle + angle;
		float turnTime = 0.2f;
		float duration = turnTime / walkSpeed;
		float delta = 0f;
		while (duration > delta)
		{
			delta += Time.deltaTime;
			float rate = Mathf.Clamp01(delta / duration);
			float angleY = Mathf.LerpAngle(startAngle, targetAngle, rate);
			Vector3 angles = base.transform.localEulerAngles;
			angles.y = angleY;
			base.transform.localEulerAngles = angles;
			yield return null;
		}
		yield break;
	}

	private IEnumerator Movement(Vector3 startPosition, Vector3 targetPosition, float speed)
	{
		float distance = Vector3.Distance(startPosition, targetPosition);
		float duration = distance / speed;
		float delta = 0f;
		while (duration > delta)
		{
			delta += Time.deltaTime * speed;
			float rate = Mathf.Clamp01(delta / duration);
			base.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, rate);
			yield return null;
		}
		yield break;
	}

	public enum AppearanceType
	{
		NORMAL
	}
}
