using UnityEngine;
using System.Collections;

public class FXFlyingTrianglesScript : MonoBehaviour 
{
	[SerializeField]
	private Material _triMaterial;
	public Material TriMaterial
	{
		get { return _triMaterial; }
		set { _triMaterial = value; }
	}

	[SerializeField]
	private int _numberOfTriangles = 50;
	public int NumberOfTriangles
	{
		get { return _numberOfTriangles; }
		set { _numberOfTriangles = value; }
	}

	[SerializeField]
	private float _timeout = 2.0f;
	public float Timeout
	{
		get { return _timeout; }
		set { _timeout = value; }
	}

	[SerializeField]
	private float _rotationSpeed = 1.0f;
	public float RotationSpeed
	{
		get { return _rotationSpeed; }
		set { _rotationSpeed = value; }
	}

	private GameObject[] triangles;
	private float speed;

	public void Fly() 
	{
		((MeshRenderer)GetComponentInChildren(typeof(MeshRenderer))).enabled = false;
		triangles = AddTriangles(gameObject, TriMaterial);
		Invoke("DestroyAfterTimeout", Timeout);
	}

	void FixedUpdate()
	{
		if(triangles != null)
		{
			for(int i = 0; i < triangles.Length; ++i)
			{
				if(Random.Range(0, 100) < 10)
				{
					RotationSpeed += Random.Range(0.0f, 0.5f);
				}

				triangles[i].transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), Time.deltaTime * RotationSpeed);
				triangles[i].transform.Translate(Vector3.up * Time.deltaTime);
			}
		}
	}

	void DestroyAfterTimeout()
	{
		//gameObject.SetActive(false);
		Color col = gameObject.renderer.material.color;
		col.a = 0.0f;
		gameObject.renderer.material.color = col;
	}

	GameObject[] AddTriangles(GameObject root, Material mat)
	{
		GameObject[] tris = new GameObject[NumberOfTriangles];

		for(int i = 0; i < tris.Length; ++i)
		{
			tris[i] = CreateTriangle("Triangle" + i, mat);

			tris[i].transform.position = root.transform.position;
			tris[i].transform.rotation = root.transform.rotation;
			tris[i].transform.position = root.transform.position + (new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2)));
			tris[i].transform.parent = root.transform;
		}

		return tris;
	}

	GameObject CreateTriangle(string name, Material mat)
	{
		Mesh mesh = new Mesh();
		
		mesh.vertices = new Vector3[]
		{
			Vector3.zero,
			Vector3.up,
			Vector3.right
		};
		
		mesh.SetIndices(
			new int[]
			{
				0, 1, 2	
			}, 
			MeshTopology.Triangles,
			0
		);

		mesh.uv = new Vector2[3];
		mesh.normals = new Vector3[3];
		mesh.tangents = new Vector4[3];

		GameObject go = new GameObject(name);

		MeshFilter mfilter = (MeshFilter)go.AddComponent(typeof(MeshFilter));
		mfilter.mesh = mesh;

		MeshRenderer mrenderer = (MeshRenderer)go.AddComponent(typeof(MeshRenderer));
		mrenderer.material = mat;

		go.transform.localScale /= 3;

		return go;
	}
}
