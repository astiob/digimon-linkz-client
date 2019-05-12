using System;
using System.Diagnostics;

namespace UnityEngine.Experimental.UIElements
{
	public class Slider : VisualElement
	{
		private float m_LowValue;

		private float m_HighValue;

		private Rect m_DragElementStartPos;

		private Slider.SliderValue m_SliderValue;

		private Slider.Direction m_Direction;

		public Slider(float start, float end, Action<float> valueChanged, Slider.Direction direction = Slider.Direction.Horizontal, float pageSize = 10f)
		{
			this.valueChanged = valueChanged;
			this.direction = direction;
			this.pageSize = pageSize;
			this.lowValue = start;
			this.highValue = end;
			base.Add(new VisualElement
			{
				name = "TrackElement"
			});
			this.dragElement = new VisualElement
			{
				name = "DragElement"
			};
			base.Add(this.dragElement);
			this.clampedDragger = new ClampedDragger(this, new Action(this.SetSliderValueFromClick), new Action(this.SetSliderValueFromDrag));
			this.AddManipulator(this.clampedDragger);
		}

		public VisualElement dragElement { get; private set; }

		public float lowValue
		{
			get
			{
				return this.m_LowValue;
			}
			set
			{
				if (!Mathf.Approximately(this.m_LowValue, value))
				{
					this.m_LowValue = value;
					this.ClampValue();
				}
			}
		}

		public float highValue
		{
			get
			{
				return this.m_HighValue;
			}
			set
			{
				if (!Mathf.Approximately(this.m_HighValue, value))
				{
					this.m_HighValue = value;
					this.ClampValue();
				}
			}
		}

		public float range
		{
			get
			{
				return this.highValue - this.lowValue;
			}
		}

