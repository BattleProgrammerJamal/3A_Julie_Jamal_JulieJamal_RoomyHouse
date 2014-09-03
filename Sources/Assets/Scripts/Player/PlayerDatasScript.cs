using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private Texture[] _fragsStar2D;
	public Texture[] FragsStar2D
	{
		get { return _fragsStar2D; }
		set { _fragsStar2D = value; }
	}

	[SerializeField]
	private GUISkin _enemyNameSkin;
	public GUISkin EnemyNameSkin
	{
		get { return _enemyNameSkin; }
		set { _enemyNameSkin = value; }
	}

	private float _healthBarLength = 0.0f;

	void Start() 
	{
		if(networkView.isMine)
		{
			if(GeneralCreateMenuGuiScript.Chapeau_Id == 0 || GeneralCreateMenuGuiScript.Chapeau_Id == 1)
			{
				foreach(Component obj in this.transform.GetComponentsInChildren<Component>())
				{
					if(GeneralCreateMenuGuiScript.Chapeau_Id == 0 && obj.name.Equals("Hat1"))
					{
						obj.gameObject.SetActive(false);
					}

					if(GeneralCreateMenuGuiScript.Chapeau_Id == 0 && obj.name.Equals("Hat2"))
					{
						obj.gameObject.SetActive(true);
					}

					if(GeneralCreateMenuGuiScript.Chapeau_Id == 1 && obj.name.Equals("Hat1"))
					{
						obj.gameObject.SetActive(true);
					}
					
					if(GeneralCreateMenuGuiScript.Chapeau_Id == 1 && obj.name.Equals("Hat2"))
					{
						obj.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				foreach(Component obj in this.transform.GetComponentsInChildren<Component>())
				{
					if(obj.name.Equals("Hat1") || obj.name.Equals("Hat2"))
					{
						obj.gameObject.SetActive(false);
					}
				}
			}

			_healthBarLength = Screen.width / 2;
		}
	}
	
	void Update()
	{
		if(networkView.isMine)
		{
			string datas = PlayerName + ";" + MaxHealth.ToString() + ";" + Health.ToString() + ";" + RedBalls.ToString() + ";" + YellowBalls.ToString() + ";" + GreenBalls.ToString()
				+ ";" + NbCollectedFragments.ToString();
			networkView.RPC("SynchronizePlayerDatas", RPCMode.Others, datas);
			AdjustHealth(0);
		}
	}

	void OnGUI() 
	{
		if(networkView.isMine)
		{
			switch(NbCollectedFragments)
			{
				case 1:
					GUI.DrawTexture(new Rect(10.0f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[0]);
				break;

				case 2:
					GUI.DrawTexture(new Rect(10.0f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[1]);
				break;

				case 3:
					GUI.DrawTexture(new Rect(10.0f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[2]);
				break;

				case 4:
					GUI.DrawTexture(new Rect(10.0f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[3]);
				break;

				case 5:
					GUI.DrawTexture(new Rect(10.0f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[4]);
				break;
			}

			switch(GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>().NbCollectedFragments)
			{
				case 1:
					GUI.DrawTexture(new Rect(Screen.width * 0.36f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[0]);
				break;

				case 2:
					GUI.DrawTexture(new Rect(Screen.width * 0.36f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[1]);
				break;

				case 3:
					GUI.DrawTexture(new Rect(Screen.width * 0.36f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[2]);
				break;

				case 4:
					GUI.DrawTexture(new Rect(Screen.width * 0.36f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[3]);
				break;

				case 5:
					GUI.DrawTexture(new Rect(Screen.width * 0.36f, 125.0f, Screen.width * 0.10f, Screen.height * 0.10f), FragsStar2D[4]);
				break;
			}

			GUI.Box(new Rect(Screen.width * 0.01f, 10, Screen.width * 0.33f, 25), PlayerName); 
			GUI.Box(new Rect(Screen.width * 0.36f, 10, Screen.width * 0.62f, 25), RedBalls + " red balls | " + YellowBalls + " yellow balls | " + GreenBalls + " green balls"); 
			GUI.Box(new Rect(10, 38, 60.0f + _healthBarLength, 20), (Health * 100.0f) / MaxHealth + "%");

			GUISkin temp = GUI.skin;
			GUI.skin = EnemyNameSkin;

			GUI.Box(new Rect(Screen.width * 0.01f, 60.0f, Screen.width * 0.33f, 25), GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>().PlayerName); 

			GUI.skin = temp;

			GUI.Box(new Rect(Screen.width * 0.36f, 60.0f, Screen.width * 0.62f, 25), GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>().RedBalls + " red balls | " + GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>().YellowBalls + " yellow balls | " + GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>().GreenBalls + " green balls"); 
			GUI.Box(new Rect(10, 93.0f, 60.0f + _healthBarLength, 20), (GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>().Health * 100.0f) / GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>().MaxHealth + "%");
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

	[RPC]
	void SynchronizePlayerDatas(string datas)
	{
		if(Network.peerType == NetworkPeerType.Client)
		{
			PlayerDatasScript otherDatas = GameObject.Find("PlayerTmp(Clone)").GetComponent<PlayerDatasScript>();
			string[] t_data = datas.Split(new char[] { ';' });

			otherDatas.PlayerName = t_data[0];
			otherDatas.MaxHealth = float.Parse(t_data[1]);
			otherDatas.Health = float.Parse(t_data[2]);
			otherDatas.RedBalls = int.Parse(t_data[3]);
			otherDatas.YellowBalls = int.Parse(t_data[4]);
			otherDatas.GreenBalls = int.Parse(t_data[5]);
			otherDatas.NbCollectedFragments = int.Parse(t_data[6]);
		}
	}
}
