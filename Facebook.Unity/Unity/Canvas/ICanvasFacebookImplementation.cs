using System;

namespace Facebook.Unity.Canvas
{
	internal interface ICanvasFacebookImplementation : ICanvasFacebook, ICanvasFacebookResultHandler, IPayFacebook, IFacebook, IFacebookResultHandler
	{
	}
}
