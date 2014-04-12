using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkConnectionInit : MonoBehaviour 
{
	[SerializeField]
	private bool _isServer = false;
	public bool IsServer
	{
		get { return _isServer; }
		set { _isServer = value; }
	}
	
	[SerializeField]
	private string _name = "Anonymous";
	public string Name
	{
		get { return _name; }
		set { _name = value; }
	}
	
	[SerializeField]
	private string _ip = "127.0.0.1";
	public string Ip
	{
		get { return _ip; }
		set { _ip = value; }
	}
	
	[SerializeField]
	private string _port = "7500";
	public string Port
	{
		get { return _port; }
		set { _port = value; }
	}
	
	[SerializeField]
	private int _maxPlayers = 2;
	public int MaxPlayers
	{
		get { return _maxPlayers; }
		set { _maxPlayers = value; }
	}
	
	[SerializeField]
	private string _numberConcurrentConnections = "2";
	public string NumberConcurrentConnections
	{
		get { return _numberConcurrentConnections; }
		set { _numberConcurrentConnections = value; }
	}
	
	[SerializeField]
	private GameObject _playerPrefab;
	public GameObject PlayerPrefab
	{
		get { return _playerPrefab; }
		set { _playerPrefab = value; }
	}
	
	Rect _window = new Rect(Screen.width*0.35f, Screen.height*0.05f, Screen.width*0.60f, Screen.height*0.90f);
	string[] levels = new string[]{ "Cuisine", "SalleDeBain" };
	string disconnectedLevel = "Lobby";
	
	private bool running = false;
	private int lastprefix = 0, numberOfConnected = 0;
	GameObject spawnPoint1, spawnPoint2;
	
	void Awake()
	{
		DontDestroyOnLoad(this);
		networkView.group = 1;
	}
	
	void Start()
	{
		Application.runInBackground = true;	
	}
	
	void OnGUI() 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();	
		}
		
		if(!running)
		{	
			if(IsServer)
			{
				_window = GUI.Window(0, _window, OnServerNetworkWindowCreate, "SERVER CONNECTION");	
			}
			else
			{
				_window = GUI.Window(0, _window, OnClientNetworkWindowCreate, "CLIENT CONNECTION");	
			}
			
			if(Event.current.type == EventType.Repaint)
			{
				_window = new Rect(Screen.width*0.35f, Screen.height*0.05f, Screen.width*0.60f, Screen.height*0.90f);
			}
		}
	}
	
	void OnServerNetworkWindowCreate(int id)
	{
		if(IsDisconnected())
		{
			GUILayout.BeginArea(new Rect(_window.width*0.05f, _window.height*0.05f, _window.width*0.85f, _window.height));
				GUILayout.Box("Server disconnected");
			GUILayout.EndArea();

			Network.InitializeSecurity();
			Network.InitializeServer(int.Parse(NumberConcurrentConnections), int.Parse(Port), !Network.HavePublicAddress());
		}
		else
		{		
			GUILayout.BeginArea(new Rect(_window.width*0.05f, _window.height*0.05f, _window.width*0.85f, _window.height));
				GUILayout.Box("Connected as " + Network.peerType.ToString() + " (" + Network.player.ipAddress + ":" +
				Network.player.port + ")");
			GUILayout.EndArea();	
		}
	}
	
	void OnClientNetworkWindowCreate(int id)
	{
		if(IsDisconnected())
		{
			GUILayout.BeginArea(new Rect(_window.width*0.05f, _window.height*0.05f, _window.width*0.85f, _window.height));
				GUILayout.Box("Client disconnected");
			
				GUILayout.BeginHorizontal();
					GUILayout.Label("Name");
					Name = GUILayout.TextField(Name);
				GUILayout.EndHorizontal();
			
				GUILayout.BeginHorizontal();
					GUILayout.Label("IP");
					Ip = GUILayout.TextField(Ip);
				GUILayout.EndHorizontal();
			
				GUILayout.BeginHorizontal();
					GUILayout.Label("Port");
					Port = GUILayout.TextField(Port);
				GUILayout.EndHorizontal();
			
				if(GUILayout.Button("Connect"))
				{
					Network.Connect(Ip, int.Parse(Port));
				}
			
			GUILayout.EndArea();
		}
		else
		{
			GUILayout.BeginArea(new Rect(_window.width*0.05f, _window.height*0.05f, _window.width*0.85f, _window.height));
				GUILayout.Box("Connected as " + Network.peerType.ToString());
			GUILayout.EndArea();	
		}
	}
	
	void OnServerInitialized()
	{
		((CreateChatBox)GetComponent<CreateChatBox>()).networkView.RPC("SendChatMessage", RPCMode.All, "Server", "Hi everyone, i'm the gentle server ! ☻");
		((CreateChatBox)GetComponent<CreateChatBox>()).User = "Server";	
	}
	
	void OnPlayerConnected(NetworkPlayer player)
	{
		if(Network.connections.Length == MaxPlayers)
		{
			Network.RemoveRPCsInGroup(0);
			Network.RemoveRPCsInGroup(1);
			
			networkView.RPC("ToggleState", RPCMode.Others, true);
			
			int n = Random.Range(0, levels.Length - 1);
			networkView.RPC("LoadClientLevel", RPCMode.Others, levels[n].ToString(), lastprefix + 1);
		}
	}
	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		networkView.RPC("ToggleState", RPCMode.Others, false);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);	
	}
	
	void OnConnectedToServer()
	{
		PlayerPrefs.SetString("player_name", Name);
		//((PlayerDatas)GetComponent<PlayerDatas>()).PlayerName = Name;
		((CreateChatBox)GetComponent<CreateChatBox>()).networkView.RPC("SendChatMessage", RPCMode.All, Name, Name + " has joined the game ! ");
		((CreateChatBox)GetComponent<CreateChatBox>()).User = Name;	
	}
	
	void OnDisconnectedFromServer()
	{
		Application.LoadLevel(disconnectedLevel);
	}
	
	bool IsDisconnected()
	{
		return Network.peerType == NetworkPeerType.Disconnected;
	}
	
	[RPC]
	IEnumerator LoadClientLevel(string level, int prefix)
	{
		lastprefix = prefix;
		
		Network.SetSendingEnabled(0, false);
		Network.isMessageQueueRunning = false;
		
		Network.SetLevelPrefix(prefix);
		Application.LoadLevel(level);	
		yield return null;
		yield return null;
		
		Network.isMessageQueueRunning = true;
		Network.SetSendingEnabled(0, true);
		
		networkView.RPC("IncNumberOfPlayer", RPCMode.All);
		
		spawnPoint1 = GameObject.FindGameObjectWithTag("spawn1");
		spawnPoint2 = GameObject.FindGameObjectWithTag("spawn2");
		
		if(numberOfConnected == 0)
		{
			SpawnPlayer(spawnPoint1.transform.position);
		}
		else
		{
			if(numberOfConnected == 1)
			{
				SpawnPlayer(spawnPoint2.transform.position);
			}
		}
		
		GameObject[] objects = (GameObject[])(FindObjectsOfType(typeof(GameObject)));
		for(int i = 0; i < objects.Length; ++i)
		{
			objects[i].SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	[RPC]
	void ToggleState(bool state)
	{
		running = state;	
	}
	
	void SpawnPlayer(Vector3 location)
	{
		Network.Instantiate(PlayerPrefab, location, Quaternion.identity, 0);
	}
	
	[RPC]
	void IncNumberOfPlayer()
	{
		numberOfConnected++;	
	}
}
