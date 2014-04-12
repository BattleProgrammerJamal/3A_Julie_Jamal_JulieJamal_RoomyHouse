using UnityEngine;
using System.Collections;

public class InitNetworkPlayer : MonoBehaviour
{
	PlayerDatas playerDatas;
	
	void Start()
	{
		playerDatas = GetComponent<PlayerDatas>();	
	}
	
	void OnNetworkLoadedLevel()
	{
		//((PlayerDatas)playerDatas).PlayerName = PlayerPrefs.GetString("player_name");
		if(networkView.isMine)
		{
			Camera myCamera = GetComponentInChildren<Camera>();
			if(myCamera != null)
			{
				myCamera.enabled = true;	
			}
		}
	}
	
	void Update()
	{
		if(networkView.isMine)
		{
			if(transform.position.y < 0.0f)
			{
				((PlayerDatas)playerDatas).AdjustHealth(0.0f);
			}
		}
	}
}
