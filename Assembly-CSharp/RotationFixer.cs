using System;
using UnityEngine;

[AddComponentMenu("Digimon Effects/Tools/Rotation Fixer")]
public class RotationFixer : MonoBehaviour
{
	[SerializeField]
	private bool _xFix;

	[SerializeField]
	private bool _yFix;

	[SerializeField]
	private bool _zFix;

	private Transform cTransform;

	private void Awake()
	{
		this.cTransform = base.transform;
	}

	public void SetRotation(CharacterParams character)
	{
		Vector3 eulerAngles = character.transform.eulerAngles;
		Vector3 eulerAngles2 = this.cTransform.rotation.eulerAngles;
		this.cTransform.rotation = Quaternion.Euler(new Vector3((!this._xFix) ? eulerAngles2.x : eulerAngles.x, (!this._yFix) ? eulerAngles2.y : eulerAngles.y, (!this._zFix) ? eulerAngles2.z : eulerAngles.z));
	}
}
