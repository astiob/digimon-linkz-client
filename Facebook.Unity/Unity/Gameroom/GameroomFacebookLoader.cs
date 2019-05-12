using System;

namespace Facebook.Unity.Gameroom
{
	internal class GameroomFacebookLoader : FB.CompiledFacebookLoader
	{
		protected override FacebookGameObject FBGameObject
		{
			get
			{
				GameroomFacebookGameObject component = ComponentFactory.GetComponent<GameroomFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
				if (component.Facebook == null)
				{
					component.Facebook = new GameroomFacebook();
				}
				return component;
			}
		}
	}
}
