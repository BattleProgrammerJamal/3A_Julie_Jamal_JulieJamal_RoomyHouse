using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class TeleportationPlateforme : MonoBehaviour 
{
	[SerializeField]
	private GameObject _player;
	public GameObject Player
	{
		get { return _player; }
		set { _player = value; }
	}

	void OnCollisionEnter(Collision col)
	{
		if(networkView.isMine && col.collider.name == "PlayerTmp") 
		{
			GameObject teleporteur = GameObject.FindGameObjectWithTag("spawnSecondFloor");
			Player.transform.position = teleporteur.transform.position;
			Debug.Log("NETWORKVIEW.ISMINE AND TRIGGER ENTER");
		}
		Debug.Log("TRIGGER ENTER");
	}
}
