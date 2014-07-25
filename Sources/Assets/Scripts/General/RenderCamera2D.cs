using UnityEngine;
using System.Collections;

public class RenderCamera2D : MonoBehaviour 
{
	[SerializeField]
	private Camera _cam;
	public Camera Cam 
	{
		get { return _cam; }
		set { _cam = value; }
	}

	void Update()
	{
		if(Cam)
		{
			transform.LookAt(Cam.transform.position);
			Vector3 v = Vector3.Lerp(transform.position, Cam.transform.position + (-Cam.transform.forward * 2.0f), Time.fixedDeltaTime);
			Vector3 u = new Vector3(v.x, transform.position.y, v.z);
			transform.position = u;
		}
	}
}

