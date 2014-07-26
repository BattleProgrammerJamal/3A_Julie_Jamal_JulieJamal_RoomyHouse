using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class ObjectDestroyTimerScript : MonoBehaviour 
{   
	[SerializeField]
	private float _timeToWait = 1.0f;
	public float TimeToWait
	{
		get { return _timeToWait; }
		set { if(value >= 0.0) { _timeToWait = value; } }
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds(TimeToWait);
		Destroy(gameObject);
	}
}