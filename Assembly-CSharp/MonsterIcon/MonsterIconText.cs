using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIconText : MonoBehaviour
	{
		private UILabel state;

		[SerializeField]
		private UILabel level;

		[SerializeField]
		private UILabel sort;

		[SerializeField]
		private UILabel selectNo;

		private void Awake()
		{
			this.state = base.GetComponent<UILabel>();
		}

		public void ClearMessage()
		{
			this.state.enabled = false;
			this.level.enabled = false;
			this.sort.enabled = false;
			this.selectNo.enabled = false;
		}

		public void SetStateText(string text)
		{
			this.state.enabled = true;
			this.state.text = text;
		}

		public void SetStateTextColor(Color color)
		{
			this.state.color = color;
		}

		public void ClearStateText()
		{
			this.state.enabled = false;
		}

		public void SetLevelText(string text)
		{
			this.level.enabled = true;
			this.level.text = text;
		}

		public void ClearLevelText()
		{
			this.level.enabled = false;
		}

		public void SetSortText(string text)
		{
			this.sort.enabled = true;
			this.sort.text = text;
		}

		public void SetSortTextColor(Color color)
		{
			this.sort.color = color;
		}

		public void ClearSortText()
		{
			this.sort.enabled = false;
		}

		public void SetSelectNoText(string text)
		{
			this.selectNo.enabled = true;
			this.selectNo.text = text;
		}

		public void ClearSelectNoText()
		{
			this.selectNo.enabled = false;
		}
	}
}
