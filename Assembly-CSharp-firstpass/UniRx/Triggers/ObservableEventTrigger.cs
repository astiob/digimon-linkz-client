using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableEventTrigger : ObservableTriggerBase, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
	{
		private Subject<BaseEventData> onDeselect;

		private Subject<AxisEventData> onMove;

		private Subject<PointerEventData> onPointerDown;

		private Subject<PointerEventData> onPointerEnter;

		private Subject<PointerEventData> onPointerExit;

		private Subject<PointerEventData> onPointerUp;

		private Subject<BaseEventData> onSelect;

		private Subject<PointerEventData> onPointerClick;

		private Subject<BaseEventData> onSubmit;

		private Subject<PointerEventData> onDrag;

		private Subject<PointerEventData> onBeginDrag;

		private Subject<PointerEventData> onEndDrag;

		private Subject<PointerEventData> onDrop;

		private Subject<BaseEventData> onUpdateSelected;

		private Subject<PointerEventData> onInitializePotentialDrag;

		private Subject<BaseEventData> onCancel;

		private Subject<PointerEventData> onScroll;

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			if (this.onDeselect != null)
			{
				this.onDeselect.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnDeselectAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onDeselect) == null)
			{
				result = (this.onDeselect = new Subject<BaseEventData>());
			}
			return result;
		}

		void IMoveHandler.OnMove(AxisEventData eventData)
		{
			if (this.onMove != null)
			{
				this.onMove.OnNext(eventData);
			}
		}

		public IObservable<AxisEventData> OnMoveAsObservable()
		{
			Subject<AxisEventData> result;
			if ((result = this.onMove) == null)
			{
				result = (this.onMove = new Subject<AxisEventData>());
			}
			return result;
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.onPointerDown != null)
			{
				this.onPointerDown.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerDownAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerDown) == null)
			{
				result = (this.onPointerDown = new Subject<PointerEventData>());
			}
			return result;
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (this.onPointerEnter != null)
			{
				this.onPointerEnter.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerEnterAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerEnter) == null)
			{
				result = (this.onPointerEnter = new Subject<PointerEventData>());
			}
			return result;
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (this.onPointerExit != null)
			{
				this.onPointerExit.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerExitAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerExit) == null)
			{
				result = (this.onPointerExit = new Subject<PointerEventData>());
			}
			return result;
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.onPointerUp != null)
			{
				this.onPointerUp.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerUpAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerUp) == null)
			{
				result = (this.onPointerUp = new Subject<PointerEventData>());
			}
			return result;
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (this.onSelect != null)
			{
				this.onSelect.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnSelectAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onSelect) == null)
			{
				result = (this.onSelect = new Subject<BaseEventData>());
			}
			return result;
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (this.onPointerClick != null)
			{
				this.onPointerClick.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnPointerClickAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onPointerClick) == null)
			{
				result = (this.onPointerClick = new Subject<PointerEventData>());
			}
			return result;
		}

		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			if (this.onSubmit != null)
			{
				this.onSubmit.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnSubmitAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onSubmit) == null)
			{
				result = (this.onSubmit = new Subject<BaseEventData>());
			}
			return result;
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (this.onDrag != null)
			{
				this.onDrag.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnDragAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onDrag) == null)
			{
				result = (this.onDrag = new Subject<PointerEventData>());
			}
			return result;
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (this.onBeginDrag != null)
			{
				this.onBeginDrag.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnBeginDragAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onBeginDrag) == null)
			{
				result = (this.onBeginDrag = new Subject<PointerEventData>());
			}
			return result;
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (this.onEndDrag != null)
			{
				this.onEndDrag.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnEndDragAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onEndDrag) == null)
			{
				result = (this.onEndDrag = new Subject<PointerEventData>());
			}
			return result;
		}

		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (this.onDrop != null)
			{
				this.onDrop.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnDropAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onDrop) == null)
			{
				result = (this.onDrop = new Subject<PointerEventData>());
			}
			return result;
		}

		void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
		{
			if (this.onUpdateSelected != null)
			{
				this.onUpdateSelected.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnUpdateSelectedAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onUpdateSelected) == null)
			{
				result = (this.onUpdateSelected = new Subject<BaseEventData>());
			}
			return result;
		}

		void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (this.onInitializePotentialDrag != null)
			{
				this.onInitializePotentialDrag.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnInitializePotentialDragAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onInitializePotentialDrag) == null)
			{
				result = (this.onInitializePotentialDrag = new Subject<PointerEventData>());
			}
			return result;
		}

		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			if (this.onCancel != null)
			{
				this.onCancel.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnCancelAsObservable()
		{
			Subject<BaseEventData> result;
			if ((result = this.onCancel) == null)
			{
				result = (this.onCancel = new Subject<BaseEventData>());
			}
			return result;
		}

		void IScrollHandler.OnScroll(PointerEventData eventData)
		{
			if (this.onScroll != null)
			{
				this.onScroll.OnNext(eventData);
			}
		}

		public IObservable<PointerEventData> OnScrollAsObservable()
		{
			Subject<PointerEventData> result;
			if ((result = this.onScroll) == null)
			{
				result = (this.onScroll = new Subject<PointerEventData>());
			}
			return result;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onDeselect != null)
			{
				this.onDeselect.OnCompleted();
			}
			if (this.onMove != null)
			{
				this.onMove.OnCompleted();
			}
			if (this.onPointerDown != null)
			{
				this.onPointerDown.OnCompleted();
			}
			if (this.onPointerEnter != null)
			{
				this.onPointerEnter.OnCompleted();
			}
			if (this.onPointerExit != null)
			{
				this.onPointerExit.OnCompleted();
			}
			if (this.onPointerUp != null)
			{
				this.onPointerUp.OnCompleted();
			}
			if (this.onSelect != null)
			{
				this.onSelect.OnCompleted();
			}
			if (this.onPointerClick != null)
			{
				this.onPointerClick.OnCompleted();
			}
			if (this.onSubmit != null)
			{
				this.onSubmit.OnCompleted();
			}
			if (this.onDrag != null)
			{
				this.onDrag.OnCompleted();
			}
			if (this.onBeginDrag != null)
			{
				this.onBeginDrag.OnCompleted();
			}
			if (this.onEndDrag != null)
			{
				this.onEndDrag.OnCompleted();
			}
			if (this.onDrop != null)
			{
				this.onDrop.OnCompleted();
			}
			if (this.onUpdateSelected != null)
			{
				this.onUpdateSelected.OnCompleted();
			}
			if (this.onInitializePotentialDrag != null)
			{
				this.onInitializePotentialDrag.OnCompleted();
			}
			if (this.onCancel != null)
			{
				this.onCancel.OnCompleted();
			}
			if (this.onScroll != null)
			{
				this.onScroll.OnCompleted();
			}
		}
	}
}
