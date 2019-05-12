using System;
using System.Collections.Generic;

namespace Firebase
{
	internal static class VariantExtension
	{
		private const VariantExtension.KeyOptions DefaultKeyOptions = VariantExtension.KeyOptions.UseObjectKeys;

		public static object ToObject(this Variant variant)
		{
			return variant.ToObject(VariantExtension.KeyOptions.UseObjectKeys);
		}

		public static object ToObject(this Variant variant, VariantExtension.KeyOptions options)
		{
			switch (variant.type())
			{
			case Variant.Type.Int64:
				return variant.int64_value();
			case Variant.Type.Double:
				return variant.double_value();
			case Variant.Type.Bool:
				return variant.bool_value();
			case Variant.Type.StaticString:
			case Variant.Type.MutableString:
				return variant.string_value();
			case Variant.Type.Vector:
			{
				List<object> list = new List<object>();
				foreach (Variant variant2 in variant.vector())
				{
					list.Add(variant2.ToObject(options));
				}
				return list;
			}
			case Variant.Type.Map:
			{
				if (options == VariantExtension.KeyOptions.UseStringKeys)
				{
					return variant.map().ToStringVariantMap(options);
				}
				Dictionary<object, object> dictionary = new Dictionary<object, object>();
				foreach (KeyValuePair<Variant, Variant> keyValuePair in variant.map())
				{
					object key = keyValuePair.Key.ToObject(options);
					object value = keyValuePair.Value.ToObject(options);
					dictionary[key] = value;
				}
				return dictionary;
			}
			case Variant.Type.StaticBlob:
			case Variant.Type.MutableBlob:
				return variant.blob_as_bytes();
			}
			return null;
		}

		public static IDictionary<string, object> ToStringVariantMap(this VariantVariantMap originalMap)
		{
			return originalMap.ToStringVariantMap(VariantExtension.KeyOptions.UseObjectKeys);
		}

		public static IDictionary<string, object> ToStringVariantMap(this VariantVariantMap originalMap, VariantExtension.KeyOptions options)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<Variant, Variant> keyValuePair in originalMap)
			{
				string key;
				if (keyValuePair.Key.is_string())
				{
					key = keyValuePair.Key.string_value();
				}
				else
				{
					if (!keyValuePair.Key.is_fundamental_type())
					{
						throw new InvalidCastException("Unable to convert dictionary keys to string");
					}
					key = keyValuePair.Key.AsString().string_value();
				}
				object value = keyValuePair.Value.ToObject(options);
				dictionary[key] = value;
			}
			return dictionary;
		}

		internal enum KeyOptions
		{
			UseObjectKeys,
			UseStringKeys
		}
	}
}
