using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkInitPlayerScript : MonoBehaviour
{
	void Start()
	{
		if(networkView.isMine)
		{
			GetComponent<PlayerDatasScript>().PlayerName = PlayerPrefs.GetString("player_name");
			Camera myCamera = GetComponentInChildren<Camera>();
			if(myCamera != null)
			{
				myCamera.enabled = true;	
				myCamera.GetComponent<AudioListener>().enabled = true;
				GameObject.FindGameObjectWithTag("minimap").GetComponent<RenderCamera2D>().Cam = myCamera;
			}
		}
	}
}
