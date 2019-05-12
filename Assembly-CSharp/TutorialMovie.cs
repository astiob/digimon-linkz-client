using Neptune.Movie;
using System;
using UnityEngine;

public class TutorialMovie : MonoBehaviour, NpMovie.INpMovieListener
{
	public Action actionFinishedMovie;

	public void OnMovieFinish()
	{
		if (this.actionFinishedMovie != null)
		{
			this.actionFinishedMovie();
			this.actionFinishedMovie = null;
		}
	}

	public void OnMovieErr(string errCode)
	{
		if (this.actionFinishedMovie != null)
		{
			this.actionFinishedMovie();
			this.actionFinishedMovie = null;
		}
	}
}
