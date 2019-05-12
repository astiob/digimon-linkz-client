using System;
using UnityEngine;

public class BattleAlways : MonoBehaviour
{
	[NonSerialized]
	public GameObject helpDialogGO;

	[NonSerialized]
	public BattleMenu battleMenu;

	[Header("Menuボタンのコライダー")]
	[SerializeField]
	protected Collider menuCollider;

	[Header("Autoのコライダー")]
	[SerializeField]
	protected Collider autoCollider;

	[Header("Skipのコライダー")]
	[SerializeField]
	private Collider skipCollider;

	public void SetColliderActive(bool isEnable)
	{
		this.menuCollider.enabled = isEnable;
		this.autoCollider.enabled = isEnable;
		this.skipCollider.enabled = isEnable;
	}

	public MenuDialog SetupMenu()
	{
		GameObject uibattlePrefab = ResourcesPath.GetUIBattlePrefab("Menu");
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(uibattlePrefab);
		Transform transform = gameObject.transform;
		BattleMenu component = transform.GetComponent<BattleMenu>();
		this.helpDialogGO = component.helpDialogGO;
		this.battleMenu = component;
		return component.SetupMenu(base.transform);
	}
}
