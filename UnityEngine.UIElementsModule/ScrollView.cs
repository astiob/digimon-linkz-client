using System;

namespace UnityEngine.Experimental.UIElements
{
	public class ScrollView : VisualElement
	{
		public static readonly Vector2 kDefaultScrollerValues = new Vector2(0f, 100f);

		private VisualElement m_ContentContainer;

		public ScrollView() : this(ScrollView.kDefaultScrollerValues, ScrollView.kDefaultScrollerValues)
		{
		}

		public ScrollView(Vector2 horizontalScrollerValues, Vector2 verticalScrollerValues)
		{
			this.horizontalScrollerValues = horizontalScrollerValues;
			this.verticalScrollerValues = verticalScrollerValues;
			this.contentViewport = new VisualElement
			{
				name = "ContentViewport"
			};
			this.contentViewport.clippingOptions = VisualElement.ClippingOptions.ClipContents;
			base.shadow.Add(this.contentViewport);
			this.m_ContentContainer = new VisualElement
			{
				name = "ContentView"
			};
			this.contentViewport.Add(this.m_ContentContainer);
			this.horizontalScroller = new Scroller(horizontalScrollerValues.x, horizontalScrollerValues.y, delegate(float value)
			{
				this.scrollOffset = new Vector2(value, this.scrollOffset.y);
			}, Slider.Direction.Horizontal)
			{
				name = "HorizontalScroller",
				persistenceKey = "HorizontalScroller"
			};
			base.shadow.Add(this.horizontalScroller);
			this.verticalScroller = new Scroller(verticalScrollerValues.x, verticalScrollerValues.y, delegate(float value)
			{
				this.scrollOffset = new Vector2(this.scrollOffset.x, value);
			}, Slider.Direction.Vertical)
			{
				name = "VerticalScroller",
				persistenceKey = "VerticalScroller"
			};
			base.shadow.Add(this.verticalScroller);
			base.RegisterCallback<WheelEvent>(new EventCallback<WheelEvent>(this.OnScrollWheel), Capture.NoCapture);
		}

		public Vector2 horizontalScrollerValues { get; set; }

		public Vector2 verticalScrollerValues { get; set; }

		public bool showHorizontal { get; set; }

		public bool showVertical { get; set; }

		public bool needsHorizontal
		{
			get
			{
				return this.showHorizontal || this.contentContainer.layout.width - base.layout.width > 0f;
			}
		}

		public bool needsVertical
		{
			get
			{
				return this.showVertical || this.contentContainer.layout.height - base.layout.height > 0f;
			}
		}

		public Vector2 scrollOffset
		{
			get
			{
				return new Vector2(this.horizontalScroller.value, this.verticalScroller.value);
			}
			set
			{
				if (value != this.scrollOffset)
				{
					this.horizontalScroller.value = value.x;
					this.verticalScroller.value = value.y;
					this.UpdateContentViewTransform();
				}
			}
		}

		private float scrollableWidth
		{
			get
			{
				return this.contentContainer.layout.width - this.contentViewport.layout.width;
			}
		}

		private float scrollableHeight
		{
			get
			{
				return this.contentContainer.layout.height - this.contentViewport.layout.height;
			}
		}

		private void UpdateContentViewTransform()
		{
			Vector3 position = this.contentContainer.transform.position;
			Vector2 scrollOffset = this.scrollOffset;
			position.x = -scrollOffset.x;
			position.y = -scrollOffset.y;
			this.contentContainer.transform.position = position;
			base.Dirty(ChangeType.Repaint);
		}

		public VisualElement contentViewport { get; private set; }

		[Obsolete("Please use contentContainer instead", false)]
		public VisualElement contentView
		{
			get
			{
				return this.contentContainer;
			}
		}

		public Scroller horizontalScroller { get; private set; }

		public Scroller verticalScroller { get; private set; }

		public override VisualElement contentContainer
		{
			get
			{
				return this.m_ContentContainer;
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
				if (this.contentContainer.layout.width > Mathf.Epsilon)
				{
					this.horizontalScroller.Adjust(this.contentViewport.layout.width / this.contentContainer.layout.width);
				}
				if (this.contentContainer.layout.height > Mathf.Epsilon)
				{
					this.verticalScroller.Adjust(this.contentViewport.layout.height / this.contentContainer.layout.height);
				}
				this.horizontalScroller.SetEnabled(this.contentContainer.layout.width - base.layout.width > 0f);
				this.verticalScroller.SetEnabled(this.contentContainer.layout.height - base.layout.height > 0f);
				this.contentViewport.style.positionRight = ((!this.needsVertical) ? 0f : this.verticalScroller.layout.width);
				this.horizontalScroller.style.positionRight = ((!this.needsVertical) ? 0f : this.verticalScroller.layout.width);
				this.contentViewport.style.positionBottom = ((!this.needsHorizontal) ? 0f : this.horizontalScroller.layout.height);
				this.verticalScroller.style.positionBottom = ((!this.needsHorizontal) ? 0f : this.horizontalScroller.layout.height);
				if (this.needsHorizontal)
				{
					this.horizontalScroller.lowValue = 0f;
					this.horizontalScroller.highValue = this.scrollableWidth;
				}
				else
				{
					this.horizontalScroller.value = 0f;
				}
				if (this.needsVertical)
				{
					this.verticalScroller.lowValue = 0f;
					this.verticalScroller.highValue = this.scrollableHeight;
				}
				else
				{
					this.verticalScroller.value = 0f;
				}
				if (this.horizontalScroller.visible != this.needsHorizontal)
				{
					this.horizontalScroller.visible = this.needsHorizontal;
					if (this.needsHorizontal)
					{
						this.contentViewport.AddToClassList("HorizontalScroll");
					}
					else
					{
						this.contentViewport.RemoveFromClassList("HorizontalScroll");
					}
				}
				if (this.verticalScroller.visible != this.needsVertical)
				{
					this.verticalScroller.visible = this.needsVertical;
					if (this.needsVertical)
					{
						this.contentViewport.AddToClassList("VerticalScroll");
					}
					else
					{
						this.contentViewport.RemoveFromClassList("VerticalScroll");
					}
				}
				this.UpdateContentViewTransform();
			}
		}

		private void OnScrollWheel(WheelEvent evt)
		{
			if (this.contentContainer.layout.height - base.layout.height > 0f)
			{
				if (evt.delta.y < 0f)
				{
					this.verticalScroller.ScrollPageUp();
				}
				else if (evt.delta.y > 0f)
				{
					this.verticalScroller.ScrollPageDown();
				}
			}
			evt.StopPropagation();
		}
	}
}
