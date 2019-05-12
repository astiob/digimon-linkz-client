using System;

public class BtnFareWell : GUICollider
{
	public override void PlayOkSE()
	{
		SoundMng.Instance().PlaySE("SEInternal/Farm/se_209", 0f, false, true, null, -1, 1f);
	}
}
