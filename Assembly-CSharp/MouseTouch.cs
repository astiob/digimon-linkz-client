using System;
using System.Reflection;
using UnityEngine;

public struct MouseTouch
{
	public int fingerId;

	public Vector2 position;

	public Vector2 deltaPosition;

	public float deltaTime;

	public TouchPhase phase;

	public static Touch createTouch(int finderId, int tapCount, Vector2 position, Vector2 deltaPos, float timeDelta, TouchPhase phase)
	{
		ValueType valueType = default(Touch);
		Type typeFromHandle = typeof(Touch);
		typeFromHandle.GetField("m_FingerId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(valueType, finderId);
		typeFromHandle.GetField("m_TapCount", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(valueType, tapCount);
		typeFromHandle.GetField("m_Position", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(valueType, position);
		typeFromHandle.GetField("m_PositionDelta", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(valueType, deltaPos);
		typeFromHandle.GetField("m_TimeDelta", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(valueType, timeDelta);
		typeFromHandle.GetField("m_Phase", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(valueType, phase);
		return (Touch)valueType;
	}

	public Touch toTouch()
	{
		return MouseTouch.createTouch(this.fingerId, 0, this.position, this.deltaPosition, this.deltaTime, this.phase);
	}

	public static MouseTouch fromTouch(Touch touch)
	{
		return new MouseTouch
		{
			fingerId = touch.fingerId,
			position = touch.position,
			deltaPosition = touch.deltaPosition,
			deltaTime = touch.deltaTime,
			phase = touch.phase
		};
	}

	public static MouseTouch fromInput(MouseState mouseState, ref Vector2? lastMousePosition)
	{
		MouseTouch result = default(MouseTouch);
		result.fingerId = 2;
		if (lastMousePosition != null)
		{
			result.deltaPosition = Input.mousePosition - lastMousePosition.Value;
		}
		if (mouseState == MouseState.DownThisFrame)
		{
			result.phase = TouchPhase.Began;
			lastMousePosition = new Vector2?(Input.mousePosition);
		}
		else if (mouseState == MouseState.UpThisFrame)
		{
			result.phase = TouchPhase.Ended;
			lastMousePosition = null;
		}
		else
		{
			result.phase = TouchPhase.Moved;
			lastMousePosition = new Vector2?(Input.mousePosition);
		}
		result.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		return result;
	}
}
