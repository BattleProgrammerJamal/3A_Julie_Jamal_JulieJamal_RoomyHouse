using UnityEngine;
using System.Collections;

public class GeneralCreateMenuGuiScript : MonoBehaviour 
{
	[SerializeField]
	private Texture _background;
	public Texture Background
	{
		get { return _background; }
		set { _background = value; }
	}
	
	[SerializeField]
	private GUISkin _skin;
	public GUISkin Skin
	{
		get { return _skin; }
		set { _skin = value; }
	}
	
	[SerializeField]
	private AudioSource _source;
	public AudioSource Source
	{
		get { return _source; }
		set { _source = value; }
	}
	
	[SerializeField]
	private AudioClip _corbeaux_clip;
	public AudioClip CorbeauxClip
	{
		get { return _corbeaux_clip; }
		set { _corbeaux_clip = value; }
	}
	
	private bool isClient = false;
	
	void Start()
	{
		InvokeRepeating("BackgroundSounds", 0.0f, 10.0f);
	}
	
	void OnGUI()
	{
		GUI.skin = Skin;
		
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();	
		}
		
		GUI.DrawTexture(new Rect(0.0f, 0.0f, Screen.width, Screen.height), Background);
		GUI.Box(new Rect(Screen.width * 0.20f, Screen.height * 0.20f, Screen.width * 0.66f, Screen.height * 0.5f), string.Empty);
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		style.normal.textColor = Color.white;
		GUI.Label(new Rect((Screen.width * 0.5f) - 50.0f, (Screen.height * 0.30f) - 10.0f, 100, 20), "Roomy House", style);
		
		if(GUI.Button(new Rect((Screen.width * 0.5f) - 25.0f, Screen.height * 0.40f, 100, 20), "START"))
		{
			PlayerPrefs.SetString("isClient", isClient.ToString());
			Application.LoadLevel("Lobby");
		}
		
		if(GUI.Button(new Rect((Screen.width * 0.50f) - 25.0f, Screen.height * 0.45f, 100, 20), "EXIT"))
		{
			Application.Quit();
		}
		
		isClient = GUI.Toggle(new Rect((Screen.width * 0.50f) - 25.0f, Screen.height * 0.50f, 100, 20), isClient, "Client");
	}
	
	void BackgroundSounds()
	{
		int n = Random.Range(1, 100);
		
		if(n < 50)
		{
			Source.PlayOneShot(CorbeauxClip);	
		}
	}
}
