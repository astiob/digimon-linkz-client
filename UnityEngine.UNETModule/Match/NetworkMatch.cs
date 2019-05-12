using SimpleJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
	public class NetworkMatch : MonoBehaviour
	{
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

		[Obsolete("This function is not used any longer to interface with the matchmaker. Please set up your project by logging in through the editor connect dialog.", true)]
		public void SetProgramAppID(AppID programAppID)
		{
		}

		public Coroutine CreateMatch(string matchName, uint matchSize, bool matchAdvertise, string matchPassword, string publicClientAddress, string privateClientAddress, int eloScoreForMatch, int requestDomain, NetworkMatch.DataResponseDelegate<MatchInfo> callback)
		{
			Coroutine result;
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				Debug.LogError("Matchmaking is not supported on WebGL player.");
				result = null;
			}
			else
			{
				result = this.CreateMatch(new CreateMatchRequest
				{
					name = matchName,
					size = matchSize,
					advertise = matchAdvertise,
					password = matchPassword,
					publicAddress = publicClientAddress,
					privateAddress = privateClientAddress,
					eloScore = eloScoreForMatch,
					domain = requestDomain
				}, callback);
			}
			return result;
		}

		internal Coroutine CreateMatch(CreateMatchRequest req, NetworkMatch.DataResponseDelegate<MatchInfo> callback)
		{
			Coroutine result;
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting CreateMatch Request.");
				result = null;
			}
			else
			{
				Uri uri = new Uri(this.baseUri, "/json/reply/CreateMatchRequest");
				Debug.Log("MatchMakingClient Create :" + uri);
				WWWForm wwwform = new WWWForm();
				wwwform.AddField("version", Request.currentVersion);
				wwwform.AddField("projectId", Application.cloudProjectId);
				wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
				wwwform.AddField("accessTokenString", 0);
				wwwform.AddField("domain", req.domain);
				wwwform.AddField("name", req.name);
				wwwform.AddField("size", req.size.ToString());
				wwwform.AddField("advertise", req.advertise.ToString());
				wwwform.AddField("password", req.password);
				wwwform.AddField("publicAddress", req.publicAddress);
				wwwform.AddField("privateAddress", req.privateAddress);
				wwwform.AddField("eloScore", req.eloScore.ToString());
				wwwform.headers["Accept"] = "application/json";
				UnityWebRequest client = UnityWebRequest.Post(uri.ToString(), wwwform);
				result = base.StartCoroutine(this.ProcessMatchResponse<CreateMatchResponse, NetworkMatch.DataResponseDelegate<MatchInfo>>(client, new NetworkMatch.InternalResponseDelegate<CreateMatchResponse, NetworkMatch.DataResponseDelegate<MatchInfo>>(this.OnMatchCreate), callback));
			}
			return result;
		}

		internal virtual void OnMatchCreate(CreateMatchResponse response, NetworkMatch.DataResponseDelegate<MatchInfo> userCallback)
		{
			if (response.success)
			{
				Utility.SetAccessTokenForNetwork(response.networkId, new NetworkAccessToken(response.accessTokenString));
			}
			userCallback(response.success, response.extendedInfo, new MatchInfo(response));
		}

		public Coroutine JoinMatch(NetworkID netId, string matchPassword, string publicClientAddress, string privateClientAddress, int eloScoreForClient, int requestDomain, NetworkMatch.DataResponseDelegate<MatchInfo> callback)
		{
			return this.JoinMatch(new JoinMatchRequest
			{
				networkId = netId,
				password = matchPassword,
				publicAddress = publicClientAddress,
				privateAddress = privateClientAddress,
				eloScore = eloScoreForClient,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine JoinMatch(JoinMatchRequest req, NetworkMatch.DataResponseDelegate<MatchInfo> callback)
		{
			Coroutine result;
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting JoinMatch Request.");
				result = null;
			}
			else
			{
				Uri uri = new Uri(this.baseUri, "/json/reply/JoinMatchRequest");
				Debug.Log("MatchMakingClient Join :" + uri);
				WWWForm wwwform = new WWWForm();
				wwwform.AddField("version", Request.currentVersion);
				wwwform.AddField("projectId", Application.cloudProjectId);
				wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
				wwwform.AddField("accessTokenString", 0);
				wwwform.AddField("domain", req.domain);
				wwwform.AddField("networkId", req.networkId.ToString());
				wwwform.AddField("password", req.password);
				wwwform.AddField("publicAddress", req.publicAddress);
				wwwform.AddField("privateAddress", req.privateAddress);
				wwwform.AddField("eloScore", req.eloScore.ToString());
				wwwform.headers["Accept"] = "application/json";
				UnityWebRequest client = UnityWebRequest.Post(uri.ToString(), wwwform);
				result = base.StartCoroutine(this.ProcessMatchResponse<JoinMatchResponse, NetworkMatch.DataResponseDelegate<MatchInfo>>(client, new NetworkMatch.InternalResponseDelegate<JoinMatchResponse, NetworkMatch.DataResponseDelegate<MatchInfo>>(this.OnMatchJoined), callback));
			}
			return result;
		}

		internal void OnMatchJoined(JoinMatchResponse response, NetworkMatch.DataResponseDelegate<MatchInfo> userCallback)
		{
			if (response.success)
			{
				Utility.SetAccessTokenForNetwork(response.networkId, new NetworkAccessToken(response.accessTokenString));
			}
			userCallback(response.success, response.extendedInfo, new MatchInfo(response));
		}

		public Coroutine DestroyMatch(NetworkID netId, int requestDomain, NetworkMatch.BasicResponseDelegate callback)
		{
			return this.DestroyMatch(new DestroyMatchRequest
			{
				networkId = netId,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine DestroyMatch(DestroyMatchRequest req, NetworkMatch.BasicResponseDelegate callback)
		{
			Coroutine result;
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting DestroyMatch Request.");
				result = null;
			}
			else
			{
				Uri uri = new Uri(this.baseUri, "/json/reply/DestroyMatchRequest");
				Debug.Log("MatchMakingClient Destroy :" + uri.ToString());
				WWWForm wwwform = new WWWForm();
				wwwform.AddField("version", Request.currentVersion);
				wwwform.AddField("projectId", Application.cloudProjectId);
				wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
				wwwform.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
				wwwform.AddField("domain", req.domain);
				wwwform.AddField("networkId", req.networkId.ToString());
				wwwform.headers["Accept"] = "application/json";
				UnityWebRequest client = UnityWebRequest.Post(uri.ToString(), wwwform);
				result = base.StartCoroutine(this.ProcessMatchResponse<BasicResponse, NetworkMatch.BasicResponseDelegate>(client, new NetworkMatch.InternalResponseDelegate<BasicResponse, NetworkMatch.BasicResponseDelegate>(this.OnMatchDestroyed), callback));
			}
			return result;
		}

		internal void OnMatchDestroyed(BasicResponse response, NetworkMatch.BasicResponseDelegate userCallback)
		{
			userCallback(response.success, response.extendedInfo);
		}

		public Coroutine DropConnection(NetworkID netId, NodeID dropNodeId, int requestDomain, NetworkMatch.BasicResponseDelegate callback)
		{
			return this.DropConnection(new DropConnectionRequest
			{
				networkId = netId,
				nodeId = dropNodeId,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine DropConnection(DropConnectionRequest req, NetworkMatch.BasicResponseDelegate callback)
		{
			Coroutine result;
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting DropConnection Request.");
				result = null;
			}
			else
			{
				Uri uri = new Uri(this.baseUri, "/json/reply/DropConnectionRequest");
				Debug.Log("MatchMakingClient DropConnection :" + uri);
				WWWForm wwwform = new WWWForm();
				wwwform.AddField("version", Request.currentVersion);
				wwwform.AddField("projectId", Application.cloudProjectId);
				wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
				wwwform.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
				wwwform.AddField("domain", req.domain);
				wwwform.AddField("networkId", req.networkId.ToString());
				wwwform.AddField("nodeId", req.nodeId.ToString());
				wwwform.headers["Accept"] = "application/json";
				UnityWebRequest client = UnityWebRequest.Post(uri.ToString(), wwwform);
				result = base.StartCoroutine(this.ProcessMatchResponse<DropConnectionResponse, NetworkMatch.BasicResponseDelegate>(client, new NetworkMatch.InternalResponseDelegate<DropConnectionResponse, NetworkMatch.BasicResponseDelegate>(this.OnDropConnection), callback));
			}
			return result;
		}

		internal void OnDropConnection(DropConnectionResponse response, NetworkMatch.BasicResponseDelegate userCallback)
		{
			userCallback(response.success, response.extendedInfo);
		}

		public Coroutine ListMatches(int startPageNumber, int resultPageSize, string matchNameFilter, bool filterOutPrivateMatchesFromResults, int eloScoreTarget, int requestDomain, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>> callback)
		{
			Coroutine result;
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				Debug.LogError("Matchmaking is not supported on WebGL player.");
				result = null;
			}
			else
			{
				result = this.ListMatches(new ListMatchRequest
				{
					pageNum = startPageNumber,
					pageSize = resultPageSize,
					nameFilter = matchNameFilter,
					filterOutPrivateMatches = filterOutPrivateMatchesFromResults,
					eloScore = eloScoreTarget,
					domain = requestDomain
				}, callback);
			}
			return result;
		}

		internal Coroutine ListMatches(ListMatchRequest req, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>> callback)
		{
			Coroutine result;
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting ListMatch Request.");
				result = null;
			}
			else
			{
				Uri uri = new Uri(this.baseUri, "/json/reply/ListMatchRequest");
				Debug.Log("MatchMakingClient ListMatches :" + uri);
				WWWForm wwwform = new WWWForm();
				wwwform.AddField("version", Request.currentVersion);
				wwwform.AddField("projectId", Application.cloudProjectId);
				wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
				wwwform.AddField("accessTokenString", 0);
				wwwform.AddField("domain", req.domain);
				wwwform.AddField("pageSize", req.pageSize);
				wwwform.AddField("pageNum", req.pageNum);
				wwwform.AddField("nameFilter", req.nameFilter);
				wwwform.AddField("filterOutPrivateMatches", req.filterOutPrivateMatches.ToString());
				wwwform.AddField("eloScore", req.eloScore.ToString());
				wwwform.headers["Accept"] = "application/json";
				UnityWebRequest client = UnityWebRequest.Post(uri.ToString(), wwwform);
				result = base.StartCoroutine(this.ProcessMatchResponse<ListMatchResponse, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>>>(client, new NetworkMatch.InternalResponseDelegate<ListMatchResponse, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>>>(this.OnMatchList), callback));
			}
			return result;
		}

		internal void OnMatchList(ListMatchResponse response, NetworkMatch.DataResponseDelegate<List<MatchInfoSnapshot>> userCallback)
		{
			List<MatchInfoSnapshot> list = new List<MatchInfoSnapshot>();
			foreach (MatchDesc matchDesc in response.matches)
			{
				list.Add(new MatchInfoSnapshot(matchDesc));
			}
			userCallback(response.success, response.extendedInfo, list);
		}

		public Coroutine SetMatchAttributes(NetworkID networkId, bool isListed, int requestDomain, NetworkMatch.BasicResponseDelegate callback)
		{
			return this.SetMatchAttributes(new SetMatchAttributesRequest
			{
				networkId = networkId,
				isListed = isListed,
				domain = requestDomain
			}, callback);
		}

		internal Coroutine SetMatchAttributes(SetMatchAttributesRequest req, NetworkMatch.BasicResponseDelegate callback)
		{
			Coroutine result;
			if (callback == null)
			{
				Debug.Log("callback supplied is null, aborting SetMatchAttributes Request.");
				result = null;
			}
			else
			{
				Uri uri = new Uri(this.baseUri, "/json/reply/SetMatchAttributesRequest");
				Debug.Log("MatchMakingClient SetMatchAttributes :" + uri);
				WWWForm wwwform = new WWWForm();
				wwwform.AddField("version", Request.currentVersion);
				wwwform.AddField("projectId", Application.cloudProjectId);
				wwwform.AddField("sourceId", Utility.GetSourceID().ToString());
				wwwform.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
				wwwform.AddField("domain", req.domain);
				wwwform.AddField("networkId", req.networkId.ToString());
				wwwform.AddField("isListed", req.isListed.ToString());
				wwwform.headers["Accept"] = "application/json";
				UnityWebRequest client = UnityWebRequest.Post(uri.ToString(), wwwform);
				result = base.StartCoroutine(this.ProcessMatchResponse<BasicResponse, NetworkMatch.BasicResponseDelegate>(client, new NetworkMatch.InternalResponseDelegate<BasicResponse, NetworkMatch.BasicResponseDelegate>(this.OnSetMatchAttributes), callback));
			}
			return result;
		}

		internal void OnSetMatchAttributes(BasicResponse response, NetworkMatch.BasicResponseDelegate userCallback)
		{
			userCallback(response.success, response.extendedInfo);
		}

		private IEnumerator ProcessMatchResponse<JSONRESPONSE, USERRESPONSEDELEGATETYPE>(UnityWebRequest client, NetworkMatch.InternalResponseDelegate<JSONRESPONSE, USERRESPONSEDELEGATETYPE> internalCallback, USERRESPONSEDELEGATETYPE userCallback) where JSONRESPONSE : Response, new()
		{
			yield return client.SendWebRequest();
			JSONRESPONSE jsonInterface = Activator.CreateInstance<JSONRESPONSE>();
			if (!client.isNetworkError && !client.isHttpError)
			{
				object obj;
				if (SimpleJson.TryDeserializeObject(client.downloadHandler.text, out obj))
				{
					IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
					if (dictionary != null)
					{
						try
						{
							jsonInterface.Parse(obj);
						}
						catch (FormatException ex)
						{
							jsonInterface.SetFailure(UnityString.Format("FormatException:[{0}] ", new object[]
							{
								ex.ToString()
							}));
						}
					}
				}
			}
			else
			{
				jsonInterface.SetFailure(UnityString.Format("Request error:[{0}] Raw response:[{1}]", new object[]
				{
					client.error,
					client.downloadHandler.text
				}));
			}
			client.Dispose();
			internalCallback(jsonInterface, userCallback);
			yield break;
		}

		public delegate void BasicResponseDelegate(bool success, string extendedInfo);

		public delegate void DataResponseDelegate<T>(bool success, string extendedInfo, T responseData);

		private delegate void InternalResponseDelegate<T, U>(T response, U userCallback);
	}
}
