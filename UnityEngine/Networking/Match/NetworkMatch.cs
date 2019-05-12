using SimpleJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	public class NetworkMatch : MonoBehaviour
	{
		private const string kMultiplayerNetworkingIdKey = "CloudNetworkingId";

		private Uri m_BaseUri = new Uri("https://mm.unet.unity3d.com");

		public Uri baseUri
		{
			get
			{
				return this.m_BaseUri;
			}
			set
			{
				this.m_BaseUri = value;
			}
		}

		public void SetProgramAppID(AppID programAppID)
		{
			Utility.SetAppID(programAppID);
		}

		public Coroutine CreateMatch(string matchName, uint matchSize, bool matchAdvertise, string matchPassword, NetworkMatch.ResponseDelegate<CreateMatchResponse> callback)
		{
			return this.CreateMatch(new CreateMatchRequest
			{
				name = matchName,
				size = matchSize,
				advertise = matchAdvertise,
				password = matchPassword
			}, callback);
		}

		public Coroutine CreateMatch(CreateMatchRequest req, NetworkMatch.ResponseDelegate<CreateMatchResponse> callback)
		{
			Uri uri = new Uri(this.baseUri, "/json/reply/CreateMatchRequest");
			Debug.Log("MatchMakingClient Create :" + uri);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("projectId", Application.cloudProjectId);
			wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
			wwwform.AddField("appId", Utility.GetAppID().ToString());
			wwwform.AddField("accessTokenString", 0);
			wwwform.AddField("domain", 0);
			wwwform.AddField("name", req.name);
			wwwform.AddField("size", req.size.ToString());
			wwwform.AddField("advertise", req.advertise.ToString());
			wwwform.AddField("password", req.password);
			wwwform.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wwwform);
			return base.StartCoroutine(this.ProcessMatchResponse<CreateMatchResponse>(client, callback));
		}

		public Coroutine JoinMatch(NetworkID netId, string matchPassword, NetworkMatch.ResponseDelegate<JoinMatchResponse> callback)
		{
			return this.JoinMatch(new JoinMatchRequest
			{
				networkId = netId,
				password = matchPassword
			}, callback);
		}

		public Coroutine JoinMatch(JoinMatchRequest req, NetworkMatch.ResponseDelegate<JoinMatchResponse> callback)
		{
			Uri uri = new Uri(this.baseUri, "/json/reply/JoinMatchRequest");
			Debug.Log("MatchMakingClient Join :" + uri);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("projectId", Application.cloudProjectId);
			wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
			wwwform.AddField("appId", Utility.GetAppID().ToString());
			wwwform.AddField("accessTokenString", 0);
			wwwform.AddField("domain", 0);
			wwwform.AddField("networkId", req.networkId.ToString());
			wwwform.AddField("password", req.password);
			wwwform.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wwwform);
			return base.StartCoroutine(this.ProcessMatchResponse<JoinMatchResponse>(client, callback));
		}

		public Coroutine DestroyMatch(NetworkID netId, NetworkMatch.ResponseDelegate<BasicResponse> callback)
		{
			return this.DestroyMatch(new DestroyMatchRequest
			{
				networkId = netId
			}, callback);
		}

		public Coroutine DestroyMatch(DestroyMatchRequest req, NetworkMatch.ResponseDelegate<BasicResponse> callback)
		{
			Uri uri = new Uri(this.baseUri, "/json/reply/DestroyMatchRequest");
			Debug.Log("MatchMakingClient Destroy :" + uri.ToString());
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("projectId", Application.cloudProjectId);
			wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
			wwwform.AddField("appId", Utility.GetAppID().ToString());
			wwwform.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
			wwwform.AddField("domain", 0);
			wwwform.AddField("networkId", req.networkId.ToString());
			wwwform.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wwwform);
			return base.StartCoroutine(this.ProcessMatchResponse<BasicResponse>(client, callback));
		}

		public Coroutine DropConnection(NetworkID netId, NodeID dropNodeId, NetworkMatch.ResponseDelegate<BasicResponse> callback)
		{
			return this.DropConnection(new DropConnectionRequest
			{
				networkId = netId,
				nodeId = dropNodeId
			}, callback);
		}

		public Coroutine DropConnection(DropConnectionRequest req, NetworkMatch.ResponseDelegate<BasicResponse> callback)
		{
			Uri uri = new Uri(this.baseUri, "/json/reply/DropConnectionRequest");
			Debug.Log("MatchMakingClient DropConnection :" + uri);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("projectId", Application.cloudProjectId);
			wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
			wwwform.AddField("appId", Utility.GetAppID().ToString());
			wwwform.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
			wwwform.AddField("domain", 0);
			wwwform.AddField("networkId", req.networkId.ToString());
			wwwform.AddField("nodeId", req.nodeId.ToString());
			wwwform.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wwwform);
			return base.StartCoroutine(this.ProcessMatchResponse<BasicResponse>(client, callback));
		}

		public Coroutine ListMatches(int startPageNumber, int resultPageSize, string matchNameFilter, NetworkMatch.ResponseDelegate<ListMatchResponse> callback)
		{
			return this.ListMatches(new ListMatchRequest
			{
				pageNum = startPageNumber,
				pageSize = resultPageSize,
				nameFilter = matchNameFilter
			}, callback);
		}

		public Coroutine ListMatches(ListMatchRequest req, NetworkMatch.ResponseDelegate<ListMatchResponse> callback)
		{
			Uri uri = new Uri(this.baseUri, "/json/reply/ListMatchRequest");
			Debug.Log("MatchMakingClient ListMatches :" + uri);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("projectId", Application.cloudProjectId);
			wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
			wwwform.AddField("appId", Utility.GetAppID().ToString());
			wwwform.AddField("includePasswordMatches", req.includePasswordMatches.ToString());
			wwwform.AddField("accessTokenString", 0);
			wwwform.AddField("domain", 0);
			wwwform.AddField("pageSize", req.pageSize);
			wwwform.AddField("pageNum", req.pageNum);
			wwwform.AddField("nameFilter", req.nameFilter);
			wwwform.headers["Accept"] = "application/json";
			WWW client = new WWW(uri.ToString(), wwwform);
			return base.StartCoroutine(this.ProcessMatchResponse<ListMatchResponse>(client, callback));
		}

		private IEnumerator ProcessMatchResponse<JSONRESPONSE>(WWW client, NetworkMatch.ResponseDelegate<JSONRESPONSE> callback) where JSONRESPONSE : Response, new()
		{
			yield return client;
			JSONRESPONSE jsonInterface = (JSONRESPONSE)((object)null);
			if (string.IsNullOrEmpty(client.error))
			{
				object o;
				if (SimpleJson.TryDeserializeObject(client.text, out o))
				{
					IDictionary<string, object> dictJsonObj = o as IDictionary<string, object>;
					if (dictJsonObj != null)
					{
						try
						{
							jsonInterface = Activator.CreateInstance<JSONRESPONSE>();
							jsonInterface.Parse(o);
						}
						catch (FormatException ex)
						{
							FormatException exception = ex;
							Debug.Log(exception);
						}
					}
				}
				if (jsonInterface == null)
				{
					Debug.LogError("Could not parse: " + client.text);
				}
				else
				{
					Debug.Log("JSON Response: " + jsonInterface.ToString());
				}
			}
			else
			{
				Debug.LogError("Request error: " + client.error);
				Debug.LogError("Raw response: " + client.text);
			}
			if (jsonInterface == null)
			{
				jsonInterface = Activator.CreateInstance<JSONRESPONSE>();
			}
			callback(jsonInterface);
			yield break;
		}

		public delegate void ResponseDelegate<T>(T response);
	}
}
