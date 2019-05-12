using System;
using UnityEngine;

public interface ITouchEvent
{
	event Action<Touch, Vector2> onTouchBegan;

	event Action<Touch, Vector2> onTouchMoved;

	event Action<Touch, Vector2, bool> onTouchEnded;

	string gName { get; }

	GameObject gObject { get; }

	float distance { get; set; }

	int generation { get; set; }

	bool useSubPhase { get; set; }

	bool removePhase { get; }

	void OnTouchInit();

	void OnTouchBegan(Touch touch, Vector2 pos);

	void OnTouchMoved(Touch touch, Vector2 pos);

	void OnTouchEnded(Touch touch, Vector2 pos, bool flag);

	bool isTouchBegan { get; }

	bool isTouchMoved { get; }

	bool isTouchEnded { get; }
}
