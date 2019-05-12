using Neptune.Movie;
using System;
using UnityEngine;

public class PlayMovie : MonoBehaviour, NpMovie.INpMovieListener
{
	public Action<bool> actionFinishedMovie;

	public void OnMovieFinish()
	{
		if (this.actionFinishedMovie != null)
		{
			this.actionFinishedMovie(false);
			this.actionFinishedMovie = null;
		}
	}

	public void OnMovieErr(string errCode)
	{
		if (this.actionFinishedMovie != null)
		{
			this.actionFinishedMovie(false);
			this.actionFinishedMovie = null;
		}
	}
}
