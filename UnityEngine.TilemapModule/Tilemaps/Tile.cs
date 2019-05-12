using System;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	[RequiredByNativeCode]
	[Serializable]
	public class Tile : TileBase
	{
		[SerializeField]
		private Sprite m_Sprite;

		[SerializeField]
		private Color m_Color = Color.white;

		[SerializeField]
		private Matrix4x4 m_Transform = Matrix4x4.identity;

		[SerializeField]
		private GameObject m_InstancedGameObject;

		[SerializeField]
		private TileFlags m_Flags = TileFlags.LockColor;

		[SerializeField]
		private Tile.ColliderType m_ColliderType = Tile.ColliderType.Sprite;

		public Sprite sprite
		{
			get
			{
				return this.m_Sprite;
			}
			set
			{
				this.m_Sprite = value;
			}
		}

		public Color color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				this.m_Color = value;
			}
		}

		public Matrix4x4 transform
		{
			get
			{
				return this.m_Transform;
			}
			set
			{
				this.m_Transform = value;
			}
		}

		public GameObject gameObject
		{
			get
			{
				return this.m_InstancedGameObject;
			}
			set
			{
				this.m_InstancedGameObject = value;
			}
		}

		public TileFlags flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		public Tile.ColliderType colliderType
		{
			get
			{
				return this.m_ColliderType;
			}
			set
			{
				this.m_ColliderType = value;
			}
		}

		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			tileData.sprite = this.m_Sprite;
			tileData.color = this.m_Color;
			tileData.transform = this.m_Transform;
			tileData.gameObject = this.m_InstancedGameObject;
			tileData.flags = this.m_Flags;
			tileData.colliderType = this.m_ColliderType;
		}

		public enum ColliderType
		{
			None,
			Sprite,
			Grid
		}
	}
}
