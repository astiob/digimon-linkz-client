using PersistentData;
using System;
using System.Collections.Generic;
using TS;
using TypeSerialize;
using UnityEngine;

namespace AdventureScene
{
	public class AdventureScriptEngine
	{
		private ScriptEngine scriptEngine;

		private bool finishScript;

		private AdventureScriptEngine.ScriptState scriptState;

		private Dictionary<string, AdventureBaseCommand> commandList;

		private BaseCommand runCommand;

		private bool continueCommand;

		private bool isError;

		private TutorialUI tutorialUI;

		private TutorialCommandAction tutorialCommandAction;

		public AdventureScriptEngine()
		{
			this.scriptEngine = new ScriptEngine();
			this.scriptEngine.SetExternalCommand(new Func<string[], bool>(this.ReceiveCommandParameter));
			this.commandList = new Dictionary<string, AdventureBaseCommand>();
			this.scriptState = AdventureScriptEngine.ScriptState.INIT;
			this.finishScript = false;
		}

		private void InitTutorialScriptEngine()
		{
			GameObject original = AssetDataMng.Instance().LoadObject("Tutorial/TutorialUI", null, true) as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.parent = Singleton<GUIMain>.Instance.transform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3(0f, 0f, -2000f);
			gameObject.transform.localRotation = Quaternion.identity;
			this.tutorialUI = gameObject.GetComponent<TutorialUI>();
			original = (AssetDataMng.Instance().LoadObject("Tutorial/TutorialMessageWindow", null, true) as GameObject);
			gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.parent = this.tutorialUI.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
			this.tutorialUI.SetMessageWindow(gameObject.GetComponent<TutorialMessageWindow>());
			original = (AssetDataMng.Instance().LoadObject("Tutorial/TutorialImageWindow", null, true) as GameObject);
			gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.parent = this.tutorialUI.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
			this.tutorialUI.SetImageWindow(gameObject.GetComponent<TutorialImageWindow>());
			original = (AssetDataMng.Instance().LoadObject("Tutorial/TutorialThumbnail", null, true) as GameObject);
			gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.transform.parent = this.tutorialUI.transform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localPosition = Vector3.zero;
			this.tutorialUI.SetThumbnail(gameObject.GetComponent<TutorialThumbnail>());
			TutorialControlToGame tutorialControlToGame = Singleton<TutorialObserver>.Instance.GetComponent<TutorialControlToGame>();
			if (null == tutorialControlToGame)
			{
				tutorialControlToGame = Singleton<TutorialObserver>.Instance.gameObject.AddComponent<TutorialControlToGame>();
			}
			this.tutorialCommandAction = new TutorialCommandAction(this.scriptEngine, this.tutorialUI, tutorialControlToGame);
		}

		private void OnLoadScriptFile(byte[] loadData)
		{
			string empty = string.Empty;
			try
			{
				TypeSerializeHelper.BytesToData<string>(loadData, out empty);
			}
			catch
			{
				global::Debug.Log("動画シーン用のスクリプトファイルの読み込みに失敗しました。");
			}
			this.scriptEngine.Deserialize(empty);
		}

		private bool ReceiveCommandParameter(string[] commandParams)
		{
			bool result = false;
			string key = commandParams[0];
			if (this.commandList.ContainsKey(key))
			{
				this.runCommand = this.commandList[key];
				if (this.runCommand != null)
				{
					result = this.runCommand.GetParameter(commandParams);
					global::Debug.Log("GetParam : " + this.runCommand.GetType());
				}
			}
			return result;
		}

		private void ActionScriptCommand(ScriptEngine.Status status, int commandAddress)
		{
			if (status == ScriptEngine.Status.EXTERNAL_COMMAND)
			{
				if (this.runCommand != null)
				{
					this.runCommand.commandAddress = commandAddress;
					if (!this.runCommand.RunScriptCommand())
					{
						global::Debug.LogWarning("エラー発生");
						this.isError = true;
					}
					global::Debug.Log(string.Concat(new object[]
					{
						"RunCommand (",
						commandAddress,
						") : ",
						this.runCommand.GetType()
					}));
					this.continueCommand = this.runCommand.continueAnalyze;
				}
			}
			else
			{
				this.continueCommand = false;
				this.tutorialCommandAction.ActionScriptCommand(status, commandAddress);
			}
		}

