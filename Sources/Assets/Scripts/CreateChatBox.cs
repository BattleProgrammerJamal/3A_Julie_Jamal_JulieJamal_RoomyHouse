using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkView))]
public class CreateChatBox : MonoBehaviour 
{
	[SerializeField]
	private string _user = "Anonymous";
	public string User
	{
		get { return _user; }
		set { _user = value; }
	}
	
	[SerializeField]
	private string _message = "";
	public string Message
	{
		get { return _message; }
		set { _message = value; }
	}
	
	[SerializeField]
	private GUISkin _skin;
	public GUISkin Skin
	{
		get { return _skin; }
		set { _skin = value; }	
	}
	
	[SerializeField]
	private float _sizeW = 0.33f;
	public float SizeW
	{
		get { return _sizeW; }
		set { _sizeW = value; }
	}	
	
	[SerializeField]
	private float _sizeH = 0.90f;
	public float SizeH
	{
		get { return _sizeH; }
		set { _sizeH = value; }
	}	
	
	string _messageBox = string.Empty;
	Rect _chatBox = new Rect(Screen.width * 0.01f, Screen.height * 0.05f, Screen.width * 0.33f, Screen.height * 0.90f);
	
	List<string> filtre_insultes = new List<string>();
	
	void Awake()
	{
		filtre_insultes.Add("idiot");	
		filtre_insultes.Add("enfoiré");
		filtre_insultes.Add("batard");
		filtre_insultes.Add("bouffon");
		filtre_insultes.Add("abruti");
		filtre_insultes.Add("misérable");
		filtre_insultes.Add("con");
		_chatBox.width = Screen.width * SizeW;
		_chatBox.height = Screen.height * SizeH;
	}
	
	void OnGUI()
	{	
		if(Network.peerType != NetworkPeerType.Disconnected)
		{
			_chatBox = GUI.Window(3, _chatBox, chatBoxHandler, "Chat");
		}
	}
	
	void chatBoxHandler(int id)
	{
		GUI.skin = Skin;
		GUILayout.Box(_messageBox, GUILayout.Height(_chatBox.height * 0.50f));
		
		GUILayout.BeginHorizontal();
			GUILayout.Label("User");
			User = GUILayout.TextField(User);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
			GUILayout.Label("Message");
			GUI.SetNextControlName("MessageTextField");
			Message = GUILayout.TextField(Message);
			GUI.SetNextControlName(string.Empty);
		GUILayout.EndHorizontal();
		
		if(GUILayout.Button("Send", GUILayout.Width(75)))
		{
			networkView.RPC("SendChatMessage", RPCMode.All, User, Message);
			Message = "";
		}
		
		if(GUI.GetNameOfFocusedControl() == "MessageTextField")
		{
			if ((Event.current.type == EventType.KeyUp) && (Event.current.keyCode == KeyCode.Return))
			{
				networkView.RPC("SendChatMessage", RPCMode.All, User, Message);
				Message = "";
			}
		}
		
		if(Event.current.type == EventType.Repaint)
		{
			_chatBox = new Rect(Screen.width * 0.01f, Screen.height * 0.05f, Screen.width * 0.33f, Screen.height * 0.90f);
		}
	}
	
	string InsulteFilter(string message)
	{
		int i = 0, j = 0, n = 0;
		string mess = message, dummy = string.Empty;
		
		for(i = 0; i < filtre_insultes.Count; ++i)
		{
			dummy = string.Empty;
			if(mess.Contains(filtre_insultes[i].ToString()))
			{
				for(j = 0; j < filtre_insultes[i].Length; ++j)
				{ 
					n = Random.Range(0, 4);
					switch(n)
					{
						case 0: dummy += "%"; break; 	
						case 1: dummy += "$"; break;
						case 2: dummy += "?"; break;
						case 3: dummy += "^"; break;
						case 4: dummy += "*"; break;
					}
				}
				mess = mess.Replace(filtre_insultes[i].ToString(), dummy);	
			}	
		}
		return mess;
	}
	
	[RPC]
	void SendChatMessage(string user, string mess)
	{
		if(mess != "")
		{	
			mess = InsulteFilter(mess);
			_messageBox += user + " say : " + mess + "\n";
		}
	}
}
