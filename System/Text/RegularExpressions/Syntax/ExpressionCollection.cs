using System;
using System.Collections;

namespace System.Text.RegularExpressions.Syntax
{
	internal class ExpressionCollection : CollectionBase
	{
		public void Add(Expression e)
		{
			base.List.Add(e);
		}

		public Expression this[int i]
		{
			get
			{
				return (Expression)base.List[i];
			}
			set
			{
				base.List[i] = value;
			}
		}

		protected override void OnValidate(object o)
		{
		}
	}
}
