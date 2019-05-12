using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Specification for how to render a character from the font texture. See Font.characterInfo.</para>
	/// </summary>
	public struct CharacterInfo
	{
		/// <summary>
		///   <para>Unicode value of the character.</para>
		/// </summary>
		public int index;

		/// <summary>
		///   <para>UV coordinates for the character in the texture.</para>
		/// </summary>
		[Obsolete("CharacterInfo.uv is deprecated. Use uvBottomLeft, uvBottomRight, uvTopRight or uvTopLeft instead.")]
		public Rect uv;

		/// <summary>
		///   <para>Screen coordinates for the character in generated text meshes.</para>
		/// </summary>
		[Obsolete("CharacterInfo.vert is deprecated. Use minX, maxX, minY, maxY instead.")]
		public Rect vert;

		/// <summary>
		///   <para>How far to advance between the beginning of this charcater and the next.</para>
		/// </summary>
		[Obsolete("CharacterInfo.width is deprecated. Use advance instead.")]
		public float width;

		/// <summary>
		///   <para>The size of the character or 0 if it is the default font size.</para>
		/// </summary>
		public int size;

		/// <summary>
		///   <para>The style of the character.</para>
		/// </summary>
		public FontStyle style;

		/// <summary>
		///   <para>Is the character flipped?</para>
		/// </summary>
		[Obsolete("CharacterInfo.flipped is deprecated. Use uvBottomLeft, uvBottomRight, uvTopRight or uvTopLeft instead, which will be correct regardless of orientation.")]
		public bool flipped;

		private int ascent;

		/// <summary>
		///   <para>The horizontal distance from the origin of this character to the origin of the next character.</para>
		/// </summary>
		public int advance
		{
			get
			{
				return (int)this.width;
			}
			set
			{
				this.width = (float)value;
			}
		}

		/// <summary>
		///   <para>The width of the glyph image.</para>
		/// </summary>
		public int glyphWidth
		{
			get
			{
				return (int)this.vert.width;
			}
			set
			{
				this.vert.width = (float)value;
			}
		}

		/// <summary>
		///   <para>The height of the glyph image.</para>
		/// </summary>
		public int glyphHeight
		{
			get
			{
				return (int)(-(int)this.vert.height);
			}
			set
			{
				float height = this.vert.height;
				this.vert.height = (float)(-(float)value);
				this.vert.y = this.vert.y + (height - this.vert.height);
			}
		}

		/// <summary>
		///   <para>The horizontal distance from the origin of this glyph to the begining of the glyph image.</para>
		/// </summary>
		public int bearing
		{
			get
			{
				return (int)this.vert.x;
			}
			set
			{
				this.vert.x = (float)value;
			}
		}

		/// <summary>
		///   <para>The minimum extend of the glyph image in the y-axis.</para>
		/// </summary>
		public int minY
		{
			get
			{
				return this.ascent + (int)(this.vert.y + this.vert.height);
			}
			set
			{
				this.vert.height = (float)(value - this.ascent) - this.vert.y;
			}
		}

		/// <summary>
		///   <para>The maximum extend of the glyph image in the y-axis.</para>
		/// </summary>
		public int maxY
		{
			get
			{
				return this.ascent + (int)this.vert.y;
			}
			set
			{
				float y = this.vert.y;
				this.vert.y = (float)(value - this.ascent);
				this.vert.height = this.vert.height + (y - this.vert.y);
			}
		}

		/// <summary>
		///   <para>The minium extend of the glyph image in the x-axis.</para>
		/// </summary>
		public int minX
		{
			get
			{
				return (int)this.vert.x;
			}
			set
			{
				float x = this.vert.x;
				this.vert.x = (float)value;
				this.vert.width = this.vert.width + (x - this.vert.x);
			}
		}

		/// <summary>
		///   <para>The maximum extend of the glyph image in the x-axis.</para>
		/// </summary>
		public int maxX
		{
			get
			{
				return (int)(this.vert.x + this.vert.width);
			}
			set
			{
				this.vert.width = (float)value - this.vert.x;
			}
		}

		internal Vector2 uvBottomLeftUnFlipped
		{
			get
			{
				return new Vector2(this.uv.x, this.uv.y);
			}
			set
			{
				Vector2 uvTopRightUnFlipped = this.uvTopRightUnFlipped;
				this.uv.x = value.x;
				this.uv.y = value.y;
				this.uv.width = uvTopRightUnFlipped.x - this.uv.x;
				this.uv.height = uvTopRightUnFlipped.y - this.uv.y;
			}
		}

		internal Vector2 uvBottomRightUnFlipped
		{
			get
			{
				return new Vector2(this.uv.x + this.uv.width, this.uv.y);
			}
			set
			{
				Vector2 uvTopRightUnFlipped = this.uvTopRightUnFlipped;
				this.uv.width = value.x - this.uv.x;
				this.uv.y = value.y;
				this.uv.height = uvTopRightUnFlipped.y - this.uv.y;
			}
		}

		internal Vector2 uvTopRightUnFlipped
		{
			get
			{
				return new Vector2(this.uv.x + this.uv.width, this.uv.y + this.uv.height);
			}
			set
			{
				this.uv.width = value.x - this.uv.x;
				this.uv.height = value.y - this.uv.y;
			}
		}

		internal Vector2 uvTopLeftUnFlipped
		{
			get
			{
				return new Vector2(this.uv.x, this.uv.y + this.uv.height);
			}
			set
			{
				Vector2 uvTopRightUnFlipped = this.uvTopRightUnFlipped;
				this.uv.x = value.x;
				this.uv.height = value.y - this.uv.y;
				this.uv.width = uvTopRightUnFlipped.x - this.uv.x;
			}
		}

		/// <summary>
		///   <para>The uv coordinate matching the bottom left of the glyph image in the font texture.</para>
		/// </summary>
		public Vector2 uvBottomLeft
		{
			get
			{
				return this.uvBottomLeftUnFlipped;
			}
			set
			{
				this.uvBottomLeftUnFlipped = value;
			}
		}

		/// <summary>
		///   <para>The uv coordinate matching the bottom right of the glyph image in the font texture.</para>
		/// </summary>
		public Vector2 uvBottomRight
		{
			get
			{
				return (!this.flipped) ? this.uvBottomRightUnFlipped : this.uvTopLeftUnFlipped;
			}
			set
			{
				if (this.flipped)
				{
					this.uvTopLeftUnFlipped = value;
				}
				else
				{
					this.uvBottomRightUnFlipped = value;
				}
			}
		}

		/// <summary>
		///   <para>The uv coordinate matching the top right of the glyph image in the font texture.</para>
		/// </summary>
		public Vector2 uvTopRight
		{
			get
			{
				return this.uvTopRightUnFlipped;
			}
			set
			{
				this.uvTopRightUnFlipped = value;
			}
		}

		/// <summary>
		///   <para>The uv coordinate matching the top left of the glyph image in the font texture.</para>
		/// </summary>
		public Vector2 uvTopLeft
		{
			get
			{
				return (!this.flipped) ? this.uvTopLeftUnFlipped : this.uvBottomRightUnFlipped;
			}
			set
			{
				if (this.flipped)
				{
					this.uvBottomRightUnFlipped = value;
				}
				else
				{
					this.uvTopLeftUnFlipped = value;
				}
			}
		}
	}
}
