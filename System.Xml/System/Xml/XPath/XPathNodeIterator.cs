using System;
using System.Collections;

namespace System.Xml.XPath
{
	/// <summary>Provides an iterator over a selected set of nodes.</summary>
	public abstract class XPathNodeIterator : IEnumerable, ICloneable
	{
		private int _count = -1;

		/// <summary>For a description of this member, see <see cref="M:System.Xml.XPath.XPathNodeIterator.Clone" />.</summary>
		/// <returns>An <see cref="T:System.Object" />.</returns>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>Gets the index of the last node in the selected set of nodes.</summary>
		/// <returns>The int index of the last node in the selected set of nodes, or 0 if there are no selected nodes.</returns>
		public virtual int Count
		{
			get
			{
				if (this._count == -1)
				{
					XPathNodeIterator xpathNodeIterator = this.Clone();
					while (xpathNodeIterator.MoveNext())
					{
					}
					this._count = xpathNodeIterator.CurrentPosition;
				}
				return this._count;
			}
		}

		/// <summary>When overridden in a derived class, returns the <see cref="T:System.Xml.XPath.XPathNavigator" /> object for this <see cref="T:System.Xml.XPath.XPathNodeIterator" />, positioned on the current context node.</summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNavigator" /> object positioned on the context node from which the node set was selected. The <see cref="M:System.Xml.XPath.XPathNodeIterator.MoveNext" /> method must be called to move the <see cref="T:System.Xml.XPath.XPathNodeIterator" /> to the first node in the selected set.</returns>
		public abstract XPathNavigator Current { get; }

		/// <summary>When overridden in a derived class, gets the index of the current position in the selected set of nodes.</summary>
		/// <returns>The int index of the current position.</returns>
		public abstract int CurrentPosition { get; }

		/// <summary>When overridden in a derived class, returns a clone of this <see cref="T:System.Xml.XPath.XPathNodeIterator" /> object.</summary>
		/// <returns>A new <see cref="T:System.Xml.XPath.XPathNodeIterator" /> object clone of this <see cref="T:System.Xml.XPath.XPathNodeIterator" /> object.</returns>
		public abstract XPathNodeIterator Clone();

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> object to iterate through the selected node set.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object to iterate through the selected node set.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			while (this.MoveNext())
			{
				XPathNavigator xpathNavigator = this.Current;
				yield return xpathNavigator;
			}
			yield break;
		}

		/// <summary>When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator" /> object returned by the <see cref="P:System.Xml.XPath.XPathNodeIterator.Current" /> property to the next node in the selected node set.</summary>
		/// <returns>true if the <see cref="T:System.Xml.XPath.XPathNavigator" /> object moved to the next node; false if there are no more selected nodes.</returns>
		public abstract bool MoveNext();
	}
}
