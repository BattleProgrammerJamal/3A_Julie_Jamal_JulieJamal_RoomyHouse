using UnityEngine;
using System.Collections;


public class TeleporterScript : MonoBehaviour 
{   
	[SerializeField]
	private GameObject _playerPrefab;
	public GameObject PlayerPrefab
	{
		get { return _playerPrefab; }
		set { _playerPrefab = value; }
	}

	[SerializeField]
	private Transform _destination;
	public Transform Destination
	{
		get { return _destination; }
		set { _destination = value; }
	}

    void OnTriggerEnter(Collider col) 
    {
		if(col.gameObject.name == "myPlayer")
		{
			PlayerPrefab.rigidbody.position = Destination.position;
		}
	}
}