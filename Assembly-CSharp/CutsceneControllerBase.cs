using System;
using UnityEngine;

public abstract class CutsceneControllerBase : CutsceneBase
{
	public override void Initialize()
	{
		base.Initialize();
		int layer = LayerMask.NameToLayer("UI");
		int layer2 = LayerMask.NameToLayer("Cutscene");
		Physics.IgnoreLayerCollision(layer, layer2);
	}
}
