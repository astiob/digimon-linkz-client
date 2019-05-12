using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Firebase.Platform
{
	internal sealed class FirebaseEditorDispatcher
	{
		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		[CompilerGenerated]
		private static Action <>f__mg$cache1;

		[CompilerGenerated]
		private static Action <>f__mg$cache2;

		private static Type EditorApplicationType
		{
			get
			{
				return Type.GetType("UnityEditor.EditorApplication, UnityEditor");
			}
		}

		public static bool EditorIsPlaying
		{
			get
			{
				Type editorApplicationType = FirebaseEditorDispatcher.EditorApplicationType;
				if (editorApplicationType != null)
				{
					PropertyInfo property = editorApplicationType.GetProperty("isPlaying");
					if (property != null)
					{
						return (bool)property.GetValue(null, null);
					}
				}
				return true;
			}
		}

		public static bool EditorIsPlayingOrWillChangePlaymode
		{
			get
			{
				Type editorApplicationType = FirebaseEditorDispatcher.EditorApplicationType;
				if (editorApplicationType != null)
				{
					PropertyInfo property = editorApplicationType.GetProperty("isPlayingOrWillChangePlaymode");
					if (property != null)
					{
						return (bool)property.GetValue(null, null);
					}
				}
				return true;
			}
		}

		public static void StartEditorUpdate()
		{
			Type editorApplicationType = FirebaseEditorDispatcher.EditorApplicationType;
			if (editorApplicationType != null)
			{
				FieldInfo field = editorApplicationType.GetField("update");
				if (FirebaseEditorDispatcher.<>f__mg$cache0 == null)
				{
					FirebaseEditorDispatcher.<>f__mg$cache0 = new Action(FirebaseEditorDispatcher.Update);
				}
				FirebaseEditorDispatcher.AddRemoveCallbackToField(field, FirebaseEditorDispatcher.<>f__mg$cache0, null, true, "Firebase failed to register for editor update calls. Most Firebase features will fail, as callbacks will notwork properly. This is caused by being unable to resolvethe necessary fields from the UnityEditor.dll.");
			}
		}

		public static void StopEditorUpdate()
		{
			Type editorApplicationType = FirebaseEditorDispatcher.EditorApplicationType;
			if (editorApplicationType != null)
			{
				FieldInfo field = editorApplicationType.GetField("update");
				if (FirebaseEditorDispatcher.<>f__mg$cache1 == null)
				{
					FirebaseEditorDispatcher.<>f__mg$cache1 = new Action(FirebaseEditorDispatcher.Update);
				}
				FirebaseEditorDispatcher.AddRemoveCallbackToField(field, FirebaseEditorDispatcher.<>f__mg$cache1, null, false, null);
			}
		}

		public static void Update()
		{
			FirebaseHandler.DefaultInstance.Update();
		}

		public static void ListenToPlayState(bool start = true)
		{
			Type editorApplicationType = FirebaseEditorDispatcher.EditorApplicationType;
			if (editorApplicationType != null)
			{
				EventInfo @event = editorApplicationType.GetEvent("playModeStateChanged");
				if (@event != null)
				{
					Type type = Type.GetType("UnityEditor.PlayModeStateChange, UnityEditor");
					if (type != null)
					{
						MethodInfo methodInfo = typeof(FirebaseEditorDispatcher).GetMethod("PlayModeStateChangedWithArg", BindingFlags.Static | BindingFlags.NonPublic);
						methodInfo = methodInfo.MakeGenericMethod(new Type[]
						{
							type
						});
						Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, null, methodInfo);
						if (start)
						{
							@event.AddEventHandler(null, handler);
						}
						else
						{
							@event.RemoveEventHandler(null, handler);
						}
						return;
					}
				}
				FieldInfo field = editorApplicationType.GetField("playmodeStateChanged");
				if (FirebaseEditorDispatcher.<>f__mg$cache2 == null)
				{
					FirebaseEditorDispatcher.<>f__mg$cache2 = new Action(FirebaseEditorDispatcher.PlayModeStateChanged);
				}
				Action callback = FirebaseEditorDispatcher.<>f__mg$cache2;
				FirebaseEditorDispatcher.AddRemoveCallbackToField(field, callback, null, start, null);
			}
		}

		private static void PlayModeStateChanged()
		{
			if (!FirebaseHandler.DefaultInstance.IsPlayMode && FirebaseEditorDispatcher.EditorIsPlaying)
			{
				FirebaseEditorDispatcher.StopEditorUpdate();
				FirebaseHandler.DefaultInstance.StartMonoBehaviour();
				FirebaseHandler.DefaultInstance.IsPlayMode = true;
			}
			else if (FirebaseHandler.DefaultInstance.IsPlayMode && !FirebaseEditorDispatcher.EditorIsPlayingOrWillChangePlaymode)
			{
				FirebaseHandler.DefaultInstance.StopMonoBehaviour();
				FirebaseEditorDispatcher.StartEditorUpdate();
				FirebaseHandler.DefaultInstance.IsPlayMode = false;
			}
		}

		private static void PlayModeStateChangedWithArg<T>(T t)
		{
			FirebaseEditorDispatcher.PlayModeStateChanged();
		}

		private static void AddRemoveCallbackToField(FieldInfo eventField, Action callback, object target = null, bool add = true, string errorMessage = null)
		{
			if (eventField != null)
			{
				Delegate @delegate = eventField.GetValue(null) as Delegate;
				if (add)
				{
					Delegate delegate2 = Delegate.CreateDelegate(eventField.FieldType, target, callback.Method);
					if (@delegate != null)
					{
						eventField.SetValue(null, Delegate.Combine(@delegate, delegate2));
					}
					else
					{
						eventField.SetValue(null, delegate2);
					}
					return;
				}
				if (@delegate != null)
				{
					Delegate value = Delegate.CreateDelegate(eventField.FieldType, target, callback.Method);
					eventField.SetValue(null, Delegate.Remove(@delegate, value));
					return;
				}
			}
			if (!string.IsNullOrEmpty(errorMessage))
			{
				FirebaseLogger.LogMessage(PlatformLogLevel.Error, errorMessage);
			}
		}

		public static void Terminate(bool isPlayMode)
		{
			FirebaseEditorDispatcher.ListenToPlayState(false);
			if (!isPlayMode)
			{
				FirebaseEditorDispatcher.StopEditorUpdate();
			}
		}
	}
}
