using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>A 2D Rectangle defined by x, y position and width, height.</para>
	/// </summary>
	public struct Rect
	{
		private float m_XMin;

		private float m_YMin;

		private float m_Width;

		private float m_Height;

		/// <summary>
		///   <para>Creates a new rectangle.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Rect(float x, float y, float width, float height)
		{
			this.m_XMin = x;
			this.m_YMin = y;
			this.m_Width = width;
			this.m_Height = height;
		}

		/// <summary>
		///   <para>Creates a rectangle given a size and position.</para>
		/// </summary>
		/// <param name="position">The position of the top-left corner.</param>
		/// <param name="size">The width and height.</param>
		public Rect(Vector2 position, Vector2 size)
		{
			this.m_XMin = position.x;
			this.m_YMin = position.y;
			this.m_Width = size.x;
			this.m_Height = size.y;
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="source"></param>
		public Rect(Rect source)
		{
			this.m_XMin = source.m_XMin;
			this.m_YMin = source.m_YMin;
			this.m_Width = source.m_Width;
			this.m_Height = source.m_Height;
		}

		/// <summary>
		///   <para>Creates a rectangle from min/max coordinate values.</para>
		/// </summary>
		/// <param name="xmin"></param>
		/// <param name="ymin"></param>
		/// <param name="xmax"></param>
		/// <param name="ymax"></param>
		public static Rect MinMaxRect(float xmin, float ymin, float xmax, float ymax)
		{
			return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
		}

		/// <summary>
		///   <para>Set components of an existing Rect.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Set(float x, float y, float width, float height)
		{
			this.m_XMin = x;
			this.m_YMin = y;
			this.m_Width = width;
			this.m_Height = height;
		}

		/// <summary>
		///   <para>Left coordinate of the rectangle.</para>
		/// </summary>
		public float x
		{
			get
			{
				return this.m_XMin;
			}
			set
			{
				this.m_XMin = value;
			}
		}

		/// <summary>
		///   <para>Top coordinate of the rectangle.</para>
		/// </summary>
		public float y
		{
			get
			{
				return this.m_YMin;
			}
			set
			{
				this.m_YMin = value;
			}
		}

		/// <summary>
		///   <para>The top left coordinates of the rectangle.</para>
		/// </summary>
		public Vector2 position
		{
			get
			{
				return new Vector2(this.m_XMin, this.m_YMin);
			}
			set
			{
				this.m_XMin = value.x;
				this.m_YMin = value.y;
			}
		}

		/// <summary>
		///   <para>Center coordinate of the rectangle.</para>
		/// </summary>
		public Vector2 center
		{
			get
			{
				return new Vector2(this.x + this.m_Width / 2f, this.y + this.m_Height / 2f);
			}
			set
			{
				this.m_XMin = value.x - this.m_Width / 2f;
				this.m_YMin = value.y - this.m_Height / 2f;
			}
		}

		/// <summary>
		///   <para>Lower left corner of the rectangle.</para>
		/// </summary>
		public Vector2 min
		{
			get
			{
				return new Vector2(this.xMin, this.yMin);
			}
			set
			{
				this.xMin = value.x;
				this.yMin = value.y;
			}
		}

		/// <summary>
		///   <para>Upper right corner of the rectangle.</para>
		/// </summary>
		public Vector2 max
		{
			get
			{
				return new Vector2(this.xMax, this.yMax);
			}
			set
			{
				this.xMax = value.x;
				this.yMax = value.y;
			}
		}

		/// <summary>
		///   <para>Width of the rectangle.</para>
		/// </summary>
		public float width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		/// <summary>
		///   <para>Height of the rectangle.</para>
		/// </summary>
		public float height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		/// <summary>
		///   <para>The size of the rectangle.</para>
		/// </summary>
		public Vector2 size
		{
			get
			{
				return new Vector2(this.m_Width, this.m_Height);
			}
			set
			{
				this.m_Width = value.x;
				this.m_Height = value.y;
			}
		}

		[Obsolete("use xMin")]
		public float left
		{
			get
			{
				return this.m_XMin;
			}
		}

		[Obsolete("use xMax")]
		public float right
		{
			get
			{
				return this.m_XMin + this.m_Width;
			}
		}

		[Obsolete("use yMin")]
		public float top
		{
			get
			{
				return this.m_YMin;
			}
		}

		[Obsolete("use yMax")]
		public float bottom
		{
			get
			{
				return this.m_YMin + this.m_Height;
			}
		}

		/// <summary>
		///   <para>Left coordinate of the rectangle.</para>
		/// </summary>
		public float xMin
		{
			get
			{
				return this.m_XMin;
			}
			set
			{
				float xMax = this.xMax;
				this.m_XMin = value;
				this.m_Width = xMax - this.m_XMin;
			}
		}

		/// <summary>
		///   <para>Top coordinate of the rectangle.</para>
		/// </summary>
		public float yMin
		{
			get
			{
				return this.m_YMin;
			}
			set
			{
				float yMax = this.yMax;
				this.m_YMin = value;
				this.m_Height = yMax - this.m_YMin;
			}
		}

		/// <summary>
		///   <para>Right coordinate of the rectangle.</para>
		/// </summary>
		public float xMax
		{
			get
			{
				return this.m_Width + this.m_XMin;
			}
			set
			{
				this.m_Width = value - this.m_XMin;
			}
		}

		/// <summary>
		///   <para>Bottom coordinate of the rectangle.</para>
		/// </summary>
		public float yMax
		{
			get
			{
				return this.m_Height + this.m_YMin;
			}
			set
			{
				this.m_Height = value - this.m_YMin;
			}
		}

		/// <summary>
		///   <para>Returns a nicely formatted string for this Rect.</para>
		/// </summary>
		/// <param name="format"></param>
		public override string ToString()
		{
			return UnityString.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", new object[]
			{
				this.x,
				this.y,
				this.width,
				this.height
			});
		}

		/// <summary>
		///   <para>Returns a nicely formatted string for this Rect.</para>
		/// </summary>
		/// <param name="format"></param>
		public string ToString(string format)
		{
			return UnityString.Format("(x:{0}, y:{1}, width:{2}, height:{3})", new object[]
			{
				this.x.ToString(format),
				this.y.ToString(format),
				this.width.ToString(format),
				this.height.ToString(format)
			});
		}

		/// <summary>
		///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
		/// </summary>
		/// <param name="point">Point to test.</param>
		/// <param name="allowInverse">Does the test allow the Rect's width and height to be negative?</param>
		public bool Contains(Vector2 point)
		{
			return point.x >= this.xMin && point.x < this.xMax && point.y >= this.yMin && point.y < this.yMax;
		}

		/// <summary>
		///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
		/// </summary>
		/// <param name="point">Point to test.</param>
		/// <param name="allowInverse">Does the test allow the Rect's width and height to be negative?</param>
		public bool Contains(Vector3 point)
		{
			return point.x >= this.xMin && point.x < this.xMax && point.y >= this.yMin && point.y < this.yMax;
		}

		/// <summary>
		///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
		/// </summary>
		/// <param name="point">Point to test.</param>
		/// <param name="allowInverse">Does the test allow the Rect's width and height to be negative?</param>
		public bool Contains(Vector3 point, bool allowInverse)
		{
			if (!allowInverse)
			{
				return this.Contains(point);
			}
			bool flag = false;
			if ((this.width < 0f && point.x <= this.xMin && point.x > this.xMax) || (this.width >= 0f && point.x >= this.xMin && point.x < this.xMax))
			{
				flag = true;
			}
			return flag && ((this.height < 0f && point.y <= this.yMin && point.y > this.yMax) || (this.height >= 0f && point.y >= this.yMin && point.y < this.yMax));
		}

		private static Rect OrderMinMax(Rect rect)
		{
			if (rect.xMin > rect.xMax)
			{
				float xMin = rect.xMin;
				rect.xMin = rect.xMax;
				rect.xMax = xMin;
			}
			if (rect.yMin > rect.yMax)
			{
				float yMin = rect.yMin;
				rect.yMin = rect.yMax;
				rect.yMax = yMin;
			}
			return rect;
		}

		/// <summary>
		///   <para>Returns true if the other rectangle overlaps this one. If allowInverse is present and true, the widths and heights of the Rects are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
		/// </summary>
		/// <param name="other">Other rectangle to test overlapping with.</param>
		/// <param name="allowInverse">Does the test allow the Rects' widths and heights to be negative?</param>
		public bool Overlaps(Rect other)
		{
			return other.xMax > this.xMin && other.xMin < this.xMax && other.yMax > this.yMin && other.yMin < this.yMax;
		}

		/// <summary>
		///   <para>Returns true if the other rectangle overlaps this one. If allowInverse is present and true, the widths and heights of the Rects are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
		/// </summary>
		/// <param name="other">Other rectangle to test overlapping with.</param>
		/// <param name="allowInverse">Does the test allow the Rects' widths and heights to be negative?</param>
		public bool Overlaps(Rect other, bool allowInverse)
		{
			Rect rect = this;
			if (allowInverse)
			{
				rect = Rect.OrderMinMax(rect);
				other = Rect.OrderMinMax(other);
			}
			return rect.Overlaps(other);
		}

		/// <summary>
		///   <para>Returns a point inside a rectangle, given normalized coordinates.</para>
		/// </summary>
		/// <param name="rectangle">Rectangle to get a point inside.</param>
		/// <param name="normalizedRectCoordinates">Normalized coordinates to get a point for.</param>
		public static Vector2 NormalizedToPoint(Rect rectangle, Vector2 normalizedRectCoordinates)
		{
			return new Vector2(Mathf.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x), Mathf.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
		}

		/// <summary>
		///   <para>Returns the normalized coordinates cooresponding the the point.</para>
		/// </summary>
		/// <param name="rectangle">Rectangle to get normalized coordinates inside.</param>
		/// <param name="point">A point inside the rectangle to get normalized coordinates for.</param>
		public static Vector2 PointToNormalized(Rect rectangle, Vector2 point)
		{
			return new Vector2(Mathf.InverseLerp(rectangle.x, rectangle.xMax, point.x), Mathf.InverseLerp(rectangle.y, rectangle.yMax, point.y));
		}

		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^ this.height.GetHashCode() >> 1;
		}

		public override bool Equals(object other)
		{
			if (!(other is Rect))
			{
				return false;
			}
			Rect rect = (Rect)other;
			return this.x.Equals(rect.x) && this.y.Equals(rect.y) && this.width.Equals(rect.width) && this.height.Equals(rect.height);
		}

		public static bool operator !=(Rect lhs, Rect rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width || lhs.height != rhs.height;
		}

		public static bool operator ==(Rect lhs, Rect rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
		}
	}
}
