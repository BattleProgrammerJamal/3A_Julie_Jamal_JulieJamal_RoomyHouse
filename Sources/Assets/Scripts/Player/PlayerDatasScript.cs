using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class PlayerDatasScript : MonoBehaviour 
{	
	[SerializeField]
	private string _playerName = "Anonymous";
	public string PlayerName
	{
		get { return _playerName; }
		set { _playerName = value; }
	}
	
	[SerializeField]
	private float _maxHealth = 100.0f;
	public float MaxHealth
	{
		get { return _maxHealth; }
		set { _maxHealth = value; }
	}
	
	[SerializeField]
	private float _health = 100.0f;
	public float Health
	{
		get { return _health; }
		set { _health = value; }
	}
	
	[SerializeField]
	private int _redBalls = 0;
	public int RedBalls
	{
		get { return _redBalls; }
		set { _redBalls = value; }
	}
	
	[SerializeField]
	private int _yellowBalls = 0;
	public int YellowBalls
	{
		get { return _yellowBalls; }
		set { _yellowBalls = value; }
	}
	
	[SerializeField]
	private int _greenBalls = 0;
	public int GreenBalls
	{
		get { return _greenBalls; }
		set { _greenBalls = value; }
	}

	[SerializeField]
	private int _nbCollectedFragments = 0;
	public int NbCollectedFragments
	{
		get { return _nbCollectedFragments; }
		set { _nbCollectedFragments = value; }
	}

	[SerializeField]
	private GUISkin _skin;
	public GUISkin Skin
	{
		get { return _skin; }
		set { _skin = value; }	
	}

	[SerializeField]
	private Object _persistentLobbyOnload;
	public Object PersistentLobbyOnload
	{
		get { return _persistentLobbyOnload; }
		set { _persistentLobbyOnload = value; }
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
	
	public static bool PlayingState;
	private bool victory = false;
	private bool end = false;
	private float _healthBarLength = 0.0f;

	void Start() 
	{
		if(networkView.isMine)
		{
			VictoryMessage = string.Empty;
			PlayingState = true;
			ShowMessage = false;
			end = false;
			_healthBarLength = Screen.width / 2;
		}
	}
	
	void Update()
	{
		if(networkView.isMine)
		{
			AdjustHealth(0);

			if(Health == 0 || NbCollectedFragments >= 5)
			{
				end = true;
			}

			if(PlayingState)
			{
				if(end)
				{
					Toolkit.Log<string>("END: " + PlayerName + "; " + VictoryMessage);
					if(Health == 0)
					{
						victory = false;
						VictoryMessage = "YOU HAVE LOST, TRY AGAIN ... ";
						FlyingTrisRun();
					}
					
					if(NbCollectedFragments >= 5)
					{
						victory = true;
						VictoryMessage = "CONGRATULATIONS " + PlayerName + ", YOU HAVE WON !!!! ";
					}

					PlayingState = false;
					ShowMessage = true;
					networkView.RPC("Victory", RPCMode.Others, victory);
					Invoke("End", 5.0f);
				}
			}
		}
	}
	
	void OnGUI() 
	{
		if(networkView.isMine)
		{
			GUI.Box(new Rect(Screen.width * 0.01f, 10, Screen.width * 0.33f, 25), PlayerName); 
			GUI.Box(new Rect(Screen.width * 0.36f, 10, Screen.width * 0.62f, 25), RedBalls + " red balls | " + YellowBalls + " yellow balls | " + GreenBalls + " green balls | " + NbCollectedFragments + " star fragments"); 
			GUI.Box(new Rect(10, 38, 60.0f + _healthBarLength, 20), (Health * 100.0f) / MaxHealth + "%");
		
			if(ShowMessage)
			{
				GUI.skin = Skin;
				GUI.Box(new Rect(Screen.width * 0.33f, Screen.height * 0.25f, Screen.width * 0.33f, Screen.height * 0.25f), VictoryMessage);
			}
		}
	}
	
	public void AdjustHealth(float health)
	{
		Health += health;
		if(Health < 0) { Health = 0; }
		if(Health > MaxHealth) { Health = MaxHealth; }
		if(MaxHealth < 1) { MaxHealth = 1; }
		_healthBarLength = (Screen.width / 2) * (Health / (float)MaxHealth);
	}

	[RPC]
	public void Victory(bool victory)
	{
		if(Network.peerType != NetworkPeerType.Server)
		{
			ShowMessage = true;
			PlayingState = false;
			if(!victory)
			{
				VictoryMessage = "CONGRATULATIONS " + PlayerName + ", YOU HAVE WON !!!! ";
			}
			else
			{
				VictoryMessage = "YOU HAVE LOST, TRY AGAIN ... ";
				FlyingTrisRun();
			}
			Invoke("End", 5.0f);
		}
	}

	public void End()
	{
		ShowMessage = false;
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
		Network.Disconnect();
		DestroyImmediate(PersistentLobbyOnload, true);
		Application.LoadLevel("Menu");
	}

	void FlyingTrisRun()
	{
		GetComponent<FXFlyingTrianglesScript>().Fly();
	}

	void OnCollisionEnter(Collision col)
	{
		networkView.RPC("HitTarget", RPCMode.All, col.gameObject.name);
	}

	[RPC]
	void HitTarget(string name)
	{
		if(name.Equals("Projectile(Clone)"))
		{
			int val = Random.Range(0, 100);
			
			if(val < 15)
			{
				AdjustHealth(-15.0f);
			}
			else
			{
				AdjustHealth(-5.0f);
			}
		}
	}
}
