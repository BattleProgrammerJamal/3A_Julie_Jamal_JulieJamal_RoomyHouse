using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class InitLevelLoaded : MonoBehaviour 
{
	private string disconnectedLevel = "Lobby";
	
	void FixedUpdate() 
	{
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			Network.RemoveRPCs(Network.player);
			Network.DestroyPlayerObjects(Network.player);
			Application.LoadLevel(disconnectedLevel);
		}
	}
}
