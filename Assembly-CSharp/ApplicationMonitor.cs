using System;
using UnityEngine;

public class ApplicationMonitor : MonoBehaviour
{
	private readonly int lenghtEnumMODE = Enum.GetValues(typeof(ApplicationMonitor.MODE)).Length;

	private readonly string[] titles = new string[]
	{
		"Mem:",
		"Peek:",
		"Heap:",
		"Total:",
		"FPS:",
		"Api"
	};

	private readonly string[] counterSuffixs = new string[]
	{
		"%",
		"%",
		"mb",
		"mb",
		"fps",
		"Log"
	};

	[SerializeField]
	private float alertUseMemoryPercent = 90f;

	[SerializeField]
	private float updateIntervalTime = 0.5f;

	private GUIStyleState styleState = new GUIStyleState();

	private GUIStyle fontStyle = new GUIStyle();

	private string displayText = string.Empty;

	private Rect displayRect = default(Rect);

	private bool isDisplay;

	private ApplicationMonitor.MODE displayMode;

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
		if ((Input.touchCount >= 3 || Input.GetMouseButtonDown(2)) && !this.isLock)
		{
			this.isDisplay = !this.isDisplay;
			this.isLock = true;
			this.frameCount = 0;
			this.elapsedTime = 0f;
		}
		if (this.isLock && ((Input.GetMouseButtonUp(0) && Input.touchCount < 3) || Input.GetMouseButtonUp(2)))
		{
			this.isLock = false;
		}
		if (this.isDisplay)
		{
			this.frameCount++;
			this.elapsedTime += Time.deltaTime;
			if (this.elapsedTime >= this.updateIntervalTime)
			{
				this.Monitoring();
				this.frameCount = 0;
				this.elapsedTime = 0f;
			}
		}
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
		case ApplicationMonitor.MODE.UseMemoryPercent:
			this.MonitoringMemory();
			break;
		case ApplicationMonitor.MODE.PeekUseMemoryPercent:
			this.MonitoringMemory();
			break;
		case ApplicationMonitor.MODE.HeapMemory:
			this.MonitoringMemory();
			break;
		case ApplicationMonitor.MODE.ReservedMemory:
			this.MonitoringMemory();
			break;
		case ApplicationMonitor.MODE.FPS:
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
		case ApplicationMonitor.MODE.UseMemoryPercent:
			this.displayText = num3.ToString("0.00");
			this.ChangeTextColor((this.alertUseMemoryPercent >= num3) ? Color.white : Color.red);
			break;
		case ApplicationMonitor.MODE.PeekUseMemoryPercent:
			this.displayText = this.peekUseMemoryPercent.ToString("0.00");
			this.ChangeTextColor((this.alertUseMemoryPercent >= this.peekUseMemoryPercent) ? Color.white : Color.red);
			break;
		case ApplicationMonitor.MODE.HeapMemory:
			this.displayText = (num / 1048576f).ToString("0.00");
			break;
		case ApplicationMonitor.MODE.ReservedMemory:
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
		if (this.displayMode >= (ApplicationMonitor.MODE)this.lenghtEnumMODE)
		{
			this.displayMode = ApplicationMonitor.MODE.UseMemoryPercent;
		}
		this.ChangeMode(this.displayMode);
	}

	private void ChangeMode(ApplicationMonitor.MODE setMode)
	{
		this.displayMode = setMode;
		this.titleText = this.titles[(int)setMode];
		this.displayText = string.Empty;
		this.counterSuffixText = this.counterSuffixs[(int)setMode];
	}

	private enum MODE
	{
		UseMemoryPercent,
		PeekUseMemoryPercent,
		HeapMemory,
		ReservedMemory,
		FPS,
		ApiLog
	}
}
