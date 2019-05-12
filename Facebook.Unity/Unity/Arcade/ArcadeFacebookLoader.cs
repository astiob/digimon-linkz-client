using System;

namespace Facebook.Unity.Arcade
{
	internal class ArcadeFacebookLoader : FB.CompiledFacebookLoader
	{
		protected override FacebookGameObject FBGameObject
		{
			get
			{
				ArcadeFacebookGameObject component = ComponentFactory.GetComponent<ArcadeFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
				if (component.Facebook == null)
				{
					component.Facebook = new ArcadeFacebook();
				}
				return component;
			}
		}
	}
}
