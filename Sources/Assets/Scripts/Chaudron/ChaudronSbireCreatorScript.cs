using UnityEngine;
using System.IO;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class ChaudronSbireCreatorScript : MonoBehaviour 
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

	[SerializeField]
	private bool _isAtGoodDistance = false;
	public bool IsAtGoodDistance
	{
		get { return _isAtGoodDistance; }
		set { _isAtGoodDistance = value; }
	}

	[SerializeField]
	private float _messageTimeOut = 3.0f;
	public float MessageTimeOut
	{
		get { return _messageTimeOut; }
		set { _messageTimeOut = value; }
	}

	private GameObject minionSpawn;
	private bool message_shown = false;

	void Start()
	{
		IsAtGoodDistance = false;
		minionSpawn = GameObject.FindGameObjectWithTag("minionSpawn");
	}
	
	void Update()
	{
		if(NetworkConnectionInitScript.myPlayer)
		{
			Vector3 vPlayer = NetworkConnectionInitScript.myPlayer.transform.position;
			Vector3 vChaudron = transform.position;

			float dst = Vector3.Distance(vPlayer, vChaudron);

			if(dst <= MinimumPlayerDistance)
			{
				IsAtGoodDistance = true;
			}
			else
			{
				IsAtGoodDistance = false;
			}
		}
	}

	void OnGUI()
	{
		if(message_shown)
		{
			float w = Screen.width, h = Screen.height;

			PlayerDatasScript datas = (PlayerDatasScript)NetworkConnectionInitScript.myPlayer.GetComponent<PlayerDatasScript>();
			GUI.Box(new Rect(w * 0.05f - 75.0f, h * 0.25f, w * 0.50f, h * 0.33f), "You have not enough red balls to create a sbire (" + datas.RedBalls.ToString() + " / 5) ! ");
		}
	}
	
	void OnMouseDown()
	{
		if(IsAtGoodDistance)
		{
			PlayerDatasScript datas = (PlayerDatasScript)NetworkConnectionInitScript.myPlayer.GetComponent<PlayerDatasScript>();
			if(datas.RedBalls >= 5)
			{
				Network.Instantiate(MinionPrefab, minionSpawn.transform.position, minionSpawn.transform.rotation, 0);
				datas.RedBalls -= 5;
			}
			else
			{
				message_shown = true;
				Invoke("CancelShowMessage", MessageTimeOut);
			}
		}
	}
	
	void OnMouseOver()
	{
		if(IsAtGoodDistance)
		{
			Cursor.SetCursor(SbireCreateTexture, Vector2.zero, CursorMode.Auto);	
		}
	}
	
	void OnMouseExit()
	{
		Cursor.SetCursor(BaseCursorTexture, Vector2.zero, CursorMode.Auto);	
	}

	void CancelShowMessage()
	{
		message_shown = false;
	}

	static void Log<T>(T data)
	{
		string url = "Log.txt";
		FileStream fstream = new FileStream(url, FileMode.Append);
		StreamWriter writer = null;

		try
		{
			writer = new StreamWriter(fstream);
			writer.WriteLine(data);
		}
		catch(System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
		finally
		{
			if(writer != null)
			{
				writer.Close();
			}
		}
	}
}
