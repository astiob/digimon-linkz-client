using System;

namespace CharacterDetailsUI
{
	public interface ICharacterDetailsUIAnimation
	{
		void OnOpenWindow();

		void OnCloseWindow();

		void OnOpenMenu();

		void OnCloseMenu();

		void StartAnimation();
	}
}
