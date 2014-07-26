using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class SbireBehaviorScript : MonoBehaviour 
{
	public enum MinionState { IDLE, FOLLOW, ATTACK, SEARCH };
	
	[SerializeField]
	private MinionState _state = MinionState.IDLE;
	public MinionState State
	{
		get { return _state; }
		set { _state = value; }
	}

	[SerializeField]
	private NavMeshAgent _agent;
	public NavMeshAgent Agent
	{
		get { return _agent; }
		set { _agent = value; }
	}
	
	[SerializeField]
	private Texture _idleButtonTexture;
	public Texture IdleButtonTexture
	{
		get { return _idleButtonTexture; }
		set { _idleButtonTexture = value; }
	}
	
	[SerializeField]
	private Texture _followButtonTexture;
	public Texture FollowButtonTexture
	{
		get { return _followButtonTexture; }
		set { _followButtonTexture = value; }
	}
	
	[SerializeField]
	private Texture _attackButtonTexture;
	public Texture AttackButtonTexture
	{
		get { return _attackButtonTexture; }
		set { _attackButtonTexture = value; }
	}
	
	[SerializeField]
	private Texture _searchButtonTexture;
	public Texture SearchButtonTexture
	{
		get { return _searchButtonTexture; }
		set { _searchButtonTexture = value; }
	}

	private GameObject _player;
	private bool _selected;
   
    void Start() 
    {
		if(networkView.isMine)
		{
			_selected = true;
			_player = NetworkConnectionInitScript.myPlayer;
		}
    }
    
    void FixedUpdate() 
    {
		if(networkView.isMine)
		{
			HandlePicking();
			
			switch(State)
			{
				case MinionState.IDLE:
					Idle();
				break;
				
				case MinionState.FOLLOW:
					Follow();
				break;
				
				case MinionState.ATTACK:
					Attack();
				break;
				
				case MinionState.SEARCH:
					Search();
				break;
				
				default:
					Idle();
				break;
			}
		}
    }
	
	void OnGUI()
	{
		if(_selected)
		{
			GUILayout.BeginArea(new Rect(Screen.width * 0.38f, Screen.height * 0.85f, Screen.width * 0.60f, Screen.height * 0.15f));
			GUILayout.BeginHorizontal();
			
			bool deselect = true;
			
			if(GUILayout.Button(IdleButtonTexture, GUILayout.Width(64), GUILayout.Height(64)))
			{
				State = MinionState.IDLE;
				deselect = false;
			}
		
			if(GUILayout.Button(FollowButtonTexture, GUILayout.Width(64), GUILayout.Height(64)))
			{
				State = MinionState.FOLLOW;
			}
			
			if(GUILayout.Button(AttackButtonTexture, GUILayout.Width(64), GUILayout.Height(64)))
			{
				State = MinionState.ATTACK;
			}
			
			if(GUILayout.Button(SearchButtonTexture, GUILayout.Width(64), GUILayout.Height(64)))
			{
				State = MinionState.SEARCH;
			}
			
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			
			if(deselect){ _selected = false; }
		}
	}
	
	void Idle()
	{
		if(_selected)
		{
			RaycastHit hit;
			
			if(Input.GetMouseButtonDown(0)) 
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if(Physics.Raycast(ray, out hit))
				{
					if(hit.collider.name != this.collider.name)
					{
						Agent.SetDestination(hit.point);
					}
				}
			}
		}
	}
	
	void Follow()
	{
		Agent.SetDestination(_player.transform.position);
	}
	
	void Attack()
	{
	}
	
	void Search()
	{
		GameObject[] redBalls = GameObject.FindGameObjectsWithTag("RedSphere");
		GameObject[] yellowBalls = GameObject.FindGameObjectsWithTag("YellowSphere");
		GameObject[] greenBalls = GameObject.FindGameObjectsWithTag("GreenSphere");
		GameObject[] keyFragments = GameObject.FindGameObjectsWithTag("morceau_cle");
		
		int l = (redBalls.Length + yellowBalls.Length + greenBalls.Length + keyFragments.Length), i = 0,
		index = 0;
		GameObject[] objects = new GameObject[l];
		
		for(i = 0; i < redBalls.Length; ++i){ objects[index] = redBalls[i]; ++index; }
		for(i = 0; i < yellowBalls.Length; ++i){ objects[index] = yellowBalls[i]; ++index; }
		for(i = 0; i < greenBalls.Length; ++i){ objects[index] = greenBalls[i]; ++index; }
		for(i = 0; i < keyFragments.Length; ++i){ objects[index] = keyFragments[i]; ++index; }

		/*
		bool found = false;
		
		for(i = 0; i < objects.Length; ++i)
		{
			
		}
		*/
	}
	
	void HandlePicking()
	{
		RaycastHit hit;
		
        if(Input.GetMouseButtonDown(0)) 
		{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
            if(Physics.Raycast(ray, out hit))
			{
                if(hit.collider.name == this.collider.name)
				{
					_selected = true;
				}
				else
				{
					_selected = false;
				}
			}
		}
	}
}