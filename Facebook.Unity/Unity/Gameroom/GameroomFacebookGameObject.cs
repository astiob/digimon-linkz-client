using System;
using System.Collections;

namespace Facebook.Unity.Gameroom
{
	internal class GameroomFacebookGameObject : FacebookGameObject, IFacebookCallbackHandler
	{
		protected IGameroomFacebookImplementation GameroomFacebookImpl
		{
			get
			{
				return (IGameroomFacebookImplementation)base.Facebook;
			}
		}

		public void WaitForResponse(GameroomFacebook.OnComplete onCompleteDelegate, string callbackId)
		{
			base.StartCoroutine(this.WaitForPipeResponse(onCompleteDelegate, callbackId));
		}

		protected override void OnAwake()
		{
		}

		private IEnumerator WaitForPipeResponse(GameroomFacebook.OnComplete onCompleteDelegate, string callbackId)
		{
			while (!this.GameroomFacebookImpl.HaveReceivedPipeResponse())
			{
				yield return null;
			}
			onCompleteDelegate(new ResultContainer(this.GameroomFacebookImpl.GetPipeResponse(callbackId)));
			yield break;
		}
	}
}
