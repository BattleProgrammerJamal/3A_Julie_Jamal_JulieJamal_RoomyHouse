using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkConnectionInitScript : MonoBehaviour 
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
		set { if(value > 0) { _maxPlayers = value; } }
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
	
	[SerializeField]
	private Texture _background;
	public Texture Background
	{
		get { return _background; }
		set { _background = value; }
	}
	
	private Rect _window = new Rect(Screen.width*0.35f, Screen.height*0.05f, Screen.width*0.60f, Screen.height*0.90f);
	private string[] levels = new string[]{ "Cuisine", "SalleDeBain" };
	private string disconnectedLevel = "Lobby";
	
	private bool playing = false;
	private int lastprefix = 0, numberOfPlayers = 0;
	private GameObject spawnPoint1, spawnPoint2;

	public static GameObject myPlayer;
	
	void Awake()
	{
		DontDestroyOnLoad(this);
		networkView.group = 1;
		Application.runInBackground = true;
		if(PlayerPrefs.GetString("isClient") == "True")
		{
			IsServer = false;	
		}
		else
		{
			IsServer = true;
			AudioListener.volume = 0.0f;
		}
	}
	
	void OnGUI() 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			Network.RemoveRPCs(Network.player);
			Network.DestroyPlayerObjects(Network.player);
			Network.Disconnect();
			Application.LoadLevel("Menu");
		}
		
		if(IsServer)
		{
			if(Network.peerType == NetworkPeerType.Server)
			{
				networkView.RPC("SyncNumberOfPlayers", RPCMode.All, Network.connections.Length);
			}
		}
		
		if(!playing)
		{	
			GUI.DrawTexture(new Rect(0.0f, 0.0f, Screen.width, Screen.height), Background);
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
			
			if(!IsServer)
			{
				GUI.Label(new Rect(Screen.width * 0.33f, 1.0f, 100.0f, 20.0f), numberOfPlayers.ToString() + " / 2 players");
			}
		}
	}
	
	void OnServerNetworkWindowCreate(int id)
	{
		if(Network.peerType == NetworkPeerType.Disconnected)
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
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			GUILayout.BeginArea(new Rect(_window.width*0.05f, _window.height*0.05f, _window.width*0.85f, _window.height));
				GUILayout.Box("Client disconnected");
			
				GUILayout.BeginHorizontal();
					GUILayout.Label("Name");
					Name = GUILayout.TextField(Name, 15);
				GUILayout.EndHorizontal();
			
				GUILayout.BeginHorizontal();
					GUILayout.Label("IP");
					Ip = GUILayout.TextField(Ip, 20);
				GUILayout.EndHorizontal();
			
				GUILayout.BeginHorizontal();
					GUILayout.Label("Port");
					Port = GUILayout.TextField(Port, 10);
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
		((GeneralCreateChatBoxScript)GetComponent<GeneralCreateChatBoxScript>()).networkView.RPC("SendChatMessage", RPCMode.AllBuffered, "Server", "Server Initialized\n");
		((GeneralCreateChatBoxScript)GetComponent<GeneralCreateChatBoxScript>()).User = "Server";	
	}
	
	void OnPlayerConnected(NetworkPlayer player)
	{
		if(numberOfPlayers == MaxPlayers - 1)
		{
			Network.RemoveRPCsInGroup(0);
			Network.RemoveRPCsInGroup(1);
			
			int n = Random.Range(0, levels.Length);
			n = 0;
			networkView.RPC("LoadClientLevel", RPCMode.Others, levels[n].ToString(), lastprefix + 1);
		}
	}
	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		networkView.RPC("ClientForceQuit", RPCMode.Others);	
	}
	
	void OnConnectedToServer()
	{
		playing = true;
		((GeneralCreateChatBoxScript)GetComponent<GeneralCreateChatBoxScript>()).networkView.RPC("SendChatMessage", RPCMode.AllBuffered, Name, Name + " has joined the game ! ");
		((GeneralCreateChatBoxScript)GetComponent<GeneralCreateChatBoxScript>()).User = Name;
	}
	
	void OnDisconnectedFromServer()
	{
		playing = false;
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
		Application.LoadLevel(disconnectedLevel);
	}
	
	[RPC]
	void SyncNumberOfPlayers(int n)
	{
		numberOfPlayers = n;
	}
	
	[RPC]
	void ClientForceQuit()
	{
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
		Network.Disconnect();
		Application.LoadLevel(disconnectedLevel);
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
		
		spawnPoint1 = GameObject.FindGameObjectWithTag("spawn1");
		spawnPoint2 = GameObject.FindGameObjectWithTag("spawn2");
		
		((GeneralCreateChatBoxScript)GetComponent<GeneralCreateChatBoxScript>()).SizeH = 0.50f;
		((GeneralCreateChatBoxScript)GetComponent<GeneralCreateChatBoxScript>()).OffsetX = 0.01f;
		((GeneralCreateChatBoxScript)GetComponent<GeneralCreateChatBoxScript>()).OffsetY = 0.45f;
		
		PlayerPrefs.SetString("player_name", Name);
		
		if(numberOfPlayers == 0)
		{
			SpawnPlayer(1, spawnPoint1.transform.position);
		}
		else
		{
			if(numberOfPlayers == 1)
			{
				SpawnPlayer(2, spawnPoint2.transform.position);
			}
		}
		
		AudioSource[] audioSources = (AudioSource[])FindObjectsOfType(typeof(AudioSource));
		for(int i = 0; i < audioSources.Length; ++i)
		{
			audioSources[i].Stop();
		}
		
		GameObject[] objects = (GameObject[])(FindObjectsOfType(typeof(GameObject)));
		for(int i = 0; i < objects.Length; ++i)
		{
			objects[i].SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void SpawnPlayer(int id, Vector3 location)
	{
		Object player = Network.Instantiate(PlayerPrefab, location, Quaternion.identity, 0);
		myPlayer = (GameObject)player;
		//	myPlayer.name = "player" + id.ToString ();
		myPlayer.name = "myPlayer";
	}
}