		public void UpdateScriptCommand()
		{
			if (!this.runCommand.UpdateCommand())
			{
				global::Debug.LogWarning("エラー発生");
				this.isError = true;
			}
		}

		public void Initialize()
		{
			this.SetCommand(new AdventureScriptStateCommand());
			this.SetCommand(new LoadStageCommand());
			this.SetCommand(new LoadCharaCommand());
			this.SetCommand(new LoadCharaAnimeCommand());
			this.SetCommand(new LoadCameraAnimeCommand());
			this.SetCommand(new LoadEffectCommand());
			this.SetCommand(new CharaShowCommand());
			this.SetCommand(new CharaPositionCommand());
			this.SetCommand(new CharaLocatorCommand());
			this.SetCommand(new CharaRotationCommand());
			this.SetCommand(new CharaAnimeCommand());
			this.SetCommand(new CharaAnimeSkillCommand());
			this.SetCommand(new CameraTargetCommand());
			this.SetCommand(new CameraMoveCommand());
			this.SetCommand(new CameraAnimationCommand());
			this.SetCommand(new CameraAnimationStopCommand());
			this.SetCommand(new EffectShowCommand());
			this.SetCommand(new EffectPositionCommand());
			this.SetCommand(new EffectLocatorCommand());
			this.SetCommand(new EffectRotationCommand());
			this.SetCommand(new WorldShakeCommand());
			this.InitTutorialScriptEngine();
		}

		public ScriptEngine GetScriptEngine()
		{
			return this.scriptEngine;
		}

		public void LosdScriptEngine(string fileName, Action onFinish)
		{
			string path = "AdventureScene/Text/" + fileName;
			TextAsset textAsset = AssetDataMng.Instance().LoadObject(path, null, true) as TextAsset;
			FileControlHelper fileControlHelper = new FileControlHelper();
			AppCoroutine.Start(fileControlHelper.Decrypt(textAsset.bytes, new Action<byte[]>(this.OnLoadScriptFile)), onFinish, false);
		}

		public void SetCommand(AdventureBaseCommand command)
		{
			if (!this.commandList.ContainsKey(command.GetCommandName()))
			{
				this.commandList.Add(command.GetCommandName(), command);
			}
		}

		public void RemoveCommand(string[] commandNameList)
		{
			if (commandNameList != null)
			{
				for (int i = 0; i < commandNameList.Length; i++)
				{
					this.RemoveCommand(commandNameList[i]);
				}
			}
		}

		public void RemoveCommand(string commandName)
		{
			if (this.commandList.ContainsKey(commandName))
			{
				this.commandList.Remove(commandName);
			}
		}

		public void RunScript()
		{
			this.continueCommand = false;
			while (!this.finishScript)
			{
				this.scriptEngine.StepCommandScript(new Action<ScriptEngine.Status, int>(this.ActionScriptCommand), new Action(this.UpdateScriptCommand));
				if (!this.isError)
				{
					this.finishScript = this.scriptEngine.IsFinished();
				}
				else
				{
					this.finishScript = true;
					this.SetScriptState(AdventureScriptEngine.ScriptState.END);
				}
				if (!this.continueCommand)
				{
					break;
				}
			}
		}

		public void OnApplicationPauseAction(bool isPause)
		{
			if (!this.finishScript && this.runCommand != null)
			{
				if (isPause)
				{
					this.runCommand.OnPauseAction();
					this.tutorialCommandAction.OnApplicationPause();
				}
				else
				{
					this.runCommand.OnResumeAction();
					this.tutorialCommandAction.OnApplicationResume();
				}
			}
		}

		public bool IsFinish()
		{
			return this.finishScript;
		}

		public void DeleteScript()
		{
			this.continueCommand = false;
			this.scriptEngine = null;
			if (null != this.tutorialUI && null != this.tutorialUI.gameObject)
			{
				UnityEngine.Object.Destroy(this.tutorialUI.gameObject);
			}
			this.tutorialUI = null;
			TutorialControlToGame component = Singleton<TutorialObserver>.Instance.GetComponent<TutorialControlToGame>();
			if (null != component)
			{
				UnityEngine.Object.Destroy(component);
			}
		}

		public void SetScriptState(AdventureScriptEngine.ScriptState state)
		{
			this.scriptState = state;
		}

		public AdventureScriptEngine.ScriptState GetState()
		{
			return this.scriptState;
		}

		public enum ScriptState
		{
			INIT,
			RUN,
			END
		}
	}
}
