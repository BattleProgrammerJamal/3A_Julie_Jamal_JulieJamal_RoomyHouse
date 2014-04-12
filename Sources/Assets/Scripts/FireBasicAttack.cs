using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class FireBasicAttack : MonoBehaviour 
{
	[SerializeField]
	public Transform _transform;
	public GameObject _projectile;
	public KeyCode _shortcut = KeyCode.V;
	public float _reloadTime = 0.5f;
	
	bool timeout = true;
	GameObject clone;
	
	void FixedUpdate() 
	{
			if(Input.GetKey(_shortcut))
			{
				Invoke("ProjectileCreation", 0.01f);
			}
	}
	
	void ProjectileCreation()
	{
		if(timeout)
		{
			timeout = false;
			
			Vector3 new_position = _transform.position;
			new_position.z += 1.0f;
			clone = (GameObject)Instantiate(_projectile, new_position, _transform.rotation);
			
			clone.rigidbody.AddRelativeForce(Vector3.forward * Time.deltaTime * 70000);
			Invoke("ProjectileDestruction", _reloadTime * 0.8f);
			Invoke("Reload", _reloadTime);
		}
	}
	
	void ProjectileDestruction()
	{
		Destroy(clone);
	}
	
	void Reload()
	{
		timeout = true;	
	}
}
