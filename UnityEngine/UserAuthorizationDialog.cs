using System;

namespace UnityEngine
{
	[AddComponentMenu("")]
	internal class UserAuthorizationDialog : MonoBehaviour
	{
		private const int width = 385;

		private const int height = 155;

		private Rect windowRect;

		private Texture warningIcon;

		private void Start()
		{
			this.warningIcon = (Resources.GetBuiltinResource(typeof(Texture2D), "WarningSign.psd") as Texture2D);
			if (Screen.width < 385 || Screen.height < 155)
			{
				Debug.LogError("Screen is to small to display authorization dialog. Authorization denied.");
				Application.ReplyToUserAuthorizationRequest(false);
			}
			this.windowRect = new Rect((float)(Screen.width / 2 - 192), (float)(Screen.height / 2 - 77), 385f, 155f);
		}

		private void OnGUI()
		{
			GUISkin skin = GUI.skin;
			GUISkin guiskin = ScriptableObject.CreateInstance("GUISkin") as GUISkin;
			guiskin.box.normal.background = (Texture2D)Resources.GetBuiltinResource(typeof(Texture2D), "GameSkin/box.png");
			guiskin.box.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
			guiskin.box.padding.left = 6;
			guiskin.box.padding.right = 6;
			guiskin.box.padding.top = 4;
			guiskin.box.padding.bottom = 4;
			guiskin.box.border.left = 6;
			guiskin.box.border.right = 6;
			guiskin.box.border.top = 6;
			guiskin.box.border.bottom = 6;
			guiskin.box.margin.left = 4;
			guiskin.box.margin.right = 4;
			guiskin.box.margin.top = 4;
			guiskin.box.margin.bottom = 4;
			guiskin.button.normal.background = (Texture2D)Resources.GetBuiltinResource(typeof(Texture2D), "GameSkin/button.png");
			guiskin.button.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
			guiskin.button.hover.background = (Texture2D)Resources.GetBuiltinResource(typeof(Texture2D), "GameSkin/button hover.png");
			guiskin.button.hover.textColor = Color.white;
			guiskin.button.active.background = (Texture2D)Resources.GetBuiltinResource(typeof(Texture2D), "GameSkin/button active.png");
			guiskin.button.active.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
			guiskin.button.border.left = 6;
			guiskin.button.border.right = 6;
			guiskin.button.border.top = 6;
			guiskin.button.border.bottom = 6;
			guiskin.button.padding.left = 8;
			guiskin.button.padding.right = 8;
			guiskin.button.padding.top = 4;
			guiskin.button.padding.bottom = 4;
			guiskin.button.margin.left = 4;
			guiskin.button.margin.right = 4;
			guiskin.button.margin.top = 4;
			guiskin.button.margin.bottom = 4;
			guiskin.label.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
			guiskin.label.padding.left = 6;
			guiskin.label.padding.right = 6;
			guiskin.label.padding.top = 4;
			guiskin.label.padding.bottom = 4;
			guiskin.label.margin.left = 4;
			guiskin.label.margin.right = 4;
			guiskin.label.margin.top = 4;
			guiskin.label.margin.bottom = 4;
			guiskin.label.alignment = TextAnchor.UpperLeft;
			guiskin.window.normal.background = (Texture2D)Resources.GetBuiltinResource(typeof(Texture2D), "GameSkin/window.png");
			guiskin.window.normal.textColor = Color.white;
			guiskin.window.border.left = 8;
			guiskin.window.border.right = 8;
			guiskin.window.border.top = 18;
			guiskin.window.border.bottom = 8;
			guiskin.window.padding.left = 8;
			guiskin.window.padding.right = 8;
			guiskin.window.padding.top = 20;
			guiskin.window.padding.bottom = 5;
			guiskin.window.alignment = TextAnchor.UpperCenter;
			guiskin.window.contentOffset = new Vector2(0f, -18f);
			GUI.skin = guiskin;
			this.windowRect = GUI.Window(0, this.windowRect, new GUI.WindowFunction(this.DoUserAuthorizationDialog), "Unity Web Player Authorization Request");
			GUI.skin = skin;
		}

		private void DoUserAuthorizationDialog(int windowID)
		{
			UserAuthorization userAuthorizationRequestMode = Application.GetUserAuthorizationRequestMode();
			GUILayout.FlexibleSpace();
			GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.7f);
			GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.Label(this.warningIcon, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			if (userAuthorizationRequestMode == (UserAuthorization)3)
			{
				GUILayout.Label("The content on this site would like to use your\ncomputer's web camera and microphone.\nThese images and sounds may be recorded.", new GUILayoutOption[0]);
			}
			else if (userAuthorizationRequestMode == UserAuthorization.WebCam)
			{
				GUILayout.Label("The content on this site would like to use\nyour computer's web camera. The images\nfrom your web camera may be recorded.", new GUILayoutOption[0]);
			}
			else
			{
				if (userAuthorizationRequestMode != UserAuthorization.Microphone)
				{
					return;
				}
				GUILayout.Label("The content on this site would like to use\nyour computer's microphone. The sounds\nfrom your microphone may be recorded.", new GUILayoutOption[0]);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUI.backgroundColor = Color.white;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Deny", new GUILayoutOption[0]))
			{
				Application.ReplyToUserAuthorizationRequest(false);
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Always Allow for this Site", new GUILayoutOption[0]))
			{
				Application.ReplyToUserAuthorizationRequest(true, true);
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Allow", new GUILayoutOption[0]))
			{
				Application.ReplyToUserAuthorizationRequest(true);
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
	}
}
