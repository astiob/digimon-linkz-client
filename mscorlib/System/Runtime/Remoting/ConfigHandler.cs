using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Lifetime;

namespace System.Runtime.Remoting
{
	internal class ConfigHandler : SmallXmlParser.IContentHandler
	{
		private ArrayList typeEntries = new ArrayList();

		private ArrayList channelInstances = new ArrayList();

		private ChannelData currentChannel;

		private Stack currentProviderData;

		private string currentClientUrl;

		private string appName;

		private string currentXmlPath = string.Empty;

		private bool onlyDelayedChannels;

		public ConfigHandler(bool onlyDelayedChannels)
		{
			this.onlyDelayedChannels = onlyDelayedChannels;
		}

		private void ValidatePath(string element, params string[] paths)
		{
			foreach (string path in paths)
			{
				if (this.CheckPath(path))
				{
					return;
				}
			}
			throw new RemotingException("Element " + element + " not allowed in this context");
		}

		private bool CheckPath(string path)
		{
			CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
			if (compareInfo.IsPrefix(path, "/", CompareOptions.Ordinal))
			{
				return path == this.currentXmlPath;
			}
			return compareInfo.IsSuffix(this.currentXmlPath, path, CompareOptions.Ordinal);
		}

		public void OnStartParsing(SmallXmlParser parser)
		{
		}

		public void OnProcessingInstruction(string name, string text)
		{
		}

		public void OnIgnorableWhitespace(string s)
		{
		}

		public void OnStartElement(string name, SmallXmlParser.IAttrList attrs)
		{
			try
			{
				if (this.currentXmlPath.StartsWith("/configuration/system.runtime.remoting"))
				{
					this.ParseElement(name, attrs);
				}
				this.currentXmlPath = this.currentXmlPath + "/" + name;
			}
			catch (Exception ex)
			{
				throw new RemotingException("Error in element " + name + ": " + ex.Message, ex);
			}
		}

		public void ParseElement(string name, SmallXmlParser.IAttrList attrs)
		{
			if (this.currentProviderData != null)
			{
				this.ReadCustomProviderData(name, attrs);
				return;
			}
			if (name != null)
			{
				if (ConfigHandler.<>f__switch$map27 == null)
				{
					ConfigHandler.<>f__switch$map27 = new Dictionary<string, int>(19)
					{
						{
							"application",
							0
						},
						{
							"lifetime",
							1
						},
						{
							"channels",
							2
						},
						{
							"channel",
							3
						},
						{
							"serverProviders",
							4
						},
						{
							"clientProviders",
							5
						},
						{
							"provider",
							6
						},
						{
							"formatter",
							6
						},
						{
							"client",
							7
						},
						{
							"service",
							8
						},
						{
							"wellknown",
							9
						},
						{
							"activated",
							10
						},
						{
							"soapInterop",
							11
						},
						{
							"interopXmlType",
							12
						},
						{
							"interopXmlElement",
							13
						},
						{
							"preLoad",
							14
						},
						{
							"debug",
							15
						},
						{
							"channelSinkProviders",
							16
						},
						{
							"customErrors",
							17
						}
					};
				}
				int num;
				if (ConfigHandler.<>f__switch$map27.TryGetValue(name, out num))
				{
					switch (num)
					{
					case 0:
						this.ValidatePath(name, new string[]
						{
							"system.runtime.remoting"
						});
						if (attrs.Names.Length > 0)
						{
							this.appName = attrs.Values[0];
						}
						break;
					case 1:
						this.ValidatePath(name, new string[]
						{
							"application"
						});
						this.ReadLifetine(attrs);
						break;
					case 2:
						this.ValidatePath(name, new string[]
						{
							"system.runtime.remoting",
							"application"
						});
						break;
					case 3:
						this.ValidatePath(name, new string[]
						{
							"channels"
						});
						if (this.currentXmlPath.IndexOf("application") != -1)
						{
							this.ReadChannel(attrs, false);
						}
						else
						{
							this.ReadChannel(attrs, true);
						}
						break;
					case 4:
						this.ValidatePath(name, new string[]
						{
							"channelSinkProviders",
							"channel"
						});
						break;
					case 5:
						this.ValidatePath(name, new string[]
						{
							"channelSinkProviders",
							"channel"
						});
						break;
					case 6:
						if (this.CheckPath("application/channels/channel/serverProviders") || this.CheckPath("channels/channel/serverProviders"))
						{
							ProviderData providerData = this.ReadProvider(name, attrs, false);
							this.currentChannel.ServerProviders.Add(providerData);
						}
						else if (this.CheckPath("application/channels/channel/clientProviders") || this.CheckPath("channels/channel/clientProviders"))
						{
							ProviderData providerData = this.ReadProvider(name, attrs, false);
							this.currentChannel.ClientProviders.Add(providerData);
						}
						else if (this.CheckPath("channelSinkProviders/serverProviders"))
						{
							ProviderData providerData = this.ReadProvider(name, attrs, true);
							RemotingConfiguration.RegisterServerProviderTemplate(providerData);
						}
						else if (this.CheckPath("channelSinkProviders/clientProviders"))
						{
							ProviderData providerData = this.ReadProvider(name, attrs, true);
							RemotingConfiguration.RegisterClientProviderTemplate(providerData);
						}
						else
						{
							this.ValidatePath(name, new string[0]);
						}
						break;
					case 7:
						this.ValidatePath(name, new string[]
						{
							"application"
						});
						this.currentClientUrl = attrs.GetValue("url");
						break;
					case 8:
						this.ValidatePath(name, new string[]
						{
							"application"
						});
						break;
					case 9:
						this.ValidatePath(name, new string[]
						{
							"client",
							"service"
						});
						if (this.CheckPath("client"))
						{
							this.ReadClientWellKnown(attrs);
						}
						else
						{
							this.ReadServiceWellKnown(attrs);
						}
						break;
					case 10:
						this.ValidatePath(name, new string[]
						{
							"client",
							"service"
						});
						if (this.CheckPath("client"))
						{
							this.ReadClientActivated(attrs);
						}
						else
						{
							this.ReadServiceActivated(attrs);
						}
						break;
					case 11:
						this.ValidatePath(name, new string[]
						{
							"application"
						});
						break;
					case 12:
						this.ValidatePath(name, new string[]
						{
							"soapInterop"
						});
						this.ReadInteropXml(attrs, false);
						break;
					case 13:
						this.ValidatePath(name, new string[]
						{
							"soapInterop"
						});
						this.ReadInteropXml(attrs, false);
						break;
					case 14:
						this.ValidatePath(name, new string[]
						{
							"soapInterop"
						});
						this.ReadPreload(attrs);
						break;
					case 15:
						this.ValidatePath(name, new string[]
						{
							"system.runtime.remoting"
						});
						break;
					case 16:
						this.ValidatePath(name, new string[]
						{
							"system.runtime.remoting"
						});
						break;
					case 17:
						this.ValidatePath(name, new string[]
						{
							"system.runtime.remoting"
						});
						RemotingConfiguration.SetCustomErrorsMode(attrs.GetValue("mode"));
						break;
					default:
						goto IL_512;
					}
					return;
				}
			}
			IL_512:
			throw new RemotingException("Element '" + name + "' is not valid in system.remoting.configuration section");
		}

		public void OnEndElement(string name)
		{
			if (this.currentProviderData != null)
			{
				this.currentProviderData.Pop();
				if (this.currentProviderData.Count == 0)
				{
					this.currentProviderData = null;
				}
			}
			this.currentXmlPath = this.currentXmlPath.Substring(0, this.currentXmlPath.Length - name.Length - 1);
		}

		private void ReadCustomProviderData(string name, SmallXmlParser.IAttrList attrs)
		{
			SinkProviderData sinkProviderData = (SinkProviderData)this.currentProviderData.Peek();
			SinkProviderData sinkProviderData2 = new SinkProviderData(name);
			for (int i = 0; i < attrs.Names.Length; i++)
			{
				sinkProviderData2.Properties[attrs.Names[i]] = attrs.GetValue(i);
			}
			sinkProviderData.Children.Add(sinkProviderData2);
			this.currentProviderData.Push(sinkProviderData2);
		}

