using System;

namespace System.Globalization
{
	[Serializable]
	internal sealed class CodePageDataItem
	{
		private string m_bodyName;

		private int m_codePage;

		private int m_dataIndex;

		private string m_description;

		private uint m_flags;

		private string m_headerName;

		private int m_uiFamilyCodePage;

		private string m_webName;

		private CodePageDataItem()
		{
		}
	}
}
