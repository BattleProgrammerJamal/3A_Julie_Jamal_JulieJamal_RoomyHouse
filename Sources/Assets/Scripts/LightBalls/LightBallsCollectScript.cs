using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class LightBallsCollectScript : MonoBehaviour 
{
	[SerializeField]
	private AudioSource _source;
	public AudioSource Source
	{
		get { return _source; }
		set { _source = value; }
	}
	
	[SerializeField]
	private AudioClip _greenSphereSound;
	public AudioClip GreenSphereSound
	{
		get { return _greenSphereSound; }
		set { _greenSphereSound = value; }
	}

	[SerializeField]
	private AudioClip _yellowSphereSound;
	public AudioClip YellowSphereSound
	{
		get { return _yellowSphereSound; }
		set { _yellowSphereSound = value; }
	}

	[SerializeField]
	private AudioClip _redSphereSound;
	public AudioClip RedSphereSound
	{
		get { return _redSphereSound; }
		set { _redSphereSound = value; }
	}

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
			Source.PlayOneShot(GreenSphereSound);
			isSphere = true;
			Datas.GreenBalls++;
			Datas.AdjustHealth(5.0f);
		}
		
		if(col.collider.name == "YellowSphere")
		{
			Source.PlayOneShot(YellowSphereSound);
			isSphere = true;
			Datas.YellowBalls++;
			GetComponentInChildren<Light>().intensity += 1.0f;
			GetComponentInChildren<Light>().spotAngle += 3.0f;
		}
		
		if(col.collider.name == "RedSphere")
		{
			Source.PlayOneShot(RedSphereSound);
			isSphere = true;
			Datas.RedBalls++;
		}
		
		if(isSphere)
		{
			Network.Destroy(col.gameObject);
		}
	}
}
