using System;
using System.Collections.Generic;
using UnityEngine;

public class PartsMatchingWaitMonsInfo : MonoBehaviour
{
	[SerializeField]
	[Header("RenderTarget用テクスチャ")]
	private UITexture ngTargetTex;

	private RenderTexture renderTex;

	private CommonRender3DPartyRT csRender3DPartyRT;

	private Vector3 v3Chara;

	public List<MonsterData> mdList { get; set; }

	private void OnDestroy()
	{
		this.ReleaseCharacter();
	}

	public void ShowGUI()
	{
		this.v3Chara = CMD_PvPMatchingWait.instance.Get3DPos();
		GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DPartyRT", null);
		this.csRender3DPartyRT = gameObject.GetComponent<CommonRender3DPartyRT>();
		List<string> list = new List<string>();
		foreach (MonsterData md in this.mdList)
		{
			string monsterCharaPathByMonsterData = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterData(md);
			list.Add(monsterCharaPathByMonsterData);
		}
		this.csRender3DPartyRT.LoadCharas(list, this.v3Chara.x, this.v3Chara.y);
		this.renderTex = this.csRender3DPartyRT.SetRenderTarget(1136, 820, 16);
		this.ngTargetTex.gameObject.SetActive(true);
		this.ngTargetTex.mainTexture = this.renderTex;
		this.csRender3DPartyRT.SetAnimation(CharacterAnimationType.attacks);
	}

	private void ReleaseCharacter()
	{
		if (this.csRender3DPartyRT != null)
		{
			UnityEngine.Object.Destroy(this.csRender3DPartyRT.gameObject);
			this.csRender3DPartyRT = null;
			this.renderTex = null;
			this.ngTargetTex.mainTexture = null;
		}
	}

	public void SetAnimation(CharacterAnimationType anim)
	{
		this.csRender3DPartyRT.SetAnimation(anim);
	}

	public GameObject GetChara(int index)
	{
		return this.csRender3DPartyRT.GetChara(index);
	}

	public void HideChara()
	{
		if (this.ngTargetTex != null)
		{
			for (int i = 0; i < this.mdList.Count; i++)
			{
				GameObject chara = this.csRender3DPartyRT.GetChara(i);
				chara.SetActive(false);
			}
		}
	}

	public void SetRenderTextureBackgroundColor(Color color)
	{
		if (null != this.csRender3DPartyRT)
		{
			this.csRender3DPartyRT.SetCameraBackgroundColor(color);
		}
	}
}
