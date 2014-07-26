using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class KeyFragSpinScript : MonoBehaviour 
{
	[SerializeField]
	private float _angle;
	public float Angle 
	{
		get { return _angle; }
		set { _angle = value; }
	}
	
	void FixedUpdate()
	{
		transform.Rotate(0.0f, Time.deltaTime * Angle, 0.0f);
		Angle += Time.deltaTime;
	}
}
