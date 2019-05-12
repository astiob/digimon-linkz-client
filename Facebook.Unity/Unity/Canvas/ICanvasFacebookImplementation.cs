using System;

namespace Facebook.Unity.Canvas
{
	internal interface ICanvasFacebookImplementation : ICanvasFacebook, IPayFacebook, IFacebook, ICanvasFacebookResultHandler, IFacebookResultHandler
	{
	}
}