		private void ReadLifetine(SmallXmlParser.IAttrList attrs)
		{
			int i = 0;
			while (i < attrs.Names.Length)
			{
				string text = attrs.Names[i];
				if (text != null)
				{
					if (ConfigHandler.<>f__switch$map28 == null)
					{
						ConfigHandler.<>f__switch$map28 = new Dictionary<string, int>(4)
						{
							{
								"leaseTime",
								0
							},
							{
								"sponsorshipTimeout",
								1
							},
							{
								"renewOnCallTime",
								2
							},
							{
								"leaseManagerPollTime",
								3
							}
						};
					}
					int num;
					if (ConfigHandler.<>f__switch$map28.TryGetValue(text, out num))
					{
						switch (num)
						{
						case 0:
							LifetimeServices.LeaseTime = this.ParseTime(attrs.GetValue(i));
							break;
						case 1:
							LifetimeServices.SponsorshipTimeout = this.ParseTime(attrs.GetValue(i));
							break;
						case 2:
							LifetimeServices.RenewOnCallTime = this.ParseTime(attrs.GetValue(i));
							break;
						case 3:
							LifetimeServices.LeaseManagerPollTime = this.ParseTime(attrs.GetValue(i));
							break;
						default:
							goto IL_E6;
						}
						i++;
						continue;
					}
				}
				IL_E6:
				throw new RemotingException("Invalid attribute: " + attrs.Names[i]);
			}
		}

		private TimeSpan ParseTime(string s)
		{
			if (s == string.Empty || s == null)
			{
				throw new RemotingException("Invalid time value");
			}
			int num = s.IndexOfAny(new char[]
			{
				'D',
				'H',
				'M',
				'S'
			});
			string text;
			if (num == -1)
			{
				text = "S";
			}
			else
			{
				text = s.Substring(num);
				s = s.Substring(0, num);
			}
			double value;
			try
			{
				value = double.Parse(s);
			}
			catch
			{
				throw new RemotingException("Invalid time value: " + s);
			}
			if (text == "D")
			{
				return TimeSpan.FromDays(value);
			}
			if (text == "H")
			{
				return TimeSpan.FromHours(value);
			}
			if (text == "M")
			{
				return TimeSpan.FromMinutes(value);
			}
			if (text == "S")
			{
				return TimeSpan.FromSeconds(value);
			}
			if (text == "MS")
			{
				return TimeSpan.FromMilliseconds(value);
			}
			throw new RemotingException("Invalid time unit: " + text);
		}

		private void ReadChannel(SmallXmlParser.IAttrList attrs, bool isTemplate)
		{
			ChannelData channelData = new ChannelData();
			for (int i = 0; i < attrs.Names.Length; i++)
			{
				string text = attrs.Names[i];
				string text2 = attrs.Values[i];
				if (text == "ref" && !isTemplate)
				{
					channelData.Ref = text2;
				}
				else if (text == "delayLoadAsClientChannel")
				{
					channelData.DelayLoadAsClientChannel = text2;
				}
				else if (text == "id" && isTemplate)
				{
					channelData.Id = text2;
				}
				else if (text == "type")
				{
					channelData.Type = text2;
				}
				else
				{
					channelData.CustomProperties.Add(text, text2);
				}
			}
			if (isTemplate)
			{
				if (channelData.Id == null)
				{
					throw new RemotingException("id attribute is required");
				}
				if (channelData.Type == null)
				{
					throw new RemotingException("id attribute is required");
				}
				RemotingConfiguration.RegisterChannelTemplate(channelData);
			}
			else
			{
				this.channelInstances.Add(channelData);
			}
			this.currentChannel = channelData;
		}

		private ProviderData ReadProvider(string name, SmallXmlParser.IAttrList attrs, bool isTemplate)
		{
			ProviderData providerData = (!(name == "provider")) ? new FormatterData() : new ProviderData();
			SinkProviderData sinkProviderData = new SinkProviderData("root");
			providerData.CustomData = sinkProviderData.Children;
			this.currentProviderData = new Stack();
			this.currentProviderData.Push(sinkProviderData);
			for (int i = 0; i < attrs.Names.Length; i++)
			{
				string text = attrs.Names[i];
				string text2 = attrs.Values[i];
				if (text == "id" && isTemplate)
				{
					providerData.Id = text2;
				}
				else if (text == "type")
				{
					providerData.Type = text2;
				}
				else if (text == "ref" && !isTemplate)
				{
					providerData.Ref = text2;
				}
				else
				{
					providerData.CustomProperties.Add(text, text2);
				}
			}
			if (providerData.Id == null && isTemplate)
			{
				throw new RemotingException("id attribute is required");
			}
			return providerData;
		}

