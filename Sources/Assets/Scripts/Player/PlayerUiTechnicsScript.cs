using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class PlayerUiTechnicsScript : MonoBehaviour 
{
	
	[SerializeField]
	private Transform _transform;
	public Transform MyTransform
	{
		get { return _transform; }
		set { _transform = value; }
	}
	
	[SerializeField]
	private GameObject _projectile;
	public GameObject Projectile
	{
		get { return _projectile; }
		set { _projectile = value; }
	}
	
	[SerializeField]
	private float _reloadTime = 0.5f;
	public float ReloadTime
	{
		get { return _reloadTime; }
		set { _reloadTime = value; }
	}
	
	[SerializeField]
	private float _force = 15.0f;
	public float Force
	{
		get { return _force; }
		set { _force = value; }
	}
	
	[SerializeField]
	private Texture _menuingametexture;
	public Texture MenuInGameTexture
	{
		get { return _menuingametexture; }
		set { _menuingametexture = value; }
	}
	
	[SerializeField]
	private Texture _fireballtexture;
	public Texture FireBallTexture
	{
		get { return _fireballtexture; }
		set { _fireballtexture = value; }
	}
	
	[SerializeField]
	private KeyCode _key = KeyCode.P;
	public KeyCode Key
	{
		get { return _key; }
		set { _key = value; }
	}
	
	[SerializeField]
	private AudioSource _source;
	public AudioSource Source
	{
		get { return _source; }
		set { _source = value; }
	}
	
	[SerializeField]
	private AudioClip _shootSound;
	public AudioClip ShootSound
	{
		get { return _shootSound; }
		set { _shootSound = value; }
	}
	
	private bool timeout = true;
	private GameObject clone;
	private bool _showMenu = false;
	private bool _activeTimeout = false;
	private string disconnectedLevel = "Lobby";
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width * 0.38f, Screen.height * 0.85f, Screen.width * 0.60f, Screen.height * 0.15f));
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button(FireBallTexture, GUILayout.Width(64), GUILayout.Height(64)))
		{
			Invoke("ProjectileCreation", 0.01f);
		}
	
		if(GUILayout.Button(MenuInGameTexture, GUILayout.Width(64), GUILayout.Height(64)) || Input.GetKeyUp(Key))
		{
			if(!_activeTimeout)
			{
				_showMenu = !_showMenu;	
				_activeTimeout = true;
				Invoke("Timeout", 0.5f);
			}
		}
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		if(_showMenu)
		{
			GUI.Box(new Rect(Screen.width * 0.33f, Screen.height * 0.33f, Screen.width * 0.33f, Screen.height * 0.5f), string.Empty);		
			GUI.Label(new Rect(Screen.width * 0.37f + (Screen.width * 0.33f / 3), Screen.height * 0.38f, 125, 20), "MENU");
			
			if(GUI.Button(new Rect(Screen.width * 0.33f + (Screen.width * 0.33f / 3), Screen.height * 0.44f, 125, 20), "Continue"))
			{
				_showMenu = false;
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.33f + (Screen.width * 0.33f / 3), Screen.height * 0.50f, 125, 20), "Exit"))
			{
				Network.RemoveRPCs(Network.player);
				Network.DestroyPlayerObjects(Network.player);
				Network.Disconnect();
				Application.LoadLevel(disconnectedLevel);
			}
		}
	}
	
	void ProjectileCreation()
	{
		if(timeout)
		{
			Source.PlayOneShot(ShootSound);
			
			timeout = false;
			clone = (GameObject)Network.Instantiate(Projectile, MyTransform.position + (MyTransform.forward * 2.0f), MyTransform.rotation, 0);
			Physics.IgnoreCollision(clone.collider, MyTransform.collider);
			
			clone.rigidbody.AddForce(MyTransform.forward * Force, ForceMode.Impulse);
			Invoke("ProjectileDestruction", ReloadTime * 0.8f);
			Invoke("Reload", ReloadTime);
		}
	}
	
	void ProjectileDestruction()
	{
		Network.Destroy(clone);
	}
	
	void Reload()
	{
		timeout = true;	
	}
	
	void Timeout()
	{
		_activeTimeout = false;
	}
}
