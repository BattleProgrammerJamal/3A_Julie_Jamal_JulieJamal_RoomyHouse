using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class KeyFragCollectScript : MonoBehaviour 
{
	[SerializeField]
	private AudioSource _source;
	public AudioSource Source
	{
		get { return _source; }
		set { _source = value; }
	}
	
	[SerializeField]
	private AudioClip _sound;
	public AudioClip Sound
	{
		get { return _sound; }
		set { _sound = value; }
	}

	[SerializeField]
	private PlayerDatasScript _datas;
	public PlayerDatasScript Datas 
	{
		get { return _datas; }
		set { _datas = value; }
	}

	bool sound_run = false;
	
	void OnCollisionEnter(Collision col)
	{
		bool isMorceau = false;
		
		if(col.collider.name == "morceau_cle")
		{
			DidactitielScript script = GameObject.Find("OnLoadPlay").GetComponent<DidactitielScript>();
			if(!sound_run && script.IsStep(2))
			{
				sound_run = true;
				script.Next();
			}
			Source.PlayOneShot(Sound);
			isMorceau = true;
			Datas.NbCollectedFragments++;
		}
		
		if(isMorceau)
		{
			Network.Destroy(col.gameObject);
		}
	}
}
