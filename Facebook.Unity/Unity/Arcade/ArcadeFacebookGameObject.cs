using System;
using System.Collections;

namespace Facebook.Unity.Arcade
{
	internal class ArcadeFacebookGameObject : FacebookGameObject, IFacebookCallbackHandler
	{
		protected IArcadeFacebookImplementation ArcadeFacebookImpl
		{
			get
			{
				return (IArcadeFacebookImplementation)base.Facebook;
			}
		}

		public void WaitForResponse(ArcadeFacebook.OnComplete onCompleteDelegate, string callbackId)
		{
			base.StartCoroutine(this.WaitForPipeResponse(onCompleteDelegate, callbackId));
		}

		protected override void OnAwake()
		{
		}

		private IEnumerator WaitForPipeResponse(ArcadeFacebook.OnComplete onCompleteDelegate, string callbackId)
		{
			while (!this.ArcadeFacebookImpl.HaveReceivedPipeResponse())
			{
				yield return null;
			}
			onCompleteDelegate(new ResultContainer(this.ArcadeFacebookImpl.GetPipeResponse(callbackId)));
			yield break;
		}
	}
}
