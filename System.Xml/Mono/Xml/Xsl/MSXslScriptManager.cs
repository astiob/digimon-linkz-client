using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
	internal class MSXslScriptManager
	{
		private Hashtable scripts = new Hashtable();

		public void AddScript(Compiler c)
		{
			MSXslScriptManager.MSXslScript msxslScript = new MSXslScriptManager.MSXslScript(c.Input, c.Evidence);
			string @namespace = c.Input.GetNamespace(msxslScript.ImplementsPrefix);
			if (@namespace == null)
			{
				throw new XsltCompileException("Specified prefix for msxsl:script was not found: " + msxslScript.ImplementsPrefix, null, c.Input);
			}
			this.scripts.Add(@namespace, msxslScript.Compile(c.Input));
		}

		public object GetExtensionObject(string ns)
		{
			if (!this.scripts.ContainsKey(ns))
			{
				return null;
			}
			return Activator.CreateInstance((Type)this.scripts[ns]);
		}

		private enum ScriptingLanguage
		{
			JScript,
			VisualBasic,
			CSharp
		}

		private class MSXslScript
		{
			private MSXslScriptManager.ScriptingLanguage language;

			private string implementsPrefix;

			private string code;

			private Evidence evidence;

			public MSXslScript(XPathNavigator nav, Evidence evidence)
			{
				this.evidence = evidence;
				this.code = nav.Value;
				if (nav.MoveToFirstAttribute())
				{
					for (;;)
					{
						string localName = nav.LocalName;
						if (localName != null)
						{
							if (MSXslScriptManager.MSXslScript.<>f__switch$map1A == null)
							{
								MSXslScriptManager.MSXslScript.<>f__switch$map1A = new Dictionary<string, int>(2)
								{
									{
										"language",
										0
									},
									{
										"implements-prefix",
										1
									}
								};
							}
							int num;
							if (MSXslScriptManager.MSXslScript.<>f__switch$map1A.TryGetValue(localName, out num))
							{
								if (num != 0)
								{
									if (num == 1)
									{
										this.implementsPrefix = nav.Value;
									}
								}
								else
								{
									string text = nav.Value.ToLower(CultureInfo.InvariantCulture);
									if (text == null)
									{
										break;
									}
									if (MSXslScriptManager.MSXslScript.<>f__switch$map19 == null)
									{
										MSXslScriptManager.MSXslScript.<>f__switch$map19 = new Dictionary<string, int>(6)
										{
											{
												"jscript",
												0
											},
											{
												"javascript",
												0
											},
											{
												"vb",
												1
											},
											{
												"visualbasic",
												1
											},
											{
												"c#",
												2
											},
											{
												"csharp",
												2
											}
										};
									}
									int num2;
									if (!MSXslScriptManager.MSXslScript.<>f__switch$map19.TryGetValue(text, out num2))
									{
										break;
									}
									switch (num2)
									{
									case 0:
										this.language = MSXslScriptManager.ScriptingLanguage.JScript;
										goto IL_154;
									case 1:
										this.language = MSXslScriptManager.ScriptingLanguage.VisualBasic;
										goto IL_154;
									case 2:
										this.language = MSXslScriptManager.ScriptingLanguage.CSharp;
										goto IL_154;
									}
									break;
									IL_154:;
								}
							}
						}
						if (!nav.MoveToNextAttribute())
						{
							goto Block_10;
						}
					}
					throw new XsltException("Invalid scripting language!", null);
					Block_10:
					nav.MoveToParent();
				}
				if (this.implementsPrefix == null)
				{
					throw new XsltException("need implements-prefix attr", null);
				}
			}

			public MSXslScriptManager.ScriptingLanguage Language
			{
				get
				{
					return this.language;
				}
			}

			public string ImplementsPrefix
			{
				get
				{
					return this.implementsPrefix;
				}
			}

			public string Code
			{
				get
				{
					return this.code;
				}
			}

			public object Compile(XPathNavigator node)
			{
				throw new NotImplementedException();
			}
		}
	}
}
