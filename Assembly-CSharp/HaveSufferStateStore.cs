using System;

[Serializable]
public class HaveSufferStateStore
{
	public HaveSufferStateStore.Data[] sufferStatePropertys;

	[Serializable]
	public class Data
	{
		public SufferStateProperty.SufferType key;

		public HaveSufferStateStore.DataChild[] values;
	}

	[Serializable]
	public class DataChild
	{
		public string key;

		public SufferStateProperty.Data[] values;
	}
}
