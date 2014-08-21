using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkInitPlayerScript : MonoBehaviour
{
	[SerializeField]
	private AudioSource _source;
	public AudioSource Source
	{
		get { return _source; }
		set { _source = value; }
	}
	
	[SerializeField]
	private AudioClip _letsPlay;
	public AudioClip LetsPlay
	{
		get { return _letsPlay; }
		set { _letsPlay = value; }
	}

	[SerializeField]
	private AudioClip[] _wallSounds;
	public AudioClip[] WallSounds
	{
		get { return _wallSounds; }
		set { _wallSounds = value; }
	}

	void Start()
	{
		if(networkView.isMine)
		{
			Source.PlayOneShot(LetsPlay);
			Invoke("PlayWallSounds", LetsPlay.length);

			GetComponent<PlayerDatasScript>().PlayerName = PlayerPrefs.GetString("player_name");
			Camera myCamera = GetComponentInChildren<Camera>();
			if(myCamera != null)
			{
				myCamera.enabled = true;	
				myCamera.GetComponent<AudioListener>().enabled = true;
				GameObject.FindGameObjectWithTag("minimap").GetComponent<RenderCamera2D>().Cam = myCamera;
			}
		}
	}

	void PlayWallSounds()
	{
		for(int i = 0; i < WallSounds.Length; ++i)
		{
			Source.volume = 0.85f;
			Source.PlayOneShot(WallSounds[i]);
		}
	}
}
