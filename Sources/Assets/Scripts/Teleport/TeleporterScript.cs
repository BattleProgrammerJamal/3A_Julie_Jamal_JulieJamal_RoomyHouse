using UnityEngine;
using System.Collections;

public class TeleporterScript : MonoBehaviour 
{   
	private Transform _destination;

	void Start()
	{
		_destination = GameObject.FindGameObjectWithTag("spawnSecondFloor").transform;
	}

	void OnCollisionEnter(Collision col)
    {
		if(col.gameObject.name == "Teleporteur")
		{
			rigidbody.position = _destination.position;
		}
	}
	
	void TeleportMe(string name)
	{
		if(name == "Teleporteur")
		{
			rigidbody.position = _destination.position;
		}
	}
}