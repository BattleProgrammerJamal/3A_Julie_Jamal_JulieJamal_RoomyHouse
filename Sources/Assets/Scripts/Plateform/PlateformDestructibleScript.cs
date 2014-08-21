using UnityEngine;
using System.Collections;

public class PlateformDestructibleScript : MonoBehaviour 
{
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.name.Equals("Projectile(Clone)"))
		{
			rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			Component[] children = GetComponentsInChildren(typeof(Object));
			
			for(int i = 0; i < children.Length; ++i)
			{
				children[i].rigidbody.useGravity = true;
			}
		}
	}
}
