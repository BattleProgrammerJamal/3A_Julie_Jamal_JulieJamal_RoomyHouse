using UnityEngine;
using System.Collections;

public class RenderCameraPostProcessShaderScript : MonoBehaviour 
{
	[SerializeField]
	private Material _renderMaterial;
	public Material RenderMaterial
	{
		get { return _renderMaterial; }
		set { _renderMaterial = value; }
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, RenderMaterial);
	}
}
