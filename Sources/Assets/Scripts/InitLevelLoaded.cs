using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class InitLevelLoaded : MonoBehaviour 
{
	string disconnectedLevel = "Lobby";
	
	void Update() 
	{
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			Network.RemoveRPCs(Network.player);
			Network.DestroyPlayerObjects(Network.player);
			Network.Disconnect();
			Application.LoadLevel(disconnectedLevel);
		}
	}
}
