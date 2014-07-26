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
	private AudioClip _wallSound;
	public AudioClip WallSound
	{
		get { return _wallSound; }
		set { _wallSound = value; }
	}

	void Start()
	{
		if(networkView.isMine)
		{
			audio.volume = 1.0f;
			Source.PlayOneShot(LetsPlay);
			audio.volume = 0.0f;
			Invoke("PlayWallSound", LetsPlay.length);

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

	void PlayWallSound()
	{
		Source.PlayOneShot(WallSound);
		audio.volume = 1.0f;
	}
}
