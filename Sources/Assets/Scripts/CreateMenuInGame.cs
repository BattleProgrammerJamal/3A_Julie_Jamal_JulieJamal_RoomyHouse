using UnityEngine;
using System.Collections;

public class CreateMenuInGame : MonoBehaviour 
{
	[SerializeField]
	public KeyCode _key = KeyCode.P;
	
	bool _showMenu = false;
	bool _activeTimeout = false;
	
	void Update() 
	{
		if(Input.GetKey(_key) && !_activeTimeout)
		{
			_showMenu = !_showMenu;	
			_activeTimeout = true;
			Invoke("Timeout", 0.5f);
		}
	}
	
	void Timeout()
	{
		_activeTimeout = false;
	}
	
	void OnGUI()
	{
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
				Application.LoadLevel("Lobby");
			}
		}
	}
}
