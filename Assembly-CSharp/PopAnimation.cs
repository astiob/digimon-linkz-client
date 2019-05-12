using System;
using UnityEngine;

public class PopAnimation : MonoBehaviour
{
	[SerializeField]
	private string[] spriteNames = new string[0];

	[SerializeField]
	private UISprite popSprite;

	public float spriteIndex;

	private int preSpriteIndex;

	private void Start()
	{
		this.Initialize();
	}

	private void Update()
	{
		if ((int)this.spriteIndex != this.preSpriteIndex)
		{
			if (this.spriteIndex < 0f || this.spriteIndex >= (float)this.spriteNames.Length)
			{
				this.spriteIndex = 0f;
			}
			this.ChangeSprite();
		}
	}

	private void Initialize()
	{
		this.spriteIndex = 0f;
		this.ChangeSprite();
	}

	private void ChangeSprite()
	{
		this.preSpriteIndex = (int)this.spriteIndex;
		this.popSprite.spriteName = this.spriteNames[this.preSpriteIndex];
	}
}
