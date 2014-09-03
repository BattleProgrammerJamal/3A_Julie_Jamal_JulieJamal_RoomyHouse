using UnityEngine;
using System.Collections;

public class GollumScreamScript : MonoBehaviour 
{
	[SerializeField]
	private GameObject _area;
	public GameObject Area
	{
		get { return _area; }
		set { _area = value; }
	}

	[SerializeField]
	private float _maxDistance;
	public float MaxDistance
	{
		get { return _maxDistance; }
		set { _maxDistance = value; }
	}

	[SerializeField]
	private AudioClip _soundGollum;
	public AudioClip SoundGollum
	{
		get { return _soundGollum; }
		set { _soundGollum = value; }
	}

	public bool Dispo = true;

	void Update()
	{
		if(Vector3.Distance(this.transform.position, Area.transform.position) <= MaxDistance && Dispo)
		{
			audio.PlayOneShot(SoundGollum);
			Dispo = false;
			Invoke("SoundDispo", 3.0f);
		}
	}

	void SoundDispo()
	{
		Dispo = true;
	}
}
