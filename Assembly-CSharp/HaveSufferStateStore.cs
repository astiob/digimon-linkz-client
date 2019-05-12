using System;

public class HaveSufferStateStore
{
	public HaveSufferStateStore.Data[] sufferStatePropertys;

	[Serializable]
	public class Data
	{
		public int key;

		public string value;
	}
}
