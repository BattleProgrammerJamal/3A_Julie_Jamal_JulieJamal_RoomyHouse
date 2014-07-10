using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class ThirdPersonController : MonoBehaviour 
{
	[SerializeField]
	private Rigidbody _body;
	public Rigidbody Body
	{
		get { return _body; }
		set { _body = value; }
	}
	
	[SerializeField]
	private float _speed = 2.0f;
	public float Speed
	{
		get { return _speed; }
		set { _speed = value; }
	}
	
	[SerializeField]
	private float _angularSpeed = 80.0f;
	public float AngularSpeed
	{
		get { return _angularSpeed; }
		set { _angularSpeed = value; }
	}
	
	private bool jumpReloaded = true;
	
	void FixedUpdate() 
	{
		if(networkView.isMine)
		{
			if(PlayerDatas.PlayingState)
			{
				if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
				{
					Body.transform.Rotate(0.0f, -AngularSpeed * Time.deltaTime, 0.0f);
				}
				
				if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
				{
					Body.transform.Rotate(0.0f, AngularSpeed * Time.deltaTime, 0.0f);
				}
				
				if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
				{
					Body.transform.Translate(Vector3.forward * Time.deltaTime * Speed);	
				}
				
				if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
				{
					Body.transform.Translate(Vector3.back * Time.deltaTime * Speed);	
				}
				
				if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Keypad0))
				{
					if(jumpReloaded)
					{
						jumpReloaded = false;
						Body.AddForce(Vector3.up * Time.deltaTime * 10000.0f, ForceMode.Acceleration);
						Invoke("ReloadJump", 1.0f);
					}
				}
			}
		}
	}
	
	void ReloadJump()
	{
		jumpReloaded = true;
	}
}
