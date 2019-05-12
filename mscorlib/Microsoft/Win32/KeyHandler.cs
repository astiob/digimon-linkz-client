using System;
using System.Collections;
using System.IO;
using System.Security;
using System.Threading;

namespace Microsoft.Win32
{
	internal class KeyHandler
	{
		private static Hashtable key_to_handler = new Hashtable();

		private static Hashtable dir_to_handler = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());

		public string Dir;

		private Hashtable values;

		private string file;

		private bool dirty;

		private KeyHandler(RegistryKey rkey, string basedir)
		{
			if (!Directory.Exists(basedir))
			{
				try
				{
					Directory.CreateDirectory(basedir);
				}
				catch (UnauthorizedAccessException)
				{
					throw new SecurityException("No access to the given key");
				}
			}
			this.Dir = basedir;
			this.file = Path.Combine(this.Dir, "values.xml");
			this.Load();
		}

		public void Load()
		{
			this.values = new Hashtable();
			if (!File.Exists(this.file))
			{
				return;
			}
			try
			{
				using (FileStream fileStream = File.OpenRead(this.file))
				{
					StreamReader streamReader = new StreamReader(fileStream);
					string text = streamReader.ReadToEnd();
					if (text.Length != 0)
					{
						SecurityElement securityElement = SecurityElement.FromString(text);
						if (securityElement.Tag == "values" && securityElement.Children != null)
						{
							foreach (object obj in securityElement.Children)
							{
								SecurityElement securityElement2 = (SecurityElement)obj;
								if (securityElement2.Tag == "value")
								{
									this.LoadKey(securityElement2);
								}
							}
						}
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
				this.values.Clear();
				throw new SecurityException("No access to the given key");
			}
			catch (Exception arg)
			{
				Console.Error.WriteLine("While loading registry key at {0}: {1}", this.file, arg);
				this.values.Clear();
			}
		}

		private void LoadKey(SecurityElement se)
		{
			Hashtable attributes = se.Attributes;
			try
			{
				string text = (string)attributes["name"];
				if (text != null)
				{
					string text2 = (string)attributes["type"];
					if (text2 != null)
					{
						string text3 = text2;
						switch (text3)
						{
						case "int":
							this.values[text] = int.Parse(se.Text);
							break;
						case "bytearray":
							this.values[text] = Convert.FromBase64String(se.Text);
							break;
						case "string":
							this.values[text] = se.Text;
							break;
						case "expand":
							this.values[text] = new ExpandString(se.Text);
							break;
						case "qword":
							this.values[text] = long.Parse(se.Text);
							break;
						case "string-array":
						{
							ArrayList arrayList = new ArrayList();
							if (se.Children != null)
							{
								foreach (object obj in se.Children)
								{
									SecurityElement securityElement = (SecurityElement)obj;
									arrayList.Add(securityElement.Text);
								}
							}
							this.values[text] = arrayList.ToArray(typeof(string));
							break;
						}
						}
					}
				}
			}
			catch
			{
			}
		}

		public RegistryKey Ensure(RegistryKey rkey, string extra, bool writable)
		{
			Type typeFromHandle = typeof(KeyHandler);
			RegistryKey result;
			lock (typeFromHandle)
			{
				string text = Path.Combine(this.Dir, extra);
				KeyHandler keyHandler = (KeyHandler)KeyHandler.dir_to_handler[text];
				if (keyHandler == null)
				{
					keyHandler = new KeyHandler(rkey, text);
				}
				RegistryKey registryKey = new RegistryKey(keyHandler, KeyHandler.CombineName(rkey, extra), writable);
				KeyHandler.key_to_handler[registryKey] = keyHandler;
				KeyHandler.dir_to_handler[text] = keyHandler;
				result = registryKey;
			}
			return result;
		}

		public RegistryKey Probe(RegistryKey rkey, string extra, bool writable)
		{
			RegistryKey registryKey = null;
			Type typeFromHandle = typeof(KeyHandler);
			RegistryKey result;
			lock (typeFromHandle)
			{
				string text = Path.Combine(this.Dir, extra);
				KeyHandler keyHandler = (KeyHandler)KeyHandler.dir_to_handler[text];
				if (keyHandler != null)
				{
					registryKey = new RegistryKey(keyHandler, KeyHandler.CombineName(rkey, extra), writable);
					KeyHandler.key_to_handler[registryKey] = keyHandler;
				}
				else if (Directory.Exists(text))
				{
					keyHandler = new KeyHandler(rkey, text);
					registryKey = new RegistryKey(keyHandler, KeyHandler.CombineName(rkey, extra), writable);
					KeyHandler.dir_to_handler[text] = keyHandler;
					KeyHandler.key_to_handler[registryKey] = keyHandler;
				}
				result = registryKey;
			}
			return result;
		}

		private static string CombineName(RegistryKey rkey, string extra)
		{
			if (extra.IndexOf('/') != -1)
			{
				extra = extra.Replace('/', '\\');
			}
			return rkey.Name + "\\" + extra;
		}

		public static KeyHandler Lookup(RegistryKey rkey, bool createNonExisting)
		{
			Type typeFromHandle = typeof(KeyHandler);
			KeyHandler result;
			lock (typeFromHandle)
			{
				KeyHandler keyHandler = (KeyHandler)KeyHandler.key_to_handler[rkey];
				if (keyHandler != null)
				{
					result = keyHandler;
				}
				else if (!rkey.IsRoot || !createNonExisting)
				{
					result = null;
				}
				else
				{
					RegistryHive hive = rkey.Hive;
					RegistryHive registryHive = hive;
					switch (registryHive + -2147483648)
					{
					case (RegistryHive)0:
					case (RegistryHive)2:
					case (RegistryHive)3:
					case (RegistryHive)4:
					case (RegistryHive)5:
					case (RegistryHive)6:
					{
						string text = Path.Combine(KeyHandler.MachineStore, hive.ToString());
						keyHandler = new KeyHandler(rkey, text);
						KeyHandler.dir_to_handler[text] = keyHandler;
						break;
					}
					case (RegistryHive)1:
					{
						string text2 = Path.Combine(KeyHandler.UserStore, hive.ToString());
						keyHandler = new KeyHandler(rkey, text2);
						KeyHandler.dir_to_handler[text2] = keyHandler;
						break;
					}
					default:
						throw new Exception("Unknown RegistryHive");
					}
					KeyHandler.key_to_handler[rkey] = keyHandler;
					result = keyHandler;
				}
			}
			return result;
		}

		public static void Drop(RegistryKey rkey)
		{
			Type typeFromHandle = typeof(KeyHandler);
			lock (typeFromHandle)
			{
				KeyHandler keyHandler = (KeyHandler)KeyHandler.key_to_handler[rkey];
				if (keyHandler != null)
				{
					KeyHandler.key_to_handler.Remove(rkey);
					int num = 0;
					foreach (object obj in KeyHandler.key_to_handler)
					{
						if (((DictionaryEntry)obj).Value == keyHandler)
						{
							num++;
						}
					}
					if (num == 0)
					{
						KeyHandler.dir_to_handler.Remove(keyHandler.Dir);
					}
				}
			}
		}

		public static void Drop(string dir)
		{
			Type typeFromHandle = typeof(KeyHandler);
			lock (typeFromHandle)
			{
				KeyHandler keyHandler = (KeyHandler)KeyHandler.dir_to_handler[dir];
				if (keyHandler != null)
				{
					KeyHandler.dir_to_handler.Remove(dir);
					ArrayList arrayList = new ArrayList();
					foreach (object obj in KeyHandler.key_to_handler)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (dictionaryEntry.Value == keyHandler)
						{
							arrayList.Add(dictionaryEntry.Key);
						}
					}
					foreach (object key in arrayList)
					{
						KeyHandler.key_to_handler.Remove(key);
					}
				}
			}
		}

		public object GetValue(string name, RegistryValueOptions options)
		{
			if (this.IsMarkedForDeletion)
			{
				return null;
			}
			if (name == null)
			{
				name = string.Empty;
			}
			object obj = this.values[name];
			ExpandString expandString = obj as ExpandString;
			if (expandString == null)
			{
				return obj;
			}
			if ((options & RegistryValueOptions.DoNotExpandEnvironmentNames) == RegistryValueOptions.None)
			{
				return expandString.Expand();
			}
			return expandString.ToString();
		}

		public void SetValue(string name, object value)
		{
			this.AssertNotMarkedForDeletion();
			if (name == null)
			{
				name = string.Empty;
			}
			if (value is int || value is string || value is byte[] || value is string[])
			{
				this.values[name] = value;
			}
			else
			{
				this.values[name] = value.ToString();
			}
			this.SetDirty();
		}

		public string[] GetValueNames()
		{
			this.AssertNotMarkedForDeletion();
			ICollection keys = this.values.Keys;
			string[] array = new string[keys.Count];
			keys.CopyTo(array, 0);
			return array;
		}

		public void SetValue(string name, object value, RegistryValueKind valueKind)
		{
			this.SetDirty();
			if (name == null)
			{
				name = string.Empty;
			}
			switch (valueKind)
			{
			case RegistryValueKind.String:
				if (value is string)
				{
					this.values[name] = value;
					return;
				}
				goto IL_186;
			case RegistryValueKind.ExpandString:
				if (value is string)
				{
					this.values[name] = new ExpandString((string)value);
					return;
				}
				goto IL_186;
			case RegistryValueKind.Binary:
				if (value is byte[])
				{
					this.values[name] = value;
					return;
				}
				goto IL_186;
			case RegistryValueKind.DWord:
				if (value is long && (long)value < 2147483647L && (long)value > -2147483648L)
				{
					this.values[name] = (int)((long)value);
					return;
				}
				if (value is int)
				{
					this.values[name] = value;
					return;
				}
				goto IL_186;
			case RegistryValueKind.MultiString:
				if (value is string[])
				{
					this.values[name] = value;
					return;
				}
				goto IL_186;
			case RegistryValueKind.QWord:
				if (value is int)
				{
					this.values[name] = (long)((int)value);
					return;
				}
				if (value is long)
				{
					this.values[name] = value;
					return;
				}
				goto IL_186;
			}
			throw new ArgumentException("unknown value", "valueKind");
			IL_186:
			throw new ArgumentException("Value could not be converted to specified type", "valueKind");
		}

		private void SetDirty()
		{
			Type typeFromHandle = typeof(KeyHandler);
			lock (typeFromHandle)
			{
				if (!this.dirty)
				{
					this.dirty = true;
					new Timer(new TimerCallback(this.DirtyTimeout), null, 3000, -1);
				}
			}
		}

		public void DirtyTimeout(object state)
		{
			this.Flush();
		}

		public void Flush()
		{
			Type typeFromHandle = typeof(KeyHandler);
			lock (typeFromHandle)
			{
				if (this.dirty)
				{
					this.Save();
					this.dirty = false;
				}
			}
		}

		public bool ValueExists(string name)
		{
			if (name == null)
			{
				name = string.Empty;
			}
			return this.values.Contains(name);
		}

		public int ValueCount
		{
			get
			{
				return this.values.Keys.Count;
			}
		}

		public bool IsMarkedForDeletion
		{
			get
			{
				return !KeyHandler.dir_to_handler.Contains(this.Dir);
			}
		}

		public void RemoveValue(string name)
		{
			this.AssertNotMarkedForDeletion();
			this.values.Remove(name);
			this.SetDirty();
		}

		~KeyHandler()
		{
			this.Flush();
		}

		private void Save()
		{
			if (this.IsMarkedForDeletion)
			{
				return;
			}
			if (!File.Exists(this.file) && this.values.Count == 0)
			{
				return;
			}
			SecurityElement securityElement = new SecurityElement("values");
			foreach (object obj in this.values)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				object value = dictionaryEntry.Value;
				SecurityElement securityElement2 = new SecurityElement("value");
				securityElement2.AddAttribute("name", SecurityElement.Escape((string)dictionaryEntry.Key));
				if (value is string)
				{
					securityElement2.AddAttribute("type", "string");
					securityElement2.Text = SecurityElement.Escape((string)value);
				}
				else if (value is int)
				{
					securityElement2.AddAttribute("type", "int");
					securityElement2.Text = value.ToString();
				}
				else if (value is long)
				{
					securityElement2.AddAttribute("type", "qword");
					securityElement2.Text = value.ToString();
				}
				else if (value is byte[])
				{
					securityElement2.AddAttribute("type", "bytearray");
					securityElement2.Text = Convert.ToBase64String((byte[])value);
				}
				else if (value is ExpandString)
				{
					securityElement2.AddAttribute("type", "expand");
					securityElement2.Text = SecurityElement.Escape(value.ToString());
				}
				else if (value is string[])
				{
					securityElement2.AddAttribute("type", "string-array");
					foreach (string str in (string[])value)
					{
						securityElement2.AddChild(new SecurityElement("string")
						{
							Text = SecurityElement.Escape(str)
						});
					}
				}
				securityElement.AddChild(securityElement2);
			}
			using (FileStream fileStream = File.Create(this.file))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(securityElement.ToString());
				streamWriter.Flush();
			}
		}

		private void AssertNotMarkedForDeletion()
		{
			if (this.IsMarkedForDeletion)
			{
				throw RegistryKey.CreateMarkedForDeletionException();
			}
		}

		private static string UserStore
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".mono/registry");
			}
		}

		private static string MachineStore
		{
			get
			{
				string text = Environment.GetEnvironmentVariable("MONO_REGISTRY_PATH");
				if (text != null)
				{
					return text;
				}
				text = Environment.GetMachineConfigPath();
				int num = text.IndexOf("machine.config");
				return Path.Combine(Path.Combine(text.Substring(0, num - 1), ".."), "registry");
			}
		}
	}
}
