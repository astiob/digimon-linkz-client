using System;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapMemberElement : XmlTypeMapMember
	{
		private XmlTypeMapElementInfoList _elementInfo;

		private string _choiceMember;

		private bool _isTextCollector;

		private TypeData _choiceTypeData;

		public XmlTypeMapElementInfoList ElementInfo
		{
			get
			{
				if (this._elementInfo == null)
				{
					this._elementInfo = new XmlTypeMapElementInfoList();
				}
				return this._elementInfo;
			}
			set
			{
				this._elementInfo = value;
			}
		}

		public string ChoiceMember
		{
			get
			{
				return this._choiceMember;
			}
			set
			{
				this._choiceMember = value;
			}
		}

		public TypeData ChoiceTypeData
		{
			get
			{
				return this._choiceTypeData;
			}
			set
			{
				this._choiceTypeData = value;
			}
		}

		public XmlTypeMapElementInfo FindElement(object ob, object memberValue)
		{
			if (this._elementInfo.Count == 1)
			{
				return (XmlTypeMapElementInfo)this._elementInfo[0];
			}
			if (this._choiceMember != null)
			{
				object value = XmlTypeMapMember.GetValue(ob, this._choiceMember);
				foreach (object obj in this._elementInfo)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
					if (xmlTypeMapElementInfo.ChoiceValue != null && xmlTypeMapElementInfo.ChoiceValue.Equals(value))
					{
						return xmlTypeMapElementInfo;
					}
				}
			}
			else
			{
				if (memberValue == null)
				{
					return (XmlTypeMapElementInfo)this._elementInfo[0];
				}
				foreach (object obj2 in this._elementInfo)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo2 = (XmlTypeMapElementInfo)obj2;
					if (xmlTypeMapElementInfo2.TypeData.Type.IsInstanceOfType(memberValue))
					{
						return xmlTypeMapElementInfo2;
					}
				}
			}
			return null;
		}

		public void SetChoice(object ob, object choice)
		{
			XmlTypeMapMember.SetValue(ob, this._choiceMember, choice);
		}

		public bool IsXmlTextCollector
		{
			get
			{
				return this._isTextCollector;
			}
			set
			{
				this._isTextCollector = value;
			}
		}

		public override bool RequiresNullable
		{
			get
			{
				foreach (object obj in this.ElementInfo)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
					if (xmlTypeMapElementInfo.IsNullable)
					{
						return true;
					}
				}
				return false;
			}
		}
	}
}
