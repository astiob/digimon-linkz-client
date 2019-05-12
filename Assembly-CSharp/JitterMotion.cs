using System;
using UnityEngine;

public class JitterMotion : MonoBehaviour
{
	public float positionFrequency = 0.2f;

	public float rotationFrequency = 0.26f;

	public float positionAmount = 0.2f;

	public float rotationAmount = 1f;

	public Vector3 positionComponents = Vector3.one;

	public Vector3 rotationComponents = new Vector3(1f, 1f, 0f);

	public int positionOctave = 3;

	public int rotationOctave = 3;

	private float timePosition;

	private float timeRotation;

	private Vector2[] noiseVectors;

	private Vector3 initialPosition;

	private Quaternion initialRotation;

	private void Awake()
	{
		this.timePosition = UnityEngine.Random.value * 10f;
		this.timeRotation = UnityEngine.Random.value * 10f;
		this.noiseVectors = new Vector2[6];
		for (int i = 0; i < 6; i++)
		{
			float f = UnityEngine.Random.value * 3.14159274f * 2f;
			this.noiseVectors[i].Set(Mathf.Cos(f), Mathf.Sin(f));
		}
		this.initialPosition = base.transform.localPosition;
		this.initialRotation = base.transform.localRotation;
	}

	private void Update()
	{
		this.timePosition += Time.deltaTime * this.positionFrequency;
		this.timeRotation += Time.deltaTime * this.rotationFrequency;
		if (this.positionAmount != 0f)
		{
			Vector3 vector = new Vector3(JitterMotion.Fbm(this.noiseVectors[0] * this.timePosition, this.positionOctave), JitterMotion.Fbm(this.noiseVectors[1] * this.timePosition, this.positionOctave), JitterMotion.Fbm(this.noiseVectors[2] * this.timePosition, this.positionOctave));
			vector = Vector3.Scale(vector, this.positionComponents) * this.positionAmount * 2f;
			base.transform.localPosition = this.initialPosition + vector;
		}
		if (this.rotationAmount != 0f)
		{
			Vector3 vector2 = new Vector3(JitterMotion.Fbm(this.noiseVectors[3] * this.timeRotation, this.rotationOctave), JitterMotion.Fbm(this.noiseVectors[4] * this.timeRotation, this.rotationOctave), JitterMotion.Fbm(this.noiseVectors[5] * this.timeRotation, this.rotationOctave));
			vector2 = Vector3.Scale(vector2, this.rotationComponents) * this.rotationAmount * 2f;
			base.transform.localRotation = Quaternion.Euler(vector2) * this.initialRotation;
		}
	}

	private static float Fbm(Vector2 coord, int octave)
	{
		float num = 0f;
		float num2 = 1f;
		for (int i = 0; i < octave; i++)
		{
			num += num2 * (Mathf.PerlinNoise(coord.x, coord.y) - 0.5f);
			coord *= 2f;
			num2 *= 0.5f;
		}
		return num;
	}
}
