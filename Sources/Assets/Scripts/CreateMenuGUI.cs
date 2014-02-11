using UnityEngine;
using System.Collections;

public class CreateMenuGUI : MonoBehaviour 
{
	void Start()
	{
	}
	
	void Update() 
	{
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();	
		}
	}
	
	void OnGUI()
	{
		GUI.Box(new Rect(Screen.width * 0.20f, Screen.height * 0.15f, Screen.width * 0.66f, Screen.height * 0.66f), string.Empty);
		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.30f, 100, 20), "Roomy House", style);
		
		if(GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.40f, 100, 20), "START"))
		{
			Application.LoadLevel("Cuisine");
		}
		
		if(GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.45f, 100, 20), "SETTINGS"))
		{
		}
		
		if(GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.50f, 100, 20), "QUIT"))
		{
			Application.Quit();
		}
		
		if(GUI.Button(new Rect(Screen.width * 0.65f, Screen.height * 0.65f, 100, 20), "Website"))
		{
			Application.OpenURL("http://www.jamal-bouizem.info");
		}
	}
}
