using System;
using System.Collections;

namespace Mono.Xml
{
	internal class DTDAutomataFactory
	{
		private DTDObjectModel root;

		private Hashtable choiceTable = new Hashtable();

		private Hashtable sequenceTable = new Hashtable();

		public DTDAutomataFactory(DTDObjectModel root)
		{
			this.root = root;
		}

		public DTDChoiceAutomata Choice(DTDAutomata left, DTDAutomata right)
		{
			Hashtable hashtable = this.choiceTable[left] as Hashtable;
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				this.choiceTable[left] = hashtable;
			}
			DTDChoiceAutomata dtdchoiceAutomata = hashtable[right] as DTDChoiceAutomata;
			if (dtdchoiceAutomata == null)
			{
				dtdchoiceAutomata = new DTDChoiceAutomata(this.root, left, right);
				hashtable[right] = dtdchoiceAutomata;
			}
			return dtdchoiceAutomata;
		}

		public DTDSequenceAutomata Sequence(DTDAutomata left, DTDAutomata right)
		{
			Hashtable hashtable = this.sequenceTable[left] as Hashtable;
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				this.sequenceTable[left] = hashtable;
			}
			DTDSequenceAutomata dtdsequenceAutomata = hashtable[right] as DTDSequenceAutomata;
			if (dtdsequenceAutomata == null)
			{
				dtdsequenceAutomata = new DTDSequenceAutomata(this.root, left, right);
				hashtable[right] = dtdsequenceAutomata;
			}
			return dtdsequenceAutomata;
		}
	}
}
