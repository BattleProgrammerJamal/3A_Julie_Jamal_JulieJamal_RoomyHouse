using UnityEngine;
using System.Collections;

public class PlayerDatas : MonoBehaviour 
{
	[SerializeField]
	public string _playerName = "";
	public float _maxHealth = 100.0f;
	public float _health = 100.0f;
	
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
		GUI.Box(new Rect(10, 10, 150, 20), _playerName);
		GUI.Box(new Rect(10, 35, _healthBarLength, 20), _health + "/" + _maxHealth);
	}
	
	void AdjustHealth(float health)
	{
		_health += health;
		if(_health < 0) { _health = 0; }
		if(_health > _maxHealth) { _health = _maxHealth; }
		if(_maxHealth < 1) { _maxHealth = 1; }
		_healthBarLength = (Screen.width / 2) * (_health / (float)_maxHealth);
	}
}
