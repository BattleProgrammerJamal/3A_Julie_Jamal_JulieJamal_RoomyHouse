using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DidactitielScript : MonoBehaviour 
{
	[SerializeField]
	private AudioSource _source;
	public AudioSource Source
	{
		get { return _source; }
		set { _source = value; }
	}

	[SerializeField]
	private List<AudioClip> _didactitielSounds;
	public List<AudioClip> DidactitielSounds
	{
		get { return _didactitielSounds; }
		set { _didactitielSounds = value; }
	}

	public bool play;
	public int step;

	void Awake()
	{
		step = 0;
	}

	void Update()
	{
		if(play && GeneralCreateMenuGuiScript.Didactitiel)
		{
			play = false;
			Source.PlayOneShot(DidactitielSounds[step]);
			if(step == DidactitielSounds.Count - 1) { GeneralCreateMenuGuiScript.Didactitiel = false; } else { step++; }
		}
	}

	void OnGUI()
	{
		//	Toolkit.MessageBox(GeneralCreateMenuGuiScript.Didactitiel.ToString());
		Toolkit.Log<string>(GeneralCreateMenuGuiScript.Didactitiel.ToString());
	}

	public bool IsStep(int n)
	{
		return (step == n);
	}

	public void Next()
	{
		play = true;
	}
}
