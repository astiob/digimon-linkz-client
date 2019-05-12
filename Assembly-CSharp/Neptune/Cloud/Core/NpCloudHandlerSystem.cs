using JsonFx.Json;
using Neptune.OAuth;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neptune.Cloud.Core
{
	public class NpCloudHandlerSystem : MonoBehaviour
	{
		private INpCloudHandlerSystem mListener;

		public new static GameObject gameObject;

		private NpCloudHandlerSystem()
		{
		}

		public static NpCloudHandlerSystem CreateInstance(INpCloudHandlerSystem listener)
		{
			NpCloudHandlerSystem.gameObject = new GameObject("NpClientHandlerSystem");
			NpCloudHandlerSystem npCloudHandlerSystem = NpCloudHandlerSystem.gameObject.AddComponent<NpCloudHandlerSystem>();
			npCloudHandlerSystem.mListener = listener;
			npCloudHandlerSystem.Active(false);
			return npCloudHandlerSystem;
		}

		public void Destroy()
		{
			UnityEngine.Object.Destroy(NpCloudHandlerSystem.gameObject);
		}

		public void Active(bool active)
		{
			NpCloudHandlerSystem.gameObject.SetActive(active);
		}

		public void GetRoomList(NpCloudRoomListData data)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["user_id"] = data.userId;
			dictionary["room_list_type"] = (int)data.roomListType;
			dictionary["room_type"] = (int)data.roomType;
			dictionary["page"] = data.page;
			dictionary["limit"] = data.limit;
			if (data.roomConditionList != null)
			{
				dictionary["room_condition"] = JsonWriter.Serialize(data.roomConditionList);
			}
			Dictionary<string, string> headers = NpOAuth.Instance.RequestHeaderDic("POST", data.url, dictionary);
			NpHttpConnection npHttpConnection = new NpHttpConnection(this, new Action<WWW>(this.OnHttpResponse), new Action(this.OnTimeOutErr));
			base.StartCoroutine(npHttpConnection.PostRequest(data.url, dictionary, headers));
		}

		private void OnHttpResponse(WWW www)
		{
			if (www.error != null)
			{
				this.mListener.OnHttpRequestException(761u, string.Empty, "Httpリクエスト通信エラー", www.error);
				return;
			}
			List<NpRoomParameter> list = new List<NpRoomParameter>();
			Dictionary<string, object> dictionary = JsonReader.Deserialize<Dictionary<string, object>>(www.text);
			Dictionary<string, object> dictionary2 = dictionary["response"] as Dictionary<string, object>;
			int num = (int)dictionary2["result"];
			if (num != 0)
			{
				string detail = dictionary2["detail"] as string;
				this.mListener.OnHttpRequestException((uint)num, string.Empty, string.Empty, detail);
				return;
			}
			Dictionary<string, object>[] array = dictionary2["room_list"] as Dictionary<string, object>[];
			if (array != null)
			{
				foreach (Dictionary<string, object> dictionary3 in array)
				{
					NpRoomParameter npRoomParameter = new NpRoomParameter();
					npRoomParameter.RoomId = (dictionary3["room_id"] as string);
					npRoomParameter.RoomName = (dictionary3["room_name"] as string);
					npRoomParameter.RoomType = (RoomType)dictionary3["room_type"];
					npRoomParameter.Owner = (int)dictionary3["owner"];
					int[] collection = (int[])dictionary3["member_list"];
					npRoomParameter.MemberList.AddRange(collection);
					if (dictionary3.ContainsKey("room_condition"))
					{
						npRoomParameter.RoomCondition = (dictionary3["room_condition"] as List<RoomCondition>);
					}
					list.Add(npRoomParameter);
				}
			}
			this.mListener.OnGetRoomList(list);
		}

		private void OnTimeOutErr()
		{
			this.mListener.OnHttpRequestException(720u, string.Empty, "接続リクエストのタイムアウトです。", string.Empty);
		}

		private void Update()
		{
			this.mListener.Update();
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			this.mListener.OnApplicationPause(pauseStatus);
		}

		private void OnApplicationQuit()
		{
			this.mListener.OnApplicationQuit();
		}
	}
}
