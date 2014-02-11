using UnityEngine;
using System.Collections;

public class CollectPowerBall : MonoBehaviour 
{
	void Start() 
	{
	}
	
	void Update() 
	{
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.name == "Player")
		{
			Destroy(this.gameObject);
		}
	}
}
