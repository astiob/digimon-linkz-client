using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public class NpChatRequestParameter
	{
		public int ngcheck = 1;

		public string roomId;

		public IList<int> to;

		public List<string> msg = new List<string>();
	}
}