		private void ReadClientActivated(SmallXmlParser.IAttrList attrs)
		{
			string notNull = this.GetNotNull(attrs, "type");
			string assemblyName = this.ExtractAssembly(ref notNull);
			if (this.currentClientUrl == null || this.currentClientUrl == string.Empty)
			{
				throw new RemotingException("url attribute is required in client element when it contains activated entries");
			}
			this.typeEntries.Add(new ActivatedClientTypeEntry(notNull, assemblyName, this.currentClientUrl));
		}

		private void ReadServiceActivated(SmallXmlParser.IAttrList attrs)
		{
			string notNull = this.GetNotNull(attrs, "type");
			string assemblyName = this.ExtractAssembly(ref notNull);
			this.typeEntries.Add(new ActivatedServiceTypeEntry(notNull, assemblyName));
		}

		private void ReadClientWellKnown(SmallXmlParser.IAttrList attrs)
		{
			string notNull = this.GetNotNull(attrs, "url");
			string notNull2 = this.GetNotNull(attrs, "type");
			string assemblyName = this.ExtractAssembly(ref notNull2);
			this.typeEntries.Add(new WellKnownClientTypeEntry(notNull2, assemblyName, notNull));
		}

		private void ReadServiceWellKnown(SmallXmlParser.IAttrList attrs)
		{
			string notNull = this.GetNotNull(attrs, "objectUri");
			string notNull2 = this.GetNotNull(attrs, "mode");
			string notNull3 = this.GetNotNull(attrs, "type");
			string assemblyName = this.ExtractAssembly(ref notNull3);
			WellKnownObjectMode mode;
			if (notNull2 == "SingleCall")
			{
				mode = WellKnownObjectMode.SingleCall;
			}
			else
			{
				if (!(notNull2 == "Singleton"))
				{
					throw new RemotingException("wellknown object mode '" + notNull2 + "' is invalid");
				}
				mode = WellKnownObjectMode.Singleton;
			}
			this.typeEntries.Add(new WellKnownServiceTypeEntry(notNull3, assemblyName, notNull, mode));
		}

		private void ReadInteropXml(SmallXmlParser.IAttrList attrs, bool isElement)
		{
			Type type = Type.GetType(this.GetNotNull(attrs, "clr"));
			string[] array = this.GetNotNull(attrs, "xml").Split(new char[]
			{
				','
			});
			string text = array[0].Trim();
			string text2 = (array.Length <= 0) ? null : array[1].Trim();
			if (isElement)
			{
				SoapServices.RegisterInteropXmlElement(text, text2, type);
			}
			else
			{
				SoapServices.RegisterInteropXmlType(text, text2, type);
			}
		}

		private void ReadPreload(SmallXmlParser.IAttrList attrs)
		{
			string value = attrs.GetValue("type");
			string value2 = attrs.GetValue("assembly");
			if (value != null && value2 != null)
			{
				throw new RemotingException("Type and assembly attributes cannot be specified together");
			}
			if (value != null)
			{
				SoapServices.PreLoad(Type.GetType(value));
			}
			else
			{
				if (value2 == null)
				{
					throw new RemotingException("Either type or assembly attributes must be specified");
				}
				SoapServices.PreLoad(Assembly.Load(value2));
			}
		}

		private string GetNotNull(SmallXmlParser.IAttrList attrs, string name)
		{
			string value = attrs.GetValue(name);
			if (value == null || value == string.Empty)
			{
				throw new RemotingException(name + " attribute is required");
			}
			return value;
		}

		private string ExtractAssembly(ref string type)
		{
			int num = type.IndexOf(',');
			if (num == -1)
			{
				return string.Empty;
			}
			string result = type.Substring(num + 1).Trim();
			type = type.Substring(0, num).Trim();
			return result;
		}

		public void OnChars(string ch)
		{
		}

		public void OnEndParsing(SmallXmlParser parser)
		{
			RemotingConfiguration.RegisterChannels(this.channelInstances, this.onlyDelayedChannels);
			if (this.appName != null)
			{
				RemotingConfiguration.ApplicationName = this.appName;
			}
			if (!this.onlyDelayedChannels)
			{
				RemotingConfiguration.RegisterTypes(this.typeEntries);
			}
		}
	}
}
