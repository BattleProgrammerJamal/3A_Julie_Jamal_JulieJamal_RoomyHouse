using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class KeyFragCollectScript : MonoBehaviour 
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
		bool isMorceau = false;
		
		if(col.collider.name == "morceau_cle")
		{
			isMorceau = true;
			Datas.NbCollectedFragments++;
		}
		
		if(isMorceau)
		{
			Network.Destroy(col.gameObject);
		}
	}
}
