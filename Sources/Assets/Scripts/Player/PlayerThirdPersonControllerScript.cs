using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class PlayerThirdPersonControllerScript : MonoBehaviour 
{
	[SerializeField]
	private AudioSource _source;
	public AudioSource Source
	{
		get { return _source; }
		set { _source = value; }
	}
	
	[SerializeField]
	private AudioClip _jumpSound;
	public AudioClip JumpSound
	{
		get { return _jumpSound; }
		set { _jumpSound = value; }
	}

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
	private bool sound_run = false;
	
	void FixedUpdate() 
	{
		if(networkView.isMine)
		{
			if(PlayerEndOfPartyScript.PlayingState)
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
						Source.PlayOneShot(JumpSound);
						jumpReloaded = false;
						Body.AddForce(Vector3.up * Time.deltaTime * 10000.0f, ForceMode.Acceleration);
						Invoke("ReloadJump", 1.0f);
					}
				}
			}

			DidactitielScript scriptt = GameObject.Find("OnLoadPlay").GetComponent<DidactitielScript>();
			if(!sound_run && scriptt.IsStep(3))
			{
				sound_run = true;
				scriptt.Next();
			}
		}
	}
	
	void ReloadJump()
	{
		jumpReloaded = true;
	}
}
