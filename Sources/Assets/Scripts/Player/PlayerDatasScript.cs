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
	
	public static bool PlayingState;
	
	float _healthBarLength;

	void Start() 
	{
		if(networkView.isMine)
		{
			PlayingState = true;
			_healthBarLength = Screen.width / 2;
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		networkView.RPC("HitTarget", RPCMode.All, col.gameObject.name);
	}
	
	void OnGUI() 
	{
		if(networkView.isMine)
		{
			AdjustHealth(0);

			if(Health <= 0)
			{
				if(name == "player1")
				{
					networkView.RPC("Victory", RPCMode.AllBuffered, "Player 2"); 
				}
				else
				{
					networkView.RPC("Victory", RPCMode.AllBuffered, "Player 1"); 
				}
			}

			if(NbCollectedFragments >= 5)
			{
				if(name == "player1")
				{
					networkView.RPC("Victory", RPCMode.AllBuffered, "Player 1"); 
				}
				else
				{
					networkView.RPC("Victory", RPCMode.AllBuffered, "Player 2"); 
				}
			}

			GUI.Box(new Rect(Screen.width * 0.01f, 10, Screen.width * 0.33f, 25), PlayerName); 
			GUI.Box(new Rect(Screen.width * 0.36f, 10, Screen.width * 0.62f, 25), RedBalls + " red balls | " + YellowBalls + " yellow balls | " + GreenBalls + " green balls | " + NbCollectedFragments + " star fragments"); 
			GUI.Box(new Rect(10, 38, 60.0f + _healthBarLength, 20), (Health * 100.0f) / MaxHealth + "%");
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
	public void Victory(string pID)
	{
		PlayingState = false;
		GUIStyle style = new GUIStyle();
		style.wordWrap = true;
		GUI.Box(new Rect(Screen.width * 0.5f - 100.0f, Screen.height * 0.5f - 50.0f, 200.0f, 100.0f), pID + " : " + PlayerName + " HAS WON !!! ", style);
		Invoke("End", 5.0f);
	}

	[RPC]
	void HitTarget(string name)
	{
		if(name == "Projectile(Clone)")
		{
			int val = Random.Range(0, 100);
			
			if(val < 15)
			{
				AdjustHealth(-6.0f);
			}
			else
			{
				AdjustHealth(-3.0f);
			}
		}
	}

	public void End()
	{
		PlayingState = true;
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
		Network.Disconnect();
		Application.LoadLevel("Menu");
	}
}
