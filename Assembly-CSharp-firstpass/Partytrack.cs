using PartytrackUnityPlugin;
using System;
using UnityEngine;

public class Partytrack
{
	public static string UUID = "uuid";

	public static string UDID = "udid";

	public static string ClientID = "client_id";

	private static AndroidJavaClass getTrack()
	{
		return new AndroidJavaClass("it.partytrack.sdk.Track");
	}

	private static AndroidJavaObject getActivity()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		return androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public static void start(int app_id, string app_key)
	{
		Partytrack.getTrack().CallStatic("start", new object[]
		{
			Partytrack.getActivity(),
			app_id,
			app_key,
			Partytrack.getActivity().Call<AndroidJavaObject>("getIntent", new object[0])
		});
	}

	public static void openDebugInfo()
	{
		Partytrack.getTrack().CallStatic("setDebugMode", new object[]
		{
			true
		});
	}

	public static void setConfigure(string name, string svalue)
	{
		Partytrack.getTrack().CallStatic("setOptionalparam", new object[]
		{
			name,
			svalue
		});
	}

	public static void setCustomEventParameter(string name, string svalue)
	{
		Partytrack.getTrack().CallStatic("setCustomEventParameter", new object[]
		{
			name,
			svalue
		});
	}

	public static void sendEvent(int event_id)
	{
		Partytrack.getTrack().CallStatic("event", new object[]
		{
			event_id
		});
	}

	public static void sendEvent(string event_name)
	{
		Partytrack.getTrack().CallStatic("event", new object[]
		{
			event_name
		});
	}

