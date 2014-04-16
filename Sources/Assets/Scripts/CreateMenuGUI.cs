using UnityEngine;
using System.Collections;

public class CreateMenuGUI : MonoBehaviour 
{
	void OnGUI()
	{
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();	
		}
		
		GUI.Box(new Rect(Screen.width * 0.20f, Screen.height * 0.15f, Screen.width * 0.66f, Screen.height * 0.66f), string.Empty);
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.30f, 100, 20), "Roomy House", style);
		
		if(GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.40f, 100, 20), "START"))
		{
			Application.LoadLevel("Lobby");
		}
		
		if(GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.45f, 100, 20), "EXIT"))
		{
			Application.Quit();
		}
	}
}
