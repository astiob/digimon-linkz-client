using System;
using System.IO;

namespace UnityEngine.Advertisements
{
	internal class UnityAdsEditorPlaceholder : MonoBehaviour
	{
		private Texture2D m_PlaceholderLandscape;

		private Texture2D m_PlaceholderPortrait;

		private bool m_Showing;

		private static Texture2D TextureFromFile(string path)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.LoadImage(File.ReadAllBytes(path));
			return texture2D;
		}

		public void Load(string extensionPath)
		{
			this.m_PlaceholderLandscape = UnityAdsEditorPlaceholder.TextureFromFile(Path.Combine(extensionPath, "Editor/Resources/Editor/test_unit_800.png"));
			this.m_PlaceholderPortrait = UnityAdsEditorPlaceholder.TextureFromFile(Path.Combine(extensionPath, "Editor/Resources/Editor/test_unit_600.png"));
		}

		private void WindowFunc(int id)
		{
			Rect position = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
			GUI.DrawTexture(position, Texture2D.blackTexture, ScaleMode.StretchToFill, false);
			if (Screen.width > Screen.height)
			{
				GUI.DrawTexture(position, this.m_PlaceholderLandscape, ScaleMode.ScaleAndCrop);
			}
			else
			{
				GUI.DrawTexture(position, this.m_PlaceholderPortrait, ScaleMode.ScaleAndCrop);
			}
			if (GUI.Button(new Rect((float)(Screen.width - 160), 10f, 150f, 50f), "Close"))
			{
				this.Hide();
			}
		}

		public void OnGUI()
		{
			if (!this.m_Showing)
			{
				return;
			}
			GUI.ModalWindow(0, new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), new GUI.WindowFunction(this.WindowFunc), string.Empty);
		}

		public void Show()
		{
			this.m_Showing = true;
		}

		public void Hide()
		{
			UnityAdsInternal.CallUnityAdsVideoCompleted(null, false);
			UnityAdsInternal.CallUnityAdsHide();
			this.m_Showing = false;
		}
	}
}