	public static void sendEventWithItems(string event_name, Item[] items)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.lang.reflect.Array");
		AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("newInstance", new object[]
		{
			new AndroidJavaClass("it.partytrack.sdk.Item"),
			items.Length
		});
		AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("java.lang.Integer");
		AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("java.lang.Float");
		for (int i = 0; i < items.Length; i++)
		{
			AndroidJavaObject androidJavaObject2 = null;
			if (items[i].item_num != null)
			{
				androidJavaObject2 = androidJavaClass2.CallStatic<AndroidJavaObject>("valueOf", new object[]
				{
					items[i].item_num.Value
				});
			}
			AndroidJavaObject androidJavaObject3 = null;
			if (items[i].item_price != null)
			{
				androidJavaObject3 = androidJavaClass3.CallStatic<AndroidJavaObject>("valueOf", new object[]
				{
					(float)items[i].item_price.Value
				});
			}
			AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("it.partytrack.sdk.Item", new object[0]);
			if (items[i].identifier != null)
			{
				androidJavaObject4.Set<string>("identifier", items[i].identifier);
			}
			if (items[i].item_name != null)
			{
				androidJavaObject4.Set<string>("name", items[i].item_name);
			}
			if (androidJavaObject2 != null)
			{
				androidJavaObject4.Set<AndroidJavaObject>("num", androidJavaObject2);
			}
			if (androidJavaObject3 != null)
			{
				androidJavaObject4.Set<AndroidJavaObject>("price", androidJavaObject3);
			}
			if (items[i].item_price_currency != null)
			{
				androidJavaObject4.Set<string>("currency", items[i].item_price_currency);
			}
			if (items[i].achievement != null)
			{
				androidJavaObject4.Set<string>("achievement", items[i].achievement);
			}
			if (items[i].content_type != null)
			{
				androidJavaObject4.Set<string>("content_type", items[i].content_type);
			}
			if (items[i].level_value != null)
			{
				androidJavaObject4.Set<string>("level_value", items[i].level_value);
			}
			if (items[i].max_rating_value != null)
			{
				androidJavaObject4.Set<string>("max_rating_value", items[i].max_rating_value);
			}
			if (items[i].payment_info != null)
			{
				androidJavaObject4.Set<string>("payment_info", items[i].payment_info);
			}
			if (items[i].rating_value != null)
			{
				androidJavaObject4.Set<string>("rating_value", items[i].rating_value);
			}
			if (items[i].registration_method != null)
			{
				androidJavaObject4.Set<string>("registration_method", items[i].registration_method);
			}
			if (items[i].search_string != null)
			{
				androidJavaObject4.Set<string>("search_string", items[i].search_string);
			}
			if (items[i].virtual_currency != null)
			{
				androidJavaObject4.Set<string>("virtual_currency", items[i].virtual_currency);
			}
			if (items[i].virtual_currency_price != null)
			{
				androidJavaObject4.Set<string>("virtual_currency_price", items[i].virtual_currency_price);
			}
			androidJavaClass.CallStatic("set", new object[]
			{
				androidJavaObject,
				i,
				androidJavaObject4
			});
		}
		AndroidJavaClass androidJavaClass4 = new AndroidJavaClass("java.util.Arrays");
		Partytrack.getTrack().CallStatic("items", new object[]
		{
			event_name,
			androidJavaClass4.CallStatic<AndroidJavaObject>("asList", new object[]
			{
				androidJavaObject
			})
		});
	}

	private static string sanitizeItemsToString(Item[] items)
	{
		string[] array = new string[items.Length];
		for (int i = 0; i < items.Length; i++)
		{
			string text = string.Empty;
			text = text + ((items[i].identifier == null) ? string.Empty : Partytrack.sanitize(items[i].identifier)) + ",";
			text = text + ((items[i].item_name == null) ? string.Empty : Partytrack.sanitize(items[i].item_name)) + ",";
			text = text + ((items[i].item_num == null) ? string.Empty : Partytrack.sanitize(items[i].item_num.ToString())) + ",";
			text = text + ((items[i].item_price == null) ? string.Empty : Partytrack.sanitize(items[i].item_price.ToString())) + ",";
			text = text + ((items[i].item_price_currency == null) ? string.Empty : Partytrack.sanitize(items[i].item_price_currency)) + ",";
			text = text + ((items[i].achievement == null) ? string.Empty : Partytrack.sanitize(items[i].achievement)) + ",";
			text = text + ((items[i].content_type == null) ? string.Empty : Partytrack.sanitize(items[i].content_type)) + ",";
			text = text + ((items[i].level_value == null) ? string.Empty : Partytrack.sanitize(items[i].level_value)) + ",";
			text = text + ((items[i].max_rating_value == null) ? string.Empty : Partytrack.sanitize(items[i].max_rating_value)) + ",";
			text = text + ((items[i].payment_info == null) ? string.Empty : Partytrack.sanitize(items[i].payment_info)) + ",";
			text = text + ((items[i].rating_value == null) ? string.Empty : Partytrack.sanitize(items[i].rating_value)) + ",";
			text = text + ((items[i].registration_method == null) ? string.Empty : Partytrack.sanitize(items[i].registration_method)) + ",";
			text = text + ((items[i].search_string == null) ? string.Empty : Partytrack.sanitize(items[i].search_string)) + ",";
			text = text + ((items[i].virtual_currency == null) ? string.Empty : Partytrack.sanitize(items[i].virtual_currency)) + ",";
			text += ((items[i].virtual_currency_price == null) ? string.Empty : Partytrack.sanitize(items[i].virtual_currency_price));
			array[i] = text;
		}
		return string.Join(":", array);
	}

	private static string sanitize(string str)
	{
		return str.Replace("%", "%25").Replace(",", "%2c").Replace(":", "%3a");
	}

	public static void sendPayment(string item_name, int item_num, string item_price_currency, double item_price)
	{
		float num = (float)item_price;
		Partytrack.getTrack().CallStatic("payment", new object[]
		{
			item_name,
			num,
			item_price_currency,
			item_num
		});
	}

	public static void disableAdvertisementOptimize()
	{
		Partytrack.getTrack().CallStatic("disableAdvertisementOptimize", new object[0]);
	}
}
