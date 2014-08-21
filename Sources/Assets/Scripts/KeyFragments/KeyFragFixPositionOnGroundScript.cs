using UnityEngine;
using System.Collections;

public class KeyFragFixPositionOnGroundScript : MonoBehaviour 
{
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.name.Equals("Floor"))
		{
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
	}
}
