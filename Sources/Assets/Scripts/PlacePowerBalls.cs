using UnityEngine;
using System.Collections;

public class PlacePowerBalls : MonoBehaviour 
{
	[SerializeField]
	public GameObject _prefab;
	public int _max;
	public Transform _transform;
	
	void Start()
	{
		Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
		int i = 0;
		for(i = 0; i < _max; ++i)
		{
			pos.Set(Random.Range(-60, 60) + _transform.position.x + i * 10, 13.8f, Random.Range(-40, 40) + _transform.position.z + i * 10);
			Instantiate(_prefab, pos, Quaternion.identity);
		}
	}
	
	void Update() 
	{
	}
}
