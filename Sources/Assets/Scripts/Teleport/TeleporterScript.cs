using UnityEngine;
using System.Collections;


public class TeleporterScript : MonoBehaviour 
{   
	[SerializeField]
	private Transform _destination;
	public Transform Destination
	{
		get { return _destination; }
		set { _destination = value; }
	}

    void OnCollisionEnter(Collision col) 
    {
		if(col.collider.name == "PlayerTmp")
		{
			NetworkConnectionInitScript.myPlayer.rigidbody.position = Destination.position;
		}
	}
}