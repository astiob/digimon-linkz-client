using System;

namespace UniRx
{
	public enum MainThreadDispatchType
	{
		Update,
		FixedUpdate,
		EndOfFrame,
		GameObjectUpdate,
		LateUpdate,
		[Obsolete]
		AfterUpdate
	}
}
