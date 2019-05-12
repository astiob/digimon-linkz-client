using System;
using System.IO;

namespace com.adjust.sdk
{
	public class JSONData : JSONNode
	{
		private string m_Data;

		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData(string.Empty);
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}
	}
}
