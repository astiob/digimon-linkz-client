using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Resources
{
	internal class Win32VersionResource : Win32Resource
	{
		public string[] WellKnownProperties = new string[]
		{
			"Comments",
			"CompanyName",
			"FileVersion",
			"InternalName",
			"LegalTrademarks",
			"OriginalFilename",
			"ProductName",
			"ProductVersion"
		};

		private long signature;

		private int struct_version;

		private long file_version;

		private long product_version;

		private int file_flags_mask;

		private int file_flags;

		private int file_os;

		private int file_type;

		private int file_subtype;

		private long file_date;

		private int file_lang;

		private int file_codepage;

		private Hashtable properties;

		public Win32VersionResource(int id, int language, bool compilercontext) : base(Win32ResourceType.RT_VERSION, id, language)
		{
			this.signature = (long)((ulong)-17890115);
			this.struct_version = 65536;
			this.file_flags_mask = 63;
			this.file_flags = 0;
			this.file_os = 4;
			this.file_type = 2;
			this.file_subtype = 0;
			this.file_date = 0L;
			this.file_lang = ((!compilercontext) ? 127 : 0);
			this.file_codepage = 1200;
			this.properties = new Hashtable();
			string value = (!compilercontext) ? " " : string.Empty;
			foreach (string key in this.WellKnownProperties)
			{
				this.properties[key] = value;
			}
			this.LegalCopyright = " ";
			this.FileDescription = " ";
		}

		public string Version
		{
			get
			{
				return string.Concat(new object[]
				{
					string.Empty,
					this.file_version >> 48,
					".",
					this.file_version >> 32 & 65535L,
					".",
					this.file_version >> 16 & 65535L,
					".",
					this.file_version & 65535L
				});
			}
			set
			{
				long[] array = new long[4];
				if (value != null)
				{
					string[] array2 = value.Split(new char[]
					{
						'.'
					});
					try
					{
						for (int i = 0; i < array2.Length; i++)
						{
							if (i < array.Length)
							{
								array[i] = (long)int.Parse(array2[i]);
							}
						}
					}
					catch (FormatException)
					{
					}
				}
				this.file_version = (array[0] << 48 | array[1] << 32 | (array[2] << 16) + array[3]);
				this.properties["FileVersion"] = this.Version;
			}
		}

		public virtual string this[string key]
		{
			set
			{
				this.properties[key] = value;
			}
		}

		public virtual string Comments
		{
			get
			{
				return (string)this.properties["Comments"];
			}
			set
			{
				this.properties["Comments"] = ((!(value == string.Empty)) ? value : " ");
			}
		}

		public virtual string CompanyName
		{
			get
			{
				return (string)this.properties["CompanyName"];
			}
			set
			{
				this.properties["CompanyName"] = ((!(value == string.Empty)) ? value : " ");
			}
		}

		public virtual string LegalCopyright
		{
			get
			{
				return (string)this.properties["LegalCopyright"];
			}
			set
			{
				this.properties["LegalCopyright"] = ((!(value == string.Empty)) ? value : " ");
			}
		}

		public virtual string LegalTrademarks
		{
			get
			{
				return (string)this.properties["LegalTrademarks"];
			}
			set
			{
				this.properties["LegalTrademarks"] = ((!(value == string.Empty)) ? value : " ");
			}
		}

		public virtual string OriginalFilename
		{
			get
			{
				return (string)this.properties["OriginalFilename"];
			}
			set
			{
				this.properties["OriginalFilename"] = ((!(value == string.Empty)) ? value : " ");
			}
		}

		public virtual string ProductName
		{
			get
			{
				return (string)this.properties["ProductName"];
			}
			set
			{
				this.properties["ProductName"] = ((!(value == string.Empty)) ? value : " ");
			}
		}

		public virtual string ProductVersion
		{
			get
			{
				return (string)this.properties["ProductVersion"];
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					value = " ";
				}
				long[] array = new long[4];
				string[] array2 = value.Split(new char[]
				{
					'.'
				});
				try
				{
					for (int i = 0; i < array2.Length; i++)
					{
						if (i < array.Length)
						{
							array[i] = (long)int.Parse(array2[i]);
						}
					}
				}
				catch (FormatException)
				{
				}
				this.properties["ProductVersion"] = value;
				this.product_version = (array[0] << 48 | array[1] << 32 | (array[2] << 16) + array[3]);
			}
		}

		public virtual string InternalName
		{
			get
			{
				return (string)this.properties["InternalName"];
			}
			set
			{
				this.properties["InternalName"] = ((!(value == string.Empty)) ? value : " ");
			}
		}

		public virtual string FileDescription
		{
			get
			{
				return (string)this.properties["FileDescription"];
			}
			set
			{
				this.properties["FileDescription"] = ((!(value == string.Empty)) ? value : " ");
			}
		}

		public virtual int FileLanguage
		{
			get
			{
				return this.file_lang;
			}
			set
			{
				this.file_lang = value;
			}
		}

		public virtual string FileVersion
		{
			get
			{
				return (string)this.properties["FileVersion"];
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					value = " ";
				}
				long[] array = new long[4];
				string[] array2 = value.Split(new char[]
				{
					'.'
				});
				try
				{
					for (int i = 0; i < array2.Length; i++)
					{
						if (i < array.Length)
						{
							array[i] = (long)int.Parse(array2[i]);
						}
					}
				}
				catch (FormatException)
				{
				}
				this.properties["FileVersion"] = value;
				this.file_version = (array[0] << 48 | array[1] << 32 | (array[2] << 16) + array[3]);
			}
		}

		private void emit_padding(BinaryWriter w)
		{
			Stream baseStream = w.BaseStream;
			if (baseStream.Position % 4L != 0L)
			{
				w.Write(0);
			}
		}

		private void patch_length(BinaryWriter w, long len_pos)
		{
			Stream baseStream = w.BaseStream;
			long position = baseStream.Position;
			baseStream.Position = len_pos;
			w.Write((short)(position - len_pos));
			baseStream.Position = position;
		}

		public override void WriteTo(Stream ms)
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(ms, Encoding.Unicode))
			{
				binaryWriter.Write(0);
				binaryWriter.Write(52);
				binaryWriter.Write(0);
				binaryWriter.Write("VS_VERSION_INFO".ToCharArray());
				binaryWriter.Write(0);
				this.emit_padding(binaryWriter);
				binaryWriter.Write((uint)this.signature);
				binaryWriter.Write(this.struct_version);
				binaryWriter.Write((int)(this.file_version >> 32));
				binaryWriter.Write((int)(this.file_version & (long)((ulong)-1)));
				binaryWriter.Write((int)(this.product_version >> 32));
				binaryWriter.Write((int)(this.product_version & (long)((ulong)-1)));
				binaryWriter.Write(this.file_flags_mask);
				binaryWriter.Write(this.file_flags);
				binaryWriter.Write(this.file_os);
				binaryWriter.Write(this.file_type);
				binaryWriter.Write(this.file_subtype);
				binaryWriter.Write((int)(this.file_date >> 32));
				binaryWriter.Write((int)(this.file_date & (long)((ulong)-1)));
				this.emit_padding(binaryWriter);
				long position = ms.Position;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				binaryWriter.Write(1);
				binaryWriter.Write("VarFileInfo".ToCharArray());
				binaryWriter.Write(0);
				if (ms.Position % 4L != 0L)
				{
					binaryWriter.Write(0);
				}
				long position2 = ms.Position;
				binaryWriter.Write(0);
				binaryWriter.Write(4);
				binaryWriter.Write(0);
				binaryWriter.Write("Translation".ToCharArray());
				binaryWriter.Write(0);
				if (ms.Position % 4L != 0L)
				{
					binaryWriter.Write(0);
				}
				binaryWriter.Write((short)this.file_lang);
				binaryWriter.Write((short)this.file_codepage);
				this.patch_length(binaryWriter, position2);
				this.patch_length(binaryWriter, position);
				long position3 = ms.Position;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				binaryWriter.Write(1);
				binaryWriter.Write("StringFileInfo".ToCharArray());
				this.emit_padding(binaryWriter);
				long position4 = ms.Position;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				binaryWriter.Write(1);
				binaryWriter.Write(string.Format("{0:x4}{1:x4}", this.file_lang, this.file_codepage).ToCharArray());
				this.emit_padding(binaryWriter);
				foreach (object obj in this.properties.Keys)
				{
					string text = (string)obj;
					string text2 = (string)this.properties[text];
					long position5 = ms.Position;
					binaryWriter.Write(0);
					binaryWriter.Write((short)(text2.ToCharArray().Length + 1));
					binaryWriter.Write(1);
					binaryWriter.Write(text.ToCharArray());
					binaryWriter.Write(0);
					this.emit_padding(binaryWriter);
					binaryWriter.Write(text2.ToCharArray());
					binaryWriter.Write(0);
					this.emit_padding(binaryWriter);
					this.patch_length(binaryWriter, position5);
				}
				this.patch_length(binaryWriter, position4);
				this.patch_length(binaryWriter, position3);
				this.patch_length(binaryWriter, 0L);
			}
		}
	}
}
