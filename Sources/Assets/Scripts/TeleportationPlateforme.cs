using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class TeleportationPlateforme : MonoBehaviour 
{
	void OnTriggerEnter(Collider col)
	{
		if(col.name == "PlayerTmp") 
		{
			GameObject teleporteur = GameObject.FindGameObjectWithTag("spawnSecondFloor");
			transform.position = teleporteur.transform.position;
			Vector3 u = new Vector3(transform.position.x + 1500, transform.position.y, transform.position.z);
			transform.position = u;
		}
	}
}
