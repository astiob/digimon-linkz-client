using System;

namespace DebugMonitor
{
	public static class ApplicationMonitorModeList
	{
		private static ApplicationMonitorMode Create(MonitorMode mode, string title, string suffix)
		{
			return new ApplicationMonitorMode
			{
				mode = mode,
				title = title,
				suffix = suffix
			};
		}

		public static ApplicationMonitorMode[] CreateList()
		{
			return new ApplicationMonitorMode[]
			{
				ApplicationMonitorModeList.Create(MonitorMode.USE_MEMORY_PERCENT, "Mem:", "%"),
				ApplicationMonitorModeList.Create(MonitorMode.PEEK_USE_MEMORY_PERCENT, "Peek:", "%"),
				ApplicationMonitorModeList.Create(MonitorMode.HEAP_MEMORY, "Heap:", "mb"),
				ApplicationMonitorModeList.Create(MonitorMode.RESERVED_MEMORY, "Total:", "mb"),
				ApplicationMonitorModeList.Create(MonitorMode.FPS, "FPS:", "fps"),
				ApplicationMonitorModeList.Create(MonitorMode.JSON, "Json Parse Error", string.Empty),
				ApplicationMonitorModeList.Create(MonitorMode.PLATFORM_USER_ID, "Platform User Id", string.Empty),
				ApplicationMonitorModeList.Create(MonitorMode.API_LOG, "API", "Log"),
				ApplicationMonitorModeList.Create(MonitorMode.PvP, "PvP", string.Empty)
			};
		}
	}
}
