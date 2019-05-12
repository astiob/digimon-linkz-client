using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System
{
	public class AggregateException : Exception
	{
		public AggregateException(IEnumerable<Exception> innerExceptions)
		{
			this.InnerExceptions = new ReadOnlyCollection<Exception>(innerExceptions.ToList<Exception>());
		}

		public ReadOnlyCollection<Exception> InnerExceptions { get; private set; }

		public AggregateException Flatten()
		{
			List<Exception> list = new List<Exception>();
			foreach (Exception ex in this.InnerExceptions)
			{
				AggregateException ex2 = ex as AggregateException;
				if (ex2 != null)
				{
					list.AddRange(ex2.Flatten().InnerExceptions);
				}
				else
				{
					list.Add(ex);
				}
			}
			return new AggregateException(list);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString());
			foreach (Exception ex in this.InnerExceptions)
			{
				stringBuilder.AppendLine("\n-----------------");
				stringBuilder.AppendLine(ex.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
