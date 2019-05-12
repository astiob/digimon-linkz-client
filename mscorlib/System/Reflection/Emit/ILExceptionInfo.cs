using System;

namespace System.Reflection.Emit
{
	internal struct ILExceptionInfo
	{
		private ILExceptionBlock[] handlers;

		internal int start;

		private int len;

		internal Label end;

		internal int NumHandlers()
		{
			return this.handlers.Length;
		}

		internal void AddCatch(Type extype, int offset)
		{
			this.End(offset);
			this.add_block(offset);
			int num = this.handlers.Length - 1;
			this.handlers[num].type = 0;
			this.handlers[num].start = offset;
			this.handlers[num].extype = extype;
		}

		internal void AddFinally(int offset)
		{
			this.End(offset);
			this.add_block(offset);
			int num = this.handlers.Length - 1;
			this.handlers[num].type = 2;
			this.handlers[num].start = offset;
			this.handlers[num].extype = null;
		}

		internal void AddFault(int offset)
		{
			this.End(offset);
			this.add_block(offset);
			int num = this.handlers.Length - 1;
			this.handlers[num].type = 4;
			this.handlers[num].start = offset;
			this.handlers[num].extype = null;
		}

		internal void AddFilter(int offset)
		{
			this.End(offset);
			this.add_block(offset);
			int num = this.handlers.Length - 1;
			this.handlers[num].type = -1;
			this.handlers[num].extype = null;
			this.handlers[num].filter_offset = offset;
		}

		internal void End(int offset)
		{
			if (this.handlers == null)
			{
				return;
			}
			int num = this.handlers.Length - 1;
			if (num >= 0)
			{
				this.handlers[num].len = offset - this.handlers[num].start;
			}
		}

		internal int LastClauseType()
		{
			if (this.handlers != null)
			{
				return this.handlers[this.handlers.Length - 1].type;
			}
			return 0;
		}

		internal void PatchFilterClause(int start)
		{
			if (this.handlers != null && this.handlers.Length > 0)
			{
				this.handlers[this.handlers.Length - 1].start = start;
				this.handlers[this.handlers.Length - 1].type = 1;
			}
		}

		internal void Debug(int b)
		{
		}

		private void add_block(int offset)
		{
			if (this.handlers != null)
			{
				int num = this.handlers.Length;
				ILExceptionBlock[] destinationArray = new ILExceptionBlock[num + 1];
				Array.Copy(this.handlers, destinationArray, num);
				this.handlers = destinationArray;
				this.handlers[num].len = offset - this.handlers[num].start;
			}
			else
			{
				this.handlers = new ILExceptionBlock[1];
				this.len = offset - this.start;
			}
		}
	}
}
