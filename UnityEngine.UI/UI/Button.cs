using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Button", 30)]
	public class Button : Selectable, IEventSystemHandler, IPointerClickHandler, ISubmitHandler
	{
		[FormerlySerializedAs("onClick")]
		[SerializeField]
		private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();

		protected Button()
		{
		}

		public Button.ButtonClickedEvent onClick
		{
			get
			{
				return this.m_OnClick;
			}
			set
			{
				this.m_OnClick = value;
			}
		}

		private void Press()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.m_OnClick.Invoke();
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.DoStateTransition(Selectable.SelectionState.Pressed, false);
			base.StartCoroutine(this.OnFinishSubmit());
		}

		private IEnumerator OnFinishSubmit()
		{
			float fadeTime = base.colors.fadeDuration;
			float elapsedTime = 0f;
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.DoStateTransition(base.currentSelectionState, false);
			yield break;
		}

		[Serializable]
		public class ButtonClickedEvent : UnityEvent
		{
		}
	}
}
