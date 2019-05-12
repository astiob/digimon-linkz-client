using System;

namespace UnityEngine.Experimental.UIElements
{
	public class Image : VisualElement
	{
		public Image()
		{
			this.scaleMode = ScaleMode.ScaleAndCrop;
		}

		public Texture image { get; set; }

		public ScaleMode scaleMode { get; set; }

		protected internal override Vector2 DoMeasure(float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
		{
			float num = float.NaN;
			float num2 = float.NaN;
			Vector2 result;
			if (this.image == null)
			{
				result = new Vector2(num, num2);
			}
			else
			{
				num = (float)this.image.width;
				num2 = (float)this.image.height;
				if (widthMode == VisualElement.MeasureMode.AtMost)
				{
					num = Mathf.Min(num, width);
				}
				if (heightMode == VisualElement.MeasureMode.AtMost)
				{
					num2 = Mathf.Min(num2, height);
				}
				result = new Vector2(num, num2);
			}
			return result;
		}

		internal override void DoRepaint(IStylePainter painter)
		{
			if (this.image == null)
			{
				Debug.LogWarning("null texture passed to GUI.DrawTexture");
			}
			else
			{
				TextureStylePainterParameters painterParams = new TextureStylePainterParameters
				{
					layout = base.contentRect,
					texture = this.image,
					color = GUI.color,
					scaleMode = this.scaleMode
				};
				painter.DrawTexture(painterParams);
			}
		}
	}
}
