using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkInitPlayerScript : MonoBehaviour
{
	void OnNetworkLoadedLevel()
	{
		if(networkView.isMine)
		{
			GetComponent<PlayerDatasScript>().PlayerName = PlayerPrefs.GetString("player_name");
			Camera myCamera = GetComponentInChildren<Camera>();
			if(myCamera != null)
			{
				myCamera.enabled = true;	
				myCamera.GetComponent<AudioListener>().enabled = true;
			}
		}
	}
}
