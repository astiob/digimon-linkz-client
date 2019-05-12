using System;
using System.IO;

namespace Mono.Xml.Xsl.Operations
{
	internal abstract class XslOperation
	{
		public const string XsltNamespace = "http://www.w3.org/1999/XSL/Transform";

		public abstract void Evaluate(XslTransformProcessor p);

		public virtual string EvaluateAsString(XslTransformProcessor p)
		{
			StringWriter stringWriter = new StringWriter();
			Outputter outputter = new TextOutputter(stringWriter, true);
			p.PushOutput(outputter);
			this.Evaluate(p);
			p.PopOutput();
			outputter.Done();
			return stringWriter.ToString();
		}
	}
}
