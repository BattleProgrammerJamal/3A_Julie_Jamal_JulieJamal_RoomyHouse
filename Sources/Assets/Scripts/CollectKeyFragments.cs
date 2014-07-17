using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class CollectKeyFragments : MonoBehaviour 
{
	[SerializeField]
	private PlayerDatas _datas;
	public PlayerDatas Datas 
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
