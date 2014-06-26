using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class PlayerDatas : MonoBehaviour 
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
	
	float _healthBarLength;

	void Start() 
	{
		if(networkView.isMine)
		{
			_healthBarLength = Screen.width / 2;
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(col.collider.name == "Projectile")
		{
			AdjustHealth(-(5.0f * Random.Range(1.0f, 3.0f)));
		}
	}
	
	void OnGUI() 
	{
		if(networkView.isMine)
		{
			AdjustHealth(0);
			
			GUI.Box(new Rect(Screen.width * 0.01f, 10, Screen.width * 0.33f, 25), PlayerName); 
			GUI.Box(new Rect(Screen.width * 0.36f, 10, Screen.width * 0.62f, 25), RedBalls + " red balls | " + YellowBalls + " yellow balls | " + GreenBalls + " green balls"); 
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
}
