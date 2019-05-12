using System;
using System.Collections;

namespace System.Diagnostics
{
	internal class TraceImpl
	{
		private static object initLock = new object();

		private static bool autoFlush;

		[ThreadStatic]
		private static int indentLevel = 0;

		[ThreadStatic]
		private static int indentSize;

		private static TraceListenerCollection listeners;

		private static bool use_global_lock;

		private static CorrelationManager correlation_manager = new CorrelationManager();

		private TraceImpl()
		{
		}

		public static bool AutoFlush
		{
			get
			{
				TraceImpl.InitOnce();
				return TraceImpl.autoFlush;
			}
			set
			{
				TraceImpl.InitOnce();
				TraceImpl.autoFlush = value;
			}
		}

		public static int IndentLevel
		{
			get
			{
				TraceImpl.InitOnce();
				return TraceImpl.indentLevel;
			}
			set
			{
				object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
				lock (listenersSyncRoot)
				{
					TraceImpl.indentLevel = value;
					foreach (object obj in TraceImpl.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj;
						traceListener.IndentLevel = TraceImpl.indentLevel;
					}
				}
			}
		}

		public static int IndentSize
		{
			get
			{
				TraceImpl.InitOnce();
				return TraceImpl.indentSize;
			}
			set
			{
				object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
				lock (listenersSyncRoot)
				{
					TraceImpl.indentSize = value;
					foreach (object obj in TraceImpl.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj;
						traceListener.IndentSize = TraceImpl.indentSize;
					}
				}
			}
		}

		public static TraceListenerCollection Listeners
		{
			get
			{
				TraceImpl.InitOnce();
				return TraceImpl.listeners;
			}
		}

		private static object ListenersSyncRoot
		{
			get
			{
				return ((ICollection)TraceImpl.Listeners).SyncRoot;
			}
		}

		public static CorrelationManager CorrelationManager
		{
			get
			{
				TraceImpl.InitOnce();
				return TraceImpl.correlation_manager;
			}
		}

		[MonoLimitation("the property exists but it does nothing.")]
		public static bool UseGlobalLock
		{
			get
			{
				TraceImpl.InitOnce();
				return TraceImpl.use_global_lock;
			}
			set
			{
				TraceImpl.InitOnce();
				TraceImpl.use_global_lock = value;
			}
		}

		private static void InitOnce()
		{
			if (TraceImpl.initLock != null)
			{
				object obj = TraceImpl.initLock;
				lock (obj)
				{
					if (TraceImpl.listeners == null)
					{
						TraceImplSettings traceImplSettings = new TraceImplSettings();
						TraceImpl.autoFlush = traceImplSettings.AutoFlush;
						TraceImpl.indentLevel = traceImplSettings.IndentLevel;
						TraceImpl.indentSize = traceImplSettings.IndentSize;
						TraceImpl.listeners = traceImplSettings.Listeners;
					}
				}
				TraceImpl.initLock = null;
			}
		}

		[MonoTODO]
		public static void Assert(bool condition)
		{
			if (!condition)
			{
				TraceImpl.Fail(new StackTrace(true).ToString());
			}
		}

		[MonoTODO]
		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				TraceImpl.Fail(message);
			}
		}

		[MonoTODO]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			if (!condition)
			{
				TraceImpl.Fail(message, detailMessage);
			}
		}

		public static void Close()
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.Close();
				}
			}
		}

		[MonoTODO]
		public static void Fail(string message)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.Fail(message);
				}
			}
		}

		[MonoTODO]
		public static void Fail(string message, string detailMessage)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.Fail(message, detailMessage);
				}
			}
		}

		public static void Flush()
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.Flush();
				}
			}
		}

		public static void Indent()
		{
			TraceImpl.IndentLevel++;
		}

		public static void Unindent()
		{
			TraceImpl.IndentLevel--;
		}

		public static void Write(object value)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.Write(value);
					if (TraceImpl.AutoFlush)
					{
						traceListener.Flush();
					}
				}
			}
		}

		public static void Write(string message)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.Write(message);
					if (TraceImpl.AutoFlush)
					{
						traceListener.Flush();
					}
				}
			}
		}

		public static void Write(object value, string category)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.Write(value, category);
					if (TraceImpl.AutoFlush)
					{
						traceListener.Flush();
					}
				}
			}
		}

		public static void Write(string message, string category)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.Write(message, category);
					if (TraceImpl.AutoFlush)
					{
						traceListener.Flush();
					}
				}
			}
		}

		public static void WriteIf(bool condition, object value)
		{
			if (condition)
			{
				TraceImpl.Write(value);
			}
		}

		public static void WriteIf(bool condition, string message)
		{
			if (condition)
			{
				TraceImpl.Write(message);
			}
		}

		public static void WriteIf(bool condition, object value, string category)
		{
			if (condition)
			{
				TraceImpl.Write(value, category);
			}
		}

		public static void WriteIf(bool condition, string message, string category)
		{
			if (condition)
			{
				TraceImpl.Write(message, category);
			}
		}

		public static void WriteLine(object value)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.WriteLine(value);
					if (TraceImpl.AutoFlush)
					{
						traceListener.Flush();
					}
				}
			}
		}

		public static void WriteLine(string message)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.WriteLine(message);
					if (TraceImpl.AutoFlush)
					{
						traceListener.Flush();
					}
				}
			}
		}

		public static void WriteLine(object value, string category)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.WriteLine(value, category);
					if (TraceImpl.AutoFlush)
					{
						traceListener.Flush();
					}
				}
			}
		}

		public static void WriteLine(string message, string category)
		{
			object listenersSyncRoot = TraceImpl.ListenersSyncRoot;
			lock (listenersSyncRoot)
			{
				foreach (object obj in TraceImpl.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj;
					traceListener.WriteLine(message, category);
					if (TraceImpl.AutoFlush)
					{
						traceListener.Flush();
					}
				}
			}
		}

		public static void WriteLineIf(bool condition, object value)
		{
			if (condition)
			{
				TraceImpl.WriteLine(value);
			}
		}

		public static void WriteLineIf(bool condition, string message)
		{
			if (condition)
			{
				TraceImpl.WriteLine(message);
			}
		}

		public static void WriteLineIf(bool condition, object value, string category)
		{
			if (condition)
			{
				TraceImpl.WriteLine(value, category);
			}
		}

		public static void WriteLineIf(bool condition, string message, string category)
		{
			if (condition)
			{
				TraceImpl.WriteLine(message, category);
			}
		}
	}
}
