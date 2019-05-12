using System;
using UnityEngine;

public class BattleResultLuckUserInfo : MonoBehaviour
{
	[SerializeField]
	private UILabel TXT_NAME;

	[SerializeField]
	private UILabel TXT_LUCK;

	[SerializeField]
	private UISprite arrowSprite;

	[SerializeField]
	private Transform arrowTransform;

	[SerializeField]
	private Transform iconAnchor;

	public void SetStatusInfo(string user_name, int leader_luck, string leader_monster_id, Transform target)
	{
		this.TXT_NAME.text = user_name;
		this.TXT_LUCK.text = leader_luck.ToString();
		MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(leader_monster_id);
		GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, this.iconAnchor.localScale, this.iconAnchor.localPosition, this.iconAnchor.parent, true, false);
		guimonsterIcon.name = "DigimonIcon";
		Vector3 localPosition = this.arrowSprite.transform.localPosition;
		Vector3 vector = this.arrowSprite.transform.parent.InverseTransformPoint(target.position);
		float num = Vector2.Distance(new Vector2(localPosition.x, localPosition.y), new Vector2(vector.x, vector.y));
		Vector2 v = new Vector2(vector.x, vector.y) - new Vector2(localPosition.x, localPosition.y);
		float z = Quaternion.FromToRotation(Vector3.up, v).eulerAngles.z;
		this.arrowSprite.height = (int)num - 50;
		this.arrowTransform.Rotate(Vector3.forward, z);
	}
}
