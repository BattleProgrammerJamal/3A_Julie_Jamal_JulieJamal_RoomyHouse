using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkInitLevelLoadedScript : MonoBehaviour 
{
	private string disconnectedLevel = "Lobby";
	
	void Update() 
	{
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			Network.RemoveRPCs(Network.player);
			Network.DestroyPlayerObjects(Network.player);
			Application.LoadLevel(disconnectedLevel);
		}
	}
}
