using UnityEngine;
using System.Collections;

public class ThrirdPersonController : MonoBehaviour 
{
	[SerializeField]
	public Transform _transform;
	public Camera _camera;
	public float _speed = 1.0f;
	public float _angularSpeed = 1.0f;
	public float _jumpHeight = 15.0f;
	
	float _tmpYSaut = 0.0f;
	bool _jumping = false;
	
	void Start() 
	{
	}
	
	void Update() 
	{
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
		{
			_transform.RotateAround(Vector3.up, -_angularSpeed * Time.deltaTime);
		}
		
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			_transform.RotateAround(Vector3.up, _angularSpeed * Time.deltaTime);	
		}
		
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
		{
			_transform.Translate(Vector3.forward * Time.deltaTime * _speed);	
		}
		
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			_transform.Translate(Vector3.back * Time.deltaTime * _speed);	
		}
		/*
		if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Keypad0))
		{
			if(!_jumping)
			{
				_tmpYSaut = _transform.position.y;
				Jump();
			}
			else
			{
				if(_transform.position.y >= _jumpHeight + _tmpYSaut)
				{
					GoDown();
					_jumping = false;
				}
			}
		}
		*/
	}
	
	void Jump()
	{
		_transform.Translate(Vector3.up * Time.deltaTime * _speed);
	}
	
	void GoDown()
	{
		_transform.Translate(Vector3.down * Time.deltaTime * _speed);
	}
}
