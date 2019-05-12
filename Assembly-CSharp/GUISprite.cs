using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public sealed class GUISprite : MonoBehaviour
{
	[SerializeField]
	private Color boardColor;

	private float boardWidth;

	private float boardHeight;

	private MeshFilter meshFilter;

	private void CreateMesh(Mesh _mesh)
	{
		Vector3[] array = new Vector3[4];
		Vector3[] array2 = new Vector3[4];
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num = i * 2 + j;
				float x = this.boardWidth * ((float)j - 0.5f);
				float y = this.boardHeight * (0.5f - (float)i);
				array[num] = new Vector3(x, y, 0f);
				array2[num] = new Vector3(0f, 0f, -1f);
			}
		}
		_mesh.vertices = array;
		_mesh.normals = array2;
		_mesh.bounds = new Bounds(Vector3.zero, new Vector3(this.boardWidth, this.boardHeight, 0.2f));
		_mesh.uv = new Vector2[]
		{
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(0f, 0f),
			new Vector2(1f, 0f)
		};
		Color[] array3 = new Color[4];
		for (int k = 0; k < 4; k++)
		{
			array3[k] = this.boardColor;
		}
		_mesh.colors = array3;
		_mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
	}

	private void OnDestroy()
	{
		if (null != this.meshFilter)
		{
			UnityEngine.Object.Destroy(this.meshFilter.sharedMesh);
			this.meshFilter.sharedMesh = null;
		}
	}

	public void SetBoardSize(float width, float height)
	{
		this.boardWidth = width;
		this.boardHeight = height;
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.meshFilter.mesh = new Mesh();
		this.meshFilter.sharedMesh.hideFlags = HideFlags.DontSave;
		this.meshFilter.sharedMesh.Clear();
		this.CreateMesh(this.meshFilter.sharedMesh);
	}

	public void SetColor(Color targetColor)
	{
		if (this.boardColor != targetColor)
		{
			this.boardColor = targetColor;
			Color[] array = new Color[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = this.boardColor;
			}
			this.meshFilter.sharedMesh.colors = array;
		}
	}

	public Color GetColor()
	{
		return this.boardColor;
	}
}
