using System;
using UnityEngine;

namespace UI.Common
{
	public interface IPayConfirmNotice
	{
		void SetPushActionYesButton(Action<UnityEngine.Object> action);

		void SetMessage(string title, string info);

		void SetAssetIcon(int category, string assetsValue);

		void SetAssetValue(int possession, int cost);

		void SetUseDetail(object detail);

		object GetUseDetail();
	}
}
