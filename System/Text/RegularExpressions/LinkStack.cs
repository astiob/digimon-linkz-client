using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	internal abstract class LinkStack : LinkRef
	{
		private Stack stack;

		public LinkStack()
		{
			this.stack = new Stack();
		}

		public void Push()
		{
			this.stack.Push(this.GetCurrent());
		}

		public bool Pop()
		{
			if (this.stack.Count > 0)
			{
				this.SetCurrent(this.stack.Pop());
				return true;
			}
			return false;
		}

		protected abstract object GetCurrent();

		protected abstract void SetCurrent(object l);
	}
}
