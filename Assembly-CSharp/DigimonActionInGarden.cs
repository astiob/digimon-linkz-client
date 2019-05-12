using System;
using System.Collections;
using UnityEngine;

public class DigimonActionInGarden : MonoBehaviour
{
	private readonly float walkingSpeed = 1.2f;

	private CharacterParams charaParam;

	private Animation charaAnime;

	private Coroutine currentCoroutine;

	private AnimationClip defaultAnime;

	private bool isPlayingAnime;

	private bool isEgg;

	public bool IsEgg
	{
		get
		{
			return this.isEgg;
		}
	}

	public void Initialize(CharacterParams CharaParam)
	{
		this.charaParam = CharaParam;
	}

	public void Initialize(Animation CharaAnime)
	{
		this.charaAnime = CharaAnime;
		this.isEgg = true;
	}

	public void StopAction()
	{
		if (this.currentCoroutine != null)
		{
			base.StopCoroutine(this.currentCoroutine);
		}
	}

	private void NextAction()
	{
		this.currentCoroutine = base.StartCoroutine(this.Walk());
	}

	public void RandomPosition()
	{
		float num = UnityEngine.Random.Range(-2f, 2f);
		float num2 = Mathf.Sqrt(4f - num * num);
		num2 = UnityEngine.Random.Range(-num2, num2);
		Vector3 localPosition = new Vector3(num, 0f, num2);
		base.transform.localPosition = localPosition;
	}

	public void SetPosition(Vector3 Position)
	{
		base.transform.localPosition = Position;
	}

	public Coroutine WalkAction()
	{
		return base.StartCoroutine(this.Walk());
	}

	private IEnumerator Walk()
	{
		if (this.charaParam == null)
		{
			yield break;
		}
		float x = UnityEngine.Random.Range(-2f, 2f);
		float z = Mathf.Sqrt(4f - x * x);
		z = UnityEngine.Random.Range(-z, z);
		Vector3 targetPos = new Vector3(x, 0f, z);
		Vector3 direction = targetPos - base.transform.localPosition;
		Vector3 digimonDirection = Quaternion.AngleAxis(base.transform.localEulerAngles.y, Vector3.up) * Vector3.forward;
		float angle = Vector3.Angle(digimonDirection, direction);
		Vector3 digimonRightDirection = Quaternion.AngleAxis(90f, Vector3.up) * digimonDirection;
		if (0f > Vector3.Dot(digimonRightDirection, direction))
		{
			angle *= -1f;
		}
		Coroutine coroutine = base.StartCoroutine(this.Turn(angle, this.walkingSpeed));
		if (40f <= Mathf.Abs(angle))
		{
			yield return coroutine;
		}
		this.charaParam.PlayAnimation(CharacterAnimationType.move, SkillType.Attack, 0, null, null);
		yield return base.StartCoroutine(this.Movement(base.transform.localPosition, targetPos, this.walkingSpeed));
		this.NextAction();
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

	public void SetDefaultAnimation(AnimationClip AnimClip)
	{
		if (!this.isEgg)
		{
			return;
		}
		this.charaAnime.clip = AnimClip;
		this.defaultAnime = AnimClip;
		this.charaAnime.Play();
	}

	public void PlayAnimationClip(AnimationClip AnimClip)
	{
		if (this.charaAnime == null || this.isPlayingAnime)
		{
			return;
		}
		if (this.charaAnime.isPlaying)
		{
			this.charaAnime.Stop();
		}
		this.charaAnime.clip = AnimClip;
		this.isPlayingAnime = true;
		base.StartCoroutine(this.PlayAnimationClip_());
	}

	private IEnumerator PlayAnimationClip_()
	{
		this.charaAnime.Play();
		yield return null;
		while (this.charaAnime.isPlaying)
		{
			yield return null;
		}
		this.charaAnime.clip = this.defaultAnime;
		this.charaAnime.Play();
		this.isPlayingAnime = false;
		yield break;
	}
}
