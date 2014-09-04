using UnityEngine;
using System.Collections;

public class PlayerEndOfPartyScript : MonoBehaviour 
{
	[SerializeField]
	private GameObject _playerPrefab;
	public GameObject PlayerPrefab
	{
		get { return _playerPrefab; }
		set { _playerPrefab = value; }
	}

	[SerializeField]
	private string _victoryMessage = string.Empty;
	public string VictoryMessage
	{
		get { return _victoryMessage; }
		set { _victoryMessage = value; }
	}
	
	[SerializeField]
	private bool _showMessage = false;
	public bool ShowMessage
	{
		get { return _showMessage; }
		set { _showMessage = value; }
	}

	[SerializeField]
	private GUISkin _skin;
	public GUISkin Skin
	{
		get { return _skin; }
		set { _skin = value; }	
	}

	[SerializeField]
	private AudioSource _audio;
	public AudioSource Audio
	{
		get { return _audio; }
		set { _audio = value; }
	}

	[SerializeField]
	private AudioClip _audioWin;
	public AudioClip AudioWin
	{
		get { return _audioWin; }
		set { _audioWin = value; }
	}

	[SerializeField]
	private AudioClip _audioLoose;
	public AudioClip AudioLoose
	{
		get { return _audioLoose; }
		set { _audioLoose = value; }
	}

	public static bool PlayingState;
	
	void Start()
	{
		if(networkView.isMine)
		{
			PlayingState = true;
		}
	}
	
	void Update()
	{
		if(networkView.isMine && PlayingState)
		{
			PlayerDatasScript playerDataScript = PlayerPrefab.GetComponent<PlayerDatasScript>();
			PlayerDatasScript otherPlayerDataScript = GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>();

			// Client
			if(playerDataScript.Health <= 0 || playerDataScript.NbCollectedFragments >= 5)
			{
				if(playerDataScript.Health <= 0)
				{
					OnEndOfParty(false);
				}

				if(playerDataScript.NbCollectedFragments >= 5)
				{
					OnEndOfParty(true);
				}
			}

			// Other Client
			if(otherPlayerDataScript.Health <= 0 || otherPlayerDataScript.NbCollectedFragments >= 5)
			{
				if(otherPlayerDataScript.Health <= 0)
				{
					OnEndOfParty(true);
				}
				
				if(otherPlayerDataScript.NbCollectedFragments >= 5)
				{
					OnEndOfParty(false);
				}
			}
		}
	}

	void OnEndOfParty(bool win)
	{
		PlayerDatasScript script = PlayerPrefab.GetComponent<PlayerDatasScript>();

		if(win)
		{
			VictoryMessage = script.PlayerName + ", you win ! ";
			Audio.PlayOneShot(AudioWin);
		}
		else
		{
			VictoryMessage = script.PlayerName + ", you loose ... ";
			Audio.PlayOneShot(AudioLoose);
		}

		ShowMessage = true;
		PlayingState = false;
		Invoke("End", 5.0f);
	}

	void OnGUI()
	{
		if(networkView.isMine)
		{
			if(ShowMessage)
			{
				GUI.skin = Skin;
				Toolkit.MessageBox(VictoryMessage);
			}
		}
	}
	
	public void End()
	{
		ShowMessage = false;
		Network.DestroyPlayerObjects(Network.player);
		Network.Disconnect();
		//	DestroyImmediate(PersistentLobbyOnload, true);
		Application.LoadLevel("Menu");
	}

	void FlyingTrisRun()
	{
		PlayerPrefab.GetComponent<FXFlyingTrianglesScript>().Fly();
	}
}
