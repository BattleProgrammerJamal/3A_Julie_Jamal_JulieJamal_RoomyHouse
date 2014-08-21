using UnityEngine;
using System.Collections;

public class SbireBehaviorScript : MonoBehaviour 
{
	public enum MinionState { IDLE, FOLLOW, ATTACK, SEARCH };
	
	[SerializeField]
	private GameObject _player;
	public GameObject Player
	{
		get { return _player; }
		set { _player = value; }
	}
	
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
	
	private bool _selected;
	
	void Start() 
	{
		if(networkView.isMine)
		{
			_selected = false;
		}
	}
	
	void FixedUpdate() 
	{
		if(networkView.isMine)
		{
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
	
	void OnMouseDown()
	{
		if(networkView.isMine)
		{
			_selected = !_selected;
		}
	}
	
	void OnGUI()
	{
		if(_selected && networkView.isMine)
		{
			GUILayout.BeginArea(new Rect(Screen.width * 0.38f, Screen.height * 0.70f, Screen.width * 0.60f, Screen.height * 0.15f));
			GUILayout.BeginHorizontal();
			
			if(GUILayout.Button(IdleButtonTexture, GUILayout.Width(64), GUILayout.Height(64)))
			{
				State = MinionState.IDLE;
				_selected = true;
			}
			
			if(GUILayout.Button(FollowButtonTexture, GUILayout.Width(64), GUILayout.Height(64)))
			{
				State = MinionState.FOLLOW;
				_selected = false;
			}
			
			if(GUILayout.Button(AttackButtonTexture, GUILayout.Width(64), GUILayout.Height(64)))
			{
				State = MinionState.ATTACK;
				_selected = false;
			}
			
			if(GUILayout.Button(SearchButtonTexture, GUILayout.Width(64), GUILayout.Height(64)))
			{
				State = MinionState.SEARCH;
				_selected = false;
			}
			
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
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
					if(!hit.collider.name.Equals(this.collider.name))
					{
						Agent.SetDestination(hit.point);
					}
				}
			}
		}
	}
	
	void Follow()
	{
		Agent.SetDestination(Player.transform.position);
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
}