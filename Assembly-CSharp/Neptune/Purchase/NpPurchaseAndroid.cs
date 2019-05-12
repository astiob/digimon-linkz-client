using System;
using UnityEngine;

namespace Neptune.Purchase
{
	public class NpPurchaseAndroid
	{
		public static void Init(string publicKey, string receiverGoName)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("init", new object[]
						{
							publicKey,
							@static,
							receiverGoName
						});
					}
				}
			}
		}

		public static void RetryTransactionStartUp()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("retryServerRequest", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void purchase(string productId, bool isConsumable)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("purchaseItem", new object[]
						{
							productId,
							isConsumable,
							@static
						});
					}
				}
			}
		}

		public static string GetProductId()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						result = androidJavaClass2.CallStatic<string>("getProductId", new object[0]);
					}
				}
			}
			return result;
		}

		public static string GetReceipt()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						result = androidJavaClass2.CallStatic<string>("getReceipt", new object[0]);
					}
				}
			}
			return result;
		}

		public static string GetSignature()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						result = androidJavaClass2.CallStatic<string>("getSignature", new object[0]);
					}
				}
			}
			return result;
		}

		public static void SuccessPurchase()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("successPurchase", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void RetryConsume()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("retryServerRequest", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void RestoreStart()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("restoreStart", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void RequestProducts(int type)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("getItemDetails", new object[]
						{
							type,
							@static
						});
					}
				}
			}
		}

		public static void SetRequestProduct(string productId)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("setRequestItem", new object[]
						{
							productId
						});
					}
				}
			}
		}

		public static void SetComparisonProduct(string productPrice)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("setComparisonProduct", new object[]
						{
							productPrice
						});
					}
				}
			}
		}

		public static string GetRequestProductsJson()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.SkuDetails"))
					{
						result = androidJavaClass2.Call<string>("toString", new object[0]);
					}
				}
			}
			return result;
		}

		public static bool GetIsPurchaseSupport()
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						result = androidJavaClass2.CallStatic<bool>("getIsInAppBillingV3Support", new object[0]);
					}
				}
			}
			return result;
		}

		public static void PurchaseDestroy()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("purchaseDestroy", new object[0]);
					}
				}
			}
		}

		public static void AllConsumption()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.billing.NPInAppBilling"))
					{
						androidJavaClass2.CallStatic("consumeOwnedItems", new object[]
						{
							@static
						});
					}
				}
			}
		}
	}
}