		public float pageSize { get; set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<float> valueChanged;

		internal ClampedDragger clampedDragger { get; private set; }

		public float value
		{
			get
			{
				return (this.m_SliderValue != null) ? this.m_SliderValue.m_Value : 0f;
			}
			set
			{
				if (this.m_SliderValue == null)
				{
					this.m_SliderValue = new Slider.SliderValue
					{
						m_Value = this.lowValue
					};
				}
				float num = Mathf.Clamp(value, this.lowValue, this.highValue);
				if (!Mathf.Approximately(this.m_SliderValue.m_Value, num))
				{
					this.m_SliderValue.m_Value = num;
					this.UpdateDragElementPosition();
					if (this.valueChanged != null)
					{
						this.valueChanged(this.m_SliderValue.m_Value);
					}
					base.Dirty(ChangeType.Repaint);
					base.SavePersistentData();
				}
			}
		}

		public Slider.Direction direction
		{
			get
			{
				return this.m_Direction;
			}
			set
			{
				this.m_Direction = value;
				if (this.m_Direction == Slider.Direction.Horizontal)
				{
					base.RemoveFromClassList("vertical");
					base.AddToClassList("horizontal");
				}
				else
				{
					base.RemoveFromClassList("horizontal");
					base.AddToClassList("vertical");
				}
			}
		}

		private void ClampValue()
		{
			this.value = this.value;
		}

		public override void OnPersistentDataReady()
		{
			base.OnPersistentDataReady();
			string fullHierarchicalPersistenceKey = base.GetFullHierarchicalPersistenceKey();
			this.m_SliderValue = base.GetOrCreatePersistentData<Slider.SliderValue>(this.m_SliderValue, fullHierarchicalPersistenceKey);
		}

		private void SetSliderValueFromDrag()
		{
			if (this.clampedDragger.dragDirection == ClampedDragger.DragDirection.Free)
			{
				Vector2 delta = this.clampedDragger.delta;
				if (this.direction == Slider.Direction.Horizontal)
				{
					this.ComputeValueAndDirectionFromDrag(base.layout.width, this.dragElement.style.width, this.m_DragElementStartPos.x + delta.x);
				}
				else
				{
					this.ComputeValueAndDirectionFromDrag(base.layout.height, this.dragElement.style.height, this.m_DragElementStartPos.y + delta.y);
				}
			}
		}

		private void ComputeValueAndDirectionFromDrag(float sliderLength, float dragElementLength, float dragElementPos)
		{
			float num = sliderLength - dragElementLength;
			if (Mathf.Abs(num) >= Mathf.Epsilon)
			{
				this.value = Mathf.Max(0f, Mathf.Min(dragElementPos, num)) / num * this.range + this.lowValue;
			}
		}

		private void SetSliderValueFromClick()
		{
			if (this.clampedDragger.dragDirection != ClampedDragger.DragDirection.Free)
			{
				if (this.clampedDragger.dragDirection == ClampedDragger.DragDirection.None)
				{
					if (this.pageSize == 0f)
					{
						float num = (this.direction != Slider.Direction.Horizontal) ? this.dragElement.style.positionLeft.value : (this.clampedDragger.startMousePosition.x - this.dragElement.style.width / 2f);
						float num2 = (this.direction != Slider.Direction.Horizontal) ? (this.clampedDragger.startMousePosition.y - this.dragElement.style.height / 2f) : this.dragElement.style.positionTop.value;
						this.dragElement.style.positionLeft = num;
						this.dragElement.style.positionTop = num2;
						this.m_DragElementStartPos = new Rect(num, num2, this.dragElement.style.width, this.dragElement.style.height);
						this.clampedDragger.dragDirection = ClampedDragger.DragDirection.Free;
						if (this.direction == Slider.Direction.Horizontal)
						{
							this.ComputeValueAndDirectionFromDrag(base.layout.width, this.dragElement.style.width, this.m_DragElementStartPos.x);
						}
						else
						{
							this.ComputeValueAndDirectionFromDrag(base.layout.height, this.dragElement.style.height, this.m_DragElementStartPos.y);
						}
						return;
					}
					this.m_DragElementStartPos = new Rect(this.dragElement.style.positionLeft, this.dragElement.style.positionTop, this.dragElement.style.width, this.dragElement.style.height);
				}
				if (this.direction == Slider.Direction.Horizontal)
				{
					this.ComputeValueAndDirectionFromClick(base.layout.width, this.dragElement.style.width, this.dragElement.style.positionLeft, this.clampedDragger.lastMousePosition.x);
				}
				else
				{
					this.ComputeValueAndDirectionFromClick(base.layout.height, this.dragElement.style.height, this.dragElement.style.positionTop, this.clampedDragger.lastMousePosition.y);
				}
			}
		}

		private void ComputeValueAndDirectionFromClick(float sliderLength, float dragElementLength, float dragElementPos, float dragElementLastPos)
		{
			float num = sliderLength - dragElementLength;
			if (Mathf.Abs(num) >= Mathf.Epsilon)
			{
				if (dragElementLastPos < dragElementPos && this.clampedDragger.dragDirection != ClampedDragger.DragDirection.LowToHigh)
				{
					this.clampedDragger.dragDirection = ClampedDragger.DragDirection.HighToLow;
					this.value = Mathf.Max(0f, Mathf.Min(dragElementPos - this.pageSize, num)) / num * this.range + this.lowValue;
				}
				else if (dragElementLastPos > dragElementPos + dragElementLength && this.clampedDragger.dragDirection != ClampedDragger.DragDirection.HighToLow)
				{
					this.clampedDragger.dragDirection = ClampedDragger.DragDirection.LowToHigh;
					this.value = Mathf.Max(0f, Mathf.Min(dragElementPos + this.pageSize, num)) / num * this.range + this.lowValue;
				}
			}
		}

		public void AdjustDragElement(float factor)
		{
			bool flag = factor < 1f;
			this.dragElement.visible = flag;
			if (flag)
			{
				IStyle style = this.dragElement.style;
				this.dragElement.visible = true;
				if (this.direction == Slider.Direction.Horizontal)
				{
					float specifiedValueOrDefault = style.minWidth.GetSpecifiedValueOrDefault(0f);
					style.width = Mathf.Max(base.layout.width * factor, specifiedValueOrDefault);
				}
				else
				{
					float specifiedValueOrDefault2 = style.minHeight.GetSpecifiedValueOrDefault(0f);
					style.height = Mathf.Max(base.layout.height * factor, specifiedValueOrDefault2);
				}
			}
		}

		private void UpdateDragElementPosition()
		{
			if (base.panel != null)
			{
				float num = this.value - this.lowValue;
				float num2 = this.dragElement.style.width;
				float num3 = this.dragElement.style.height;
				if (this.direction == Slider.Direction.Horizontal)
				{
					float num4 = base.layout.width - num2;
					this.dragElement.style.positionLeft = num / this.range * num4;
				}
				else
				{
					float num5 = base.layout.height - num3;
					this.dragElement.style.positionTop = num / this.range * num5;
				}
			}
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<PostLayoutEvent>.TypeId())
			{
				PostLayoutEvent postLayoutEvent = (PostLayoutEvent)evt;
				this.OnPostLayout(postLayoutEvent.hasNewLayout);
			}
		}

		private void OnPostLayout(bool hasNewLayout)
		{
			if (hasNewLayout)
			{
				this.UpdateDragElementPosition();
			}
		}

		public enum Direction
		{
			Horizontal,
			Vertical
		}

		[Serializable]
		private class SliderValue
		{
			public float m_Value = 0f;
		}
	}
}
