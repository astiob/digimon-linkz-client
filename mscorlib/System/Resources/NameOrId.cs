using System;

namespace System.Resources
{
	internal class NameOrId
	{
		private string name;

		private int id;

		public NameOrId(string name)
		{
			this.name = name;
		}

		public NameOrId(int id)
		{
			this.id = id;
		}

		public bool IsName
		{
			get
			{
				return this.name != null;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public int Id
		{
			get
			{
				return this.id;
			}
		}

		public override string ToString()
		{
			if (this.name != null)
			{
				return "Name(" + this.name + ")";
			}
			return "Id(" + this.id + ")";
		}
	}
}
