using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class LightBallsCollectScript : MonoBehaviour 
{
	[SerializeField]
	private PlayerDatasScript _datas;
	public PlayerDatasScript Datas 
	{
		get { return _datas; }
		set { _datas = value; }
	}
	
	void OnCollisionEnter(Collision col)
	{
		bool isSphere = false;
		
		if(col.collider.name == "GreenSphere")
		{
			isSphere = true;
			Datas.GreenBalls++;
			Datas.AdjustHealth(5.0f);
		}
		
		if(col.collider.name == "YellowSphere")
		{
			isSphere = true;
			Datas.YellowBalls++;
			GetComponentInChildren<Light>().intensity += 1.0f;
			GetComponentInChildren<Light>().spotAngle += 3.0f;
		}
		
		if(col.collider.name == "RedSphere")
		{
			isSphere = true;
			Datas.RedBalls++;
		}
		
		if(isSphere)
		{
			Network.Destroy(col.gameObject);
		}
	}
}
