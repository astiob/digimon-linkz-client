using System;
using UnityEngine;

public class RadioWave : MonoBehaviour
{
	[SerializeField]
	private UILabel label;

	[SerializeField]
	private UISprite sprite;

	private void SetLevel()
	{
	}

	private void OnGUI()
	{
		GUILayout.Label("Player ping values", new GUILayoutOption[0]);
		for (int i = 0; i < Network.connections.Length; i++)
		{
			GUILayout.Label(string.Concat(new object[]
			{
				"Player ",
				Network.connections[i],
				" - ",
				Network.GetAveragePing(Network.connections[i]),
				" ms"
			}), new GUILayoutOption[0]);
		}
	}
}
