using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Quest
{
	public sealed class QuestDetailedBonusPoint : MonoBehaviour
	{
		[SerializeField]
		private GameObject changeBonusButton;

		[SerializeField]
		private GameObject bonusBaseObj;

		[SerializeField]
		private List<UILabel> bonusPointLabels;

		private int viewPage;

		private List<string> bonusTextList = new List<string>();

		private TweenAlpha bonusTextTween;

		private bool bonusChange;

		private void OnPushBonusChange()
		{
			if (!this.bonusChange)
			{
				this.bonusChange = true;
				base.StartCoroutine(this.BonusChangeAnima());
			}
		}

		private IEnumerator BonusChangeAnima()
		{
			this.bonusTextTween = this.bonusBaseObj.AddComponent<TweenAlpha>();
			this.bonusTextTween.from = 1f;
			this.bonusTextTween.to = 0f;
			this.bonusTextTween.duration = 0.2f;
			this.bonusTextTween.PlayForward();
			yield return new WaitForSeconds(0.2f);
			this.viewPage++;
			int count = this.bonusTextList.Count / this.bonusPointLabels.Count;
			if (this.bonusTextList.Count % this.bonusPointLabels.Count != 0)
			{
				count++;
			}
			int viewNum = this.viewPage % count;
			for (int i = 0; i < this.bonusPointLabels.Count; i++)
			{
				this.bonusPointLabels[i].gameObject.SetActive(false);
				if (i + viewNum * this.bonusPointLabels.Count < this.bonusTextList.Count)
				{
					this.bonusPointLabels[i].gameObject.SetActive(true);
					this.bonusPointLabels[i].text = this.bonusTextList[i + viewNum * this.bonusPointLabels.Count];
				}
			}
			UnityEngine.Object.Destroy(this.bonusTextTween);
			this.bonusTextTween = null;
			this.bonusTextTween = this.bonusBaseObj.AddComponent<TweenAlpha>();
			this.bonusTextTween.from = 0f;
			this.bonusTextTween.to = 1f;
			this.bonusTextTween.duration = 0.2f;
			this.bonusTextTween.PlayForward();
			yield return new WaitForSeconds(0.2f);
			this.bonusChange = false;
			yield break;
		}

		public void Initialize(string areaId, string stageId, string dungeonId)
		{
			QuestBonusPack questBonusPack = new QuestBonusPack();
			questBonusPack.CreateQuestBonus(areaId, stageId, dungeonId);
			this.bonusTextList = QuestBonus.GetBonusText(questBonusPack.bonusChipIds, questBonusPack.eventBonuses, questBonusPack.dungeonBonuses);
		}

		public void SetBonusUI()
		{
			bool active = this.bonusTextList.Count > this.bonusPointLabels.Count;
			this.changeBonusButton.SetActive(active);
			if (this.bonusTextList.Count == 0)
			{
				this.bonusTextList.Add(StringMaster.GetString("QuestNonSpList"));
			}
			for (int i = 0; i < this.bonusPointLabels.Count; i++)
			{
				this.bonusPointLabels[i].gameObject.SetActive(false);
				if (i < this.bonusTextList.Count)
				{
					this.bonusPointLabels[i].gameObject.SetActive(true);
					this.bonusPointLabels[i].text = this.bonusTextList[i];
				}
			}
		}
	}
}
