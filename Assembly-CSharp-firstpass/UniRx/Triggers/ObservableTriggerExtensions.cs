using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	public static class ObservableTriggerExtensions
	{
		public static IObservable<int> OnAnimatorIKAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<int>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableAnimatorTrigger>(component.gameObject).OnAnimatorIKAsObservable();
		}

		public static IObservable<Unit> OnAnimatorMoveAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableAnimatorTrigger>(component.gameObject).OnAnimatorMoveAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionEnter2DAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(component.gameObject).OnCollisionEnter2DAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionExit2DAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(component.gameObject).OnCollisionExit2DAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionStay2DAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(component.gameObject).OnCollisionStay2DAsObservable();
		}

		public static IObservable<Collision> OnCollisionEnterAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(component.gameObject).OnCollisionEnterAsObservable();
		}

		public static IObservable<Collision> OnCollisionExitAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(component.gameObject).OnCollisionExitAsObservable();
		}

		public static IObservable<Collision> OnCollisionStayAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(component.gameObject).OnCollisionStayAsObservable();
		}

		public static IObservable<Unit> OnDestroyAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Return(Unit.Default);
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableDestroyTrigger>(component.gameObject).OnDestroyAsObservable();
		}

		public static IObservable<Unit> OnEnableAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEnableTrigger>(component.gameObject).OnEnableAsObservable();
		}

		public static IObservable<Unit> OnDisableAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEnableTrigger>(component.gameObject).OnDisableAsObservable();
		}

		public static IObservable<Unit> FixedUpdateAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableFixedUpdateTrigger>(component.gameObject).FixedUpdateAsObservable();
		}

		public static IObservable<Unit> LateUpdateAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableLateUpdateTrigger>(component.gameObject).LateUpdateAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerEnter2DAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(component.gameObject).OnTriggerEnter2DAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerExit2DAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(component.gameObject).OnTriggerExit2DAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerStay2DAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(component.gameObject).OnTriggerStay2DAsObservable();
		}

		public static IObservable<Collider> OnTriggerEnterAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(component.gameObject).OnTriggerEnterAsObservable();
		}

		public static IObservable<Collider> OnTriggerExitAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(component.gameObject).OnTriggerExitAsObservable();
		}

		public static IObservable<Collider> OnTriggerStayAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(component.gameObject).OnTriggerStayAsObservable();
		}

		public static IObservable<Unit> UpdateAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableUpdateTrigger>(component.gameObject).UpdateAsObservable();
		}

		public static IObservable<Unit> OnBecameInvisibleAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableVisibleTrigger>(component.gameObject).OnBecameInvisibleAsObservable();
		}

		public static IObservable<Unit> OnBecameVisibleAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableVisibleTrigger>(component.gameObject).OnBecameVisibleAsObservable();
		}

		public static IObservable<Unit> OnBeforeTransformParentChangedAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(component.gameObject).OnBeforeTransformParentChangedAsObservable();
		}

		public static IObservable<Unit> OnTransformParentChangedAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(component.gameObject).OnTransformParentChangedAsObservable();
		}

		public static IObservable<Unit> OnTransformChildrenChangedAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(component.gameObject).OnTransformChildrenChangedAsObservable();
		}

		public static IObservable<Unit> OnCanvasGroupChangedAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCanvasGroupChangedTrigger>(component.gameObject).OnCanvasGroupChangedAsObservable();
		}

		public static IObservable<Unit> OnRectTransformDimensionsChangeAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableRectTransformTrigger>(component.gameObject).OnRectTransformDimensionsChangeAsObservable();
		}

		public static IObservable<Unit> OnRectTransformRemovedAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableRectTransformTrigger>(component.gameObject).OnRectTransformRemovedAsObservable();
		}

		public static IObservable<BaseEventData> OnDeselectAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<BaseEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableDeselectTrigger>(component.gameObject).OnDeselectAsObservable();
		}

		public static IObservable<AxisEventData> OnMoveAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<AxisEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableMoveTrigger>(component.gameObject).OnMoveAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerDownAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservablePointerDownTrigger>(component.gameObject).OnPointerDownAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerEnterAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservablePointerEnterTrigger>(component.gameObject).OnPointerEnterAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerExitAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservablePointerExitTrigger>(component.gameObject).OnPointerExitAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerUpAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservablePointerUpTrigger>(component.gameObject).OnPointerUpAsObservable();
		}

		public static IObservable<BaseEventData> OnSelectAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<BaseEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableSelectTrigger>(component.gameObject).OnSelectAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerClickAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservablePointerClickTrigger>(component.gameObject).OnPointerClickAsObservable();
		}

		public static IObservable<BaseEventData> OnSubmitAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<BaseEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableSubmitTrigger>(component.gameObject).OnSubmitAsObservable();
		}

		public static IObservable<PointerEventData> OnDragAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableDragTrigger>(component.gameObject).OnDragAsObservable();
		}

		public static IObservable<PointerEventData> OnBeginDragAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableBeginDragTrigger>(component.gameObject).OnBeginDragAsObservable();
		}

		public static IObservable<PointerEventData> OnEndDragAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEndDragTrigger>(component.gameObject).OnEndDragAsObservable();
		}

		public static IObservable<PointerEventData> OnDropAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableDropTrigger>(component.gameObject).OnDropAsObservable();
		}

		public static IObservable<BaseEventData> OnUpdateSelectedAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<BaseEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableUpdateSelectedTrigger>(component.gameObject).OnUpdateSelectedAsObservable();
		}

		public static IObservable<PointerEventData> OnInitializePotentialDragAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableInitializePotentialDragTrigger>(component.gameObject).OnInitializePotentialDragAsObservable();
		}

		public static IObservable<BaseEventData> OnCancelAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<BaseEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCancelTrigger>(component.gameObject).OnCancelAsObservable();
		}

		public static IObservable<PointerEventData> OnScrollAsObservable(this UIBehaviour component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<PointerEventData>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableScrollTrigger>(component.gameObject).OnScrollAsObservable();
		}

		public static IObservable<GameObject> OnParticleCollisionAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<GameObject>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableParticleTrigger>(component.gameObject).OnParticleCollisionAsObservable();
		}

		public static IObservable<Unit> OnParticleTriggerAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableParticleTrigger>(component.gameObject).OnParticleTriggerAsObservable();
		}

		public static IObservable<int> OnAnimatorIKAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<int>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableAnimatorTrigger>(gameObject).OnAnimatorIKAsObservable();
		}

		public static IObservable<Unit> OnAnimatorMoveAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableAnimatorTrigger>(gameObject).OnAnimatorMoveAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionEnter2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(gameObject).OnCollisionEnter2DAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionExit2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(gameObject).OnCollisionExit2DAsObservable();
		}

		public static IObservable<Collision2D> OnCollisionStay2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollision2DTrigger>(gameObject).OnCollisionStay2DAsObservable();
		}

		public static IObservable<Collision> OnCollisionEnterAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(gameObject).OnCollisionEnterAsObservable();
		}

		public static IObservable<Collision> OnCollisionExitAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(gameObject).OnCollisionExitAsObservable();
		}

		public static IObservable<Collision> OnCollisionStayAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collision>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCollisionTrigger>(gameObject).OnCollisionStayAsObservable();
		}

		public static IObservable<Unit> OnDestroyAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Return(Unit.Default);
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableDestroyTrigger>(gameObject).OnDestroyAsObservable();
		}

		public static IObservable<Unit> OnEnableAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEnableTrigger>(gameObject).OnEnableAsObservable();
		}

		public static IObservable<Unit> OnDisableAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableEnableTrigger>(gameObject).OnDisableAsObservable();
		}

		public static IObservable<Unit> FixedUpdateAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableFixedUpdateTrigger>(gameObject).FixedUpdateAsObservable();
		}

		public static IObservable<Unit> LateUpdateAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableLateUpdateTrigger>(gameObject).LateUpdateAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerEnter2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(gameObject).OnTriggerEnter2DAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerExit2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(gameObject).OnTriggerExit2DAsObservable();
		}

		public static IObservable<Collider2D> OnTriggerStay2DAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider2D>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTrigger2DTrigger>(gameObject).OnTriggerStay2DAsObservable();
		}

		public static IObservable<Collider> OnTriggerEnterAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(gameObject).OnTriggerEnterAsObservable();
		}

		public static IObservable<Collider> OnTriggerExitAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(gameObject).OnTriggerExitAsObservable();
		}

		public static IObservable<Collider> OnTriggerStayAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Collider>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTriggerTrigger>(gameObject).OnTriggerStayAsObservable();
		}

		public static IObservable<Unit> UpdateAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableUpdateTrigger>(gameObject).UpdateAsObservable();
		}

		public static IObservable<Unit> OnBecameInvisibleAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableVisibleTrigger>(gameObject).OnBecameInvisibleAsObservable();
		}

		public static IObservable<Unit> OnBecameVisibleAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableVisibleTrigger>(gameObject).OnBecameVisibleAsObservable();
		}

		public static IObservable<Unit> OnBeforeTransformParentChangedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(gameObject).OnBeforeTransformParentChangedAsObservable();
		}

		public static IObservable<Unit> OnTransformParentChangedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(gameObject).OnTransformParentChangedAsObservable();
		}

		public static IObservable<Unit> OnTransformChildrenChangedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableTransformChangedTrigger>(gameObject).OnTransformChildrenChangedAsObservable();
		}

		public static IObservable<Unit> OnCanvasGroupChangedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableCanvasGroupChangedTrigger>(gameObject).OnCanvasGroupChangedAsObservable();
		}

		public static IObservable<Unit> OnRectTransformDimensionsChangeAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableRectTransformTrigger>(gameObject).OnRectTransformDimensionsChangeAsObservable();
		}

		public static IObservable<Unit> OnRectTransformRemovedAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableRectTransformTrigger>(gameObject).OnRectTransformRemovedAsObservable();
		}

		public static IObservable<GameObject> OnParticleCollisionAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<GameObject>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableParticleTrigger>(gameObject).OnParticleCollisionAsObservable();
		}

		public static IObservable<Unit> OnParticleTriggerAsObservable(this GameObject gameObject)
		{
			if (gameObject == null)
			{
				return Observable.Empty<Unit>();
			}
			return ObservableTriggerExtensions.GetOrAddComponent<ObservableParticleTrigger>(gameObject).OnParticleTriggerAsObservable();
		}

		private static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
		{
			T t = gameObject.GetComponent<T>();
			if (t == null)
			{
				t = gameObject.AddComponent<T>();
			}
			return t;
		}
	}
}
