using UnityEngine;
using System.Collections;

public class MovieSurface : MonoBehaviour 
{
	[SerializeField]
	private MovieTexture _movie;
	public MovieTexture Movie
	{
		get { return _movie; }
		set { _movie = value; }
	}

	void Start() 
	{
		Movie.Play();
	}
}
