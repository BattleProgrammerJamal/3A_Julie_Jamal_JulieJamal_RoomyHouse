using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class SbireCreator : MonoBehaviour 
{
	[SerializeField]
	private GameObject _minionPrefab;
	public GameObject MinionPrefab
	{
		get { return _minionPrefab; }
		set { _minionPrefab = value; }
	}
	
	[SerializeField]
	private Texture2D _baseCursorTexture;
	public Texture2D BaseCursorTexture
	{
		get { return _baseCursorTexture; }
		set { _baseCursorTexture = value; }
	}
	
	[SerializeField]
	private Texture2D _sbirecreatetexture;
	public Texture2D SbireCreateTexture
	{
		get { return _sbirecreatetexture; }
		set { _sbirecreatetexture = value; }
	}

	[SerializeField]
	private float _minimumPlayerDistance = 3.0f;
	public float MinimumPlayerDistance
	{
		get { return _minimumPlayerDistance; }
		set { _minimumPlayerDistance = value; }
	}

	private GameObject minionSpawn;
	private bool _isAtGoodDistance;

	void Start()
	{
		_isAtGoodDistance = false;
		minionSpawn = GameObject.FindGameObjectWithTag("minionSpawn");
	}
	
	void FixedUpdate()
	{
			Vector3 vPlayer = NetworkConnectionInit.myPlayer.transform.position;
			Vector3 vChaudron = this.transform.position;
			
			float dst = Vector3.Distance(vPlayer, vChaudron);
			
			if(dst <= MinimumPlayerDistance)
			{
				_isAtGoodDistance = true;
			}
	}
	
	void OnMouseDown()
	{
		if(networkView.isMine)
		{
			if(_isAtGoodDistance)
			{
				PlayerDatas datas = (PlayerDatas)NetworkConnectionInit.myPlayer.GetComponent(typeof(PlayerDatas));
				if(datas.RedBalls >= 5)
				{
					Network.Instantiate(MinionPrefab, minionSpawn.transform.position, transform.rotation, 0);
					datas.RedBalls -= 5;
				}
			}
		}
	}
	
	void OnMouseOver()
	{
		if(networkView.isMine)
		{
			Cursor.SetCursor(SbireCreateTexture, Vector2.zero, CursorMode.Auto);	
		}
	}
	
	void OnMouseExit()
	{
		if(networkView.isMine)
		{
			Cursor.SetCursor(BaseCursorTexture, Vector2.zero, CursorMode.Auto);	
		}
	}
}
