using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralLobbySoundHandlerScript : MonoBehaviour 
{
	[SerializeField]
	private AudioSource _source;
	public AudioSource Source
	{
		get { return _source; }
		set { _source = value; }
	}
	
	[SerializeField]
	private List<AudioClip> _sounds;
	public List<AudioClip> Sounds
	{
		get { return _sounds; }
		set { _sounds = value; }
	}
	
	private int current = 0, last = -1;
	
	void Start() 
	{
		if(Sounds.Count != 0)
		{
			Invoke("RunSound", 3.0f);
		}
	}
	
	void RunSound()
	{
		Source.PlayOneShot(Sounds[current]);
		Invoke("RunSound", Sounds[current].length + Random.Range(0.0f, 5.0f));
		last = current;
		do
		{
			current = Random.Range(0, Sounds.Count - 1);
		} while(current == last);
	}
}
