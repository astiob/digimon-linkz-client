using System;
using System.Linq;
using TutorialRequestHeader;

public class TutorialMaster
{
	private TutorialNaviMaster[] tutorialNaviMasterList;

	public void SetTutorialNaviMaster(TutorialNaviMasterResponse response)
	{
		if (response != null)
		{
			this.tutorialNaviMasterList = response.tutorialNaviM;
		}
	}

	public TutorialNaviMaster[] GetTutorialNaviMaster()
	{
		return this.tutorialNaviMasterList;
	}

	public TutorialMaster.NaviMessage GetNaviMessage(string id)
	{
		TutorialMaster.NaviMessage result = default(TutorialMaster.NaviMessage);
		try
		{
			TutorialNaviMaster tutorialNaviMaster = this.tutorialNaviMasterList.Single((TutorialNaviMaster x) => x.tutorialNaviId == id);
			result.message = tutorialNaviMaster.message;
			result.faceId = tutorialNaviMaster.face;
		}
		catch
		{
			Debug.LogErrorFormat("GetNaviMessage : ID NOT FOUND = {0}", new object[]
			{
				id
			});
		}
		return result;
	}

	public struct NaviMessage
	{
		public string message;

		public string faceId;
	}
}
