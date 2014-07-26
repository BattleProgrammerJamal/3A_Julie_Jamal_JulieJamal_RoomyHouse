using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class ObjectShatterScript : MonoBehaviour 
{   
	[SerializeField]
	private GameObject _fragments;
	public GameObject Fragments
	{
		get { return _fragments; }
		set { _fragments = value; }
	}

	void FixedUpdate()
	{
		if(Input.GetKey(KeyCode.Space))
		{
			Shatter();
		}
	}
	
	void Shatter()
	{
		Instantiate(Fragments, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}