using System;

namespace UnityEngine.Advertisements
{
	public class ShowOptions
	{
		public Action<ShowResult> resultCallback { get; set; }

		public string gamerSid { get; set; }
	}
}
