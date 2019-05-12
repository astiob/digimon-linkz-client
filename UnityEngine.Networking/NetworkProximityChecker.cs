using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	[RequireComponent(typeof(NetworkIdentity))]
	[AddComponentMenu("Network/NetworkProximityChecker")]
	public class NetworkProximityChecker : NetworkBehaviour
	{
		public int visRange = 10;

		public float visUpdateInterval = 1f;

		public NetworkProximityChecker.CheckMethod checkMethod;

		public bool forceHidden;

		private float m_VisUpdateTime;

		private void Update()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			if (Time.time - this.m_VisUpdateTime > this.visUpdateInterval)
			{
				base.GetComponent<NetworkIdentity>().RebuildObservers(false);
				this.m_VisUpdateTime = Time.time;
			}
		}

		public override bool OnCheckObserver(NetworkConnection newObserver)
		{
			if (this.forceHidden)
			{
				return false;
			}
			GameObject gameObject = null;
			foreach (PlayerController playerController in newObserver.playerControllers)
			{
				if (playerController != null && playerController.gameObject != null)
				{
					gameObject = playerController.gameObject;
					break;
				}
			}
			if (gameObject == null)
			{
				return false;
			}
			Vector3 position = gameObject.transform.position;
			return (position - base.transform.position).magnitude < (float)this.visRange;
		}

		public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initial)
		{
			if (this.forceHidden)
			{
				NetworkIdentity component = base.GetComponent<NetworkIdentity>();
				if (component.connectionToClient != null)
				{
					observers.Add(component.connectionToClient);
				}
				return true;
			}
			NetworkProximityChecker.CheckMethod checkMethod = this.checkMethod;
			if (checkMethod == NetworkProximityChecker.CheckMethod.Physics3D)
			{
				Collider[] array = Physics.OverlapSphere(base.transform.position, (float)this.visRange);
				foreach (Collider collider in array)
				{
					NetworkIdentity component2 = collider.GetComponent<NetworkIdentity>();
					if (component2 != null && component2.connectionToClient != null)
					{
						observers.Add(component2.connectionToClient);
					}
				}
				return true;
			}
			if (checkMethod != NetworkProximityChecker.CheckMethod.Physics2D)
			{
				return false;
			}
			Collider2D[] array3 = Physics2D.OverlapCircleAll(base.transform.position, (float)this.visRange);
			foreach (Collider2D collider2D in array3)
			{
				NetworkIdentity component3 = collider2D.GetComponent<NetworkIdentity>();
				if (component3 != null && component3.connectionToClient != null)
				{
					observers.Add(component3.connectionToClient);
				}
			}
			return true;
		}

		public override void OnSetLocalVisibility(bool vis)
		{
			NetworkProximityChecker.SetVis(base.gameObject, vis);
		}

		private static void SetVis(GameObject go, bool vis)
		{
			foreach (Renderer renderer in go.GetComponents<Renderer>())
			{
				renderer.enabled = vis;
			}
			for (int j = 0; j < go.transform.childCount; j++)
			{
				Transform child = go.transform.GetChild(j);
				NetworkProximityChecker.SetVis(child.gameObject, vis);
			}
		}

		public enum CheckMethod
		{
			Physics3D,
			Physics2D
		}
	}
}
