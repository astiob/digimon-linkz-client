using System;
using UnityEngine;

namespace DebugMonitor
{
	public sealed class ApplicationMonitor : MonoBehaviour
	{
		private ApplicationMonitorMode[] monitorModeList;

		[SerializeField]
		private float alertUseMemoryPercent = 90f;

		[SerializeField]
		private float updateIntervalTime = 0.5f;

		private GUIStyleState styleState = new GUIStyleState();

		private GUIStyle fontStyle = new GUIStyle();

		private string displayText = string.Empty;

		private Rect displayRect = default(Rect);

		private bool isDisplay;

		private MonitorMode displayMode;

		private string titleText = string.Empty;

		private string counterSuffixText = string.Empty;

		private float peekUseMemoryPercent;

		private bool isLock;

		private int frameCount;

		private float elapsedTime;

		private void Start()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void Update()
		{
		}

		private void OnGUI()
		{
			if (!this.isDisplay)
			{
				return;
			}
			if (GUI.Button(this.displayRect, this.titleText + this.displayText + this.counterSuffixText, this.fontStyle))
			{
				this.ChangeMode();
				this.ChangeTextColor(Color.white);
			}
		}

		private void Initialize()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.displayRect = new Rect(new Vector2((float)Screen.width / 2f, 10f), new Vector2((float)Screen.width / 10f, (float)Screen.height / 10f));
			this.ChangeTextColor(Color.white);
			this.fontStyle.fontSize = (int)this.displayRect.size.y;
			this.ChangeMode(this.displayMode);
		}

		private void Monitoring()
		{
			switch (this.displayMode)
			{
			case MonitorMode.USE_MEMORY_PERCENT:
			case MonitorMode.PEEK_USE_MEMORY_PERCENT:
			case MonitorMode.HEAP_MEMORY:
			case MonitorMode.RESERVED_MEMORY:
				this.MonitoringMemory();
				break;
			case MonitorMode.FPS:
				this.MonitoringFPS();
				break;
			}
		}

		private void MonitoringMemory()
		{
			float num = Profiler.usedHeapSize;
			float num2 = Profiler.GetTotalReservedMemory();
			float num3 = num / num2 * 100f;
			if (this.peekUseMemoryPercent < num3)
			{
				this.peekUseMemoryPercent = num3;
			}
			switch (this.displayMode)
			{
			case MonitorMode.USE_MEMORY_PERCENT:
				this.displayText = num3.ToString("0.00");
				this.ChangeTextColor((this.alertUseMemoryPercent >= num3) ? Color.white : Color.red);
				break;
			case MonitorMode.PEEK_USE_MEMORY_PERCENT:
				this.displayText = this.peekUseMemoryPercent.ToString("0.00");
				this.ChangeTextColor((this.alertUseMemoryPercent >= this.peekUseMemoryPercent) ? Color.white : Color.red);
				break;
			case MonitorMode.HEAP_MEMORY:
				this.displayText = (num / 1048576f).ToString("0.00");
				break;
			case MonitorMode.RESERVED_MEMORY:
				this.displayText = (num2 / 1048576f).ToString("0.00");
				break;
			}
		}

		private void MonitoringFPS()
		{
			this.displayText = ((float)this.frameCount / this.elapsedTime).ToString("0.0");
		}

		private void ChangeTextColor(Color TextColor)
		{
			this.styleState.textColor = TextColor;
			this.fontStyle.normal = this.styleState;
		}

		private void ChangeMode()
		{
			this.displayMode++;
			if (this.displayMode >= (MonitorMode)this.monitorModeList.Length)
			{
				this.displayMode = MonitorMode.USE_MEMORY_PERCENT;
			}
			this.ChangeMode(this.displayMode);
		}

		private void ChangeMode(MonitorMode setMode)
		{
			this.displayMode = setMode;
			this.titleText = this.monitorModeList[(int)setMode].title;
			this.displayText = string.Empty;
			this.counterSuffixText = this.monitorModeList[(int)setMode].suffix;
		}
	}
}
