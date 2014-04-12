using UnityEngine;
using System.Collections;

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
	private float _power = 5.0f;
	public float Power
	{
		get { return _power; }
		set { _power = value; }
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
	
	float _healthBarLength;
	
	void Start() 
	{
		_healthBarLength = Screen.width / 2;
	}
	
	void Update() 
	{
		AdjustHealth(0);
	}
	
	void OnGUI() 
	{
		GUI.Box(new Rect(10, 10, Screen.width * 0.10f, 25), PlayerName); 
		GUI.Box(new Rect(10 + Screen.width * 0.10f + 3.0f, 10, Screen.width * 0.80f, 25), RedBalls + " red balls | " + YellowBalls + " yellow balls | " + GreenBalls + " green balls  [ " + Power + " orbes du chaos ]"); 
		GUI.Box(new Rect(10, 38, 60.0f + _healthBarLength, 20), (Health * 100.0f) / MaxHealth + "%");
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
