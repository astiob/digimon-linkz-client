using System;

namespace Mono.Xml
{
	internal class DTDContentModel : DTDNode
	{
		private DTDObjectModel root;

		private DTDAutomata compiledAutomata;

		private string ownerElementName;

		private string elementName;

		private DTDContentOrderType orderType;

		private DTDContentModelCollection childModels = new DTDContentModelCollection();

		private DTDOccurence occurence;

		internal DTDContentModel(DTDObjectModel root, string ownerElementName)
		{
			this.root = root;
			this.ownerElementName = ownerElementName;
		}

		public DTDContentModelCollection ChildModels
		{
			get
			{
				return this.childModels;
			}
			set
			{
				this.childModels = value;
			}
		}

		public DTDElementDeclaration ElementDecl
		{
			get
			{
				return this.root.ElementDecls[this.ownerElementName];
			}
		}

		public string ElementName
		{
			get
			{
				return this.elementName;
			}
			set
			{
				this.elementName = value;
			}
		}

		public DTDOccurence Occurence
		{
			get
			{
				return this.occurence;
			}
			set
			{
				this.occurence = value;
			}
		}

		public DTDContentOrderType OrderType
		{
			get
			{
				return this.orderType;
			}
			set
			{
				this.orderType = value;
			}
		}

		public DTDAutomata GetAutomata()
		{
			if (this.compiledAutomata == null)
			{
				this.Compile();
			}
			return this.compiledAutomata;
		}

		public DTDAutomata Compile()
		{
			this.compiledAutomata = this.CompileInternal();
			return this.compiledAutomata;
		}

		private DTDAutomata CompileInternal()
		{
			if (this.ElementDecl.IsAny)
			{
				return this.root.Any;
			}
			if (this.ElementDecl.IsEmpty)
			{
				return this.root.Empty;
			}
			DTDAutomata basicContentAutomata = this.GetBasicContentAutomata();
			switch (this.Occurence)
			{
			case DTDOccurence.One:
				return basicContentAutomata;
			case DTDOccurence.Optional:
				return this.Choice(this.root.Empty, basicContentAutomata);
			case DTDOccurence.ZeroOrMore:
				return this.Choice(this.root.Empty, new DTDOneOrMoreAutomata(this.root, basicContentAutomata));
			case DTDOccurence.OneOrMore:
				return new DTDOneOrMoreAutomata(this.root, basicContentAutomata);
			default:
				throw new InvalidOperationException();
			}
		}

		private DTDAutomata GetBasicContentAutomata()
		{
			if (this.ElementName != null)
			{
				return new DTDElementAutomata(this.root, this.ElementName);
			}
			int count = this.ChildModels.Count;
			if (count == 0)
			{
				return this.root.Empty;
			}
			if (count == 1)
			{
				return this.ChildModels[0].GetAutomata();
			}
			int count2 = this.ChildModels.Count;
			DTDContentOrderType dtdcontentOrderType = this.OrderType;
			DTDAutomata dtdautomata;
			if (dtdcontentOrderType == DTDContentOrderType.Seq)
			{
				dtdautomata = this.Sequence(this.ChildModels[count2 - 2].GetAutomata(), this.ChildModels[count2 - 1].GetAutomata());
				for (int i = count2 - 2; i > 0; i--)
				{
					dtdautomata = this.Sequence(this.ChildModels[i - 1].GetAutomata(), dtdautomata);
				}
				return dtdautomata;
			}
			if (dtdcontentOrderType != DTDContentOrderType.Or)
			{
				throw new InvalidOperationException("Invalid pattern specification");
			}
			dtdautomata = this.Choice(this.ChildModels[count2 - 2].GetAutomata(), this.ChildModels[count2 - 1].GetAutomata());
			for (int j = count2 - 2; j > 0; j--)
			{
				dtdautomata = this.Choice(this.ChildModels[j - 1].GetAutomata(), dtdautomata);
			}
			return dtdautomata;
		}

		private DTDAutomata Sequence(DTDAutomata l, DTDAutomata r)
		{
			return this.root.Factory.Sequence(l, r);
		}

		private DTDAutomata Choice(DTDAutomata l, DTDAutomata r)
		{
			return l.MakeChoice(r);
		}
	}
}
