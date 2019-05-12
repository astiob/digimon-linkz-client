using System;
using System.Diagnostics;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEngine.Experimental.UIElements
{
	public class Scroller : VisualElement
	{
		public Scroller(float lowValue, float highValue, Action<float> valueChanged, Slider.Direction direction = Slider.Direction.Vertical)
		{
			this.direction = direction;
			this.valueChanged = valueChanged;
			this.slider = new Slider(lowValue, highValue, new Action<float>(this.OnSliderValueChange), direction, 10f)
			{
				name = "Slider",
				persistenceKey = "Slider"
			};
			base.Add(this.slider);
			this.lowButton = new ScrollerButton(new Action(this.ScrollPageUp), 250L, 30L)
			{
				name = "LowButton"
			};
			base.Add(this.lowButton);
			this.highButton = new ScrollerButton(new Action(this.ScrollPageDown), 250L, 30L)
			{
				name = "HighButton"
			};
			base.Add(this.highButton);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<float> valueChanged;

		public Slider slider { get; private set; }

		public ScrollerButton lowButton { get; private set; }

		public ScrollerButton highButton { get; private set; }

		public float value
		{
			get
			{
				return this.slider.value;
			}
			set
			{
				this.slider.value = value;
			}
		}

		public float lowValue
		{
			get
			{
				return this.slider.lowValue;
			}
			set
			{
				this.slider.lowValue = value;
			}
		}

		public float highValue
		{
			get
			{
				return this.slider.highValue;
			}
			set
			{
				this.slider.highValue = value;
			}
		}

		public Slider.Direction direction
		{
			get
			{
				return (base.style.flexDirection != FlexDirection.Row) ? Slider.Direction.Vertical : Slider.Direction.Horizontal;
			}
			set
			{
				if (value == Slider.Direction.Horizontal)
				{
					base.style.flexDirection = FlexDirection.Row;
					base.AddToClassList("horizontal");
				}
				else
				{
					base.style.flexDirection = FlexDirection.Column;
					base.AddToClassList("vertical");
				}
			}
		}

		public void Adjust(float factor)
		{
			this.SetEnabled(factor < 1f);
			this.slider.AdjustDragElement(factor);
		}

		private void OnSliderValueChange(float newValue)
		{
			this.value = newValue;
			if (this.valueChanged != null)
			{
				this.valueChanged(this.slider.value);
			}
			base.Dirty(ChangeType.Repaint);
		}

		public void ScrollPageUp()
		{
			this.value -= this.slider.pageSize * ((this.slider.lowValue >= this.slider.highValue) ? -1f : 1f);
		}

		public void ScrollPageDown()
		{
			this.value += this.slider.pageSize * ((this.slider.lowValue >= this.slider.highValue) ? -1f : 1f);
		}
	}
}
