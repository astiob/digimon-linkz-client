using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkProximityChecker")]
	[RequireComponent(typeof(NetworkIdentity))]
	public class NetworkProximityChecker : NetworkBehaviour
	{
		[Tooltip("The maximum range that objects will be visible at.")]
		public int visRange = 10;

		[Tooltip("How often (in seconds) that this object should update the set of players that can see it.")]
		public float visUpdateInterval = 1f;

		[Tooltip("Which method to use for checking proximity of players.\n\nPhysics3D uses 3D physics to determine proximity.\n\nPhysics2D uses 2D physics to determine proximity.")]
		public NetworkProximityChecker.CheckMethod checkMethod = NetworkProximityChecker.CheckMethod.Physics3D;

		[Tooltip("Enable to force this object to be hidden from players.")]
		public bool forceHidden = false;

		private float m_VisUpdateTime;

		private void Update()
		{
			if (NetworkServer.active)
			{
				if (Time.time - this.m_VisUpdateTime > this.visUpdateInterval)
				{
					base.GetComponent<NetworkIdentity>().RebuildObservers(false);
					this.m_VisUpdateTime = Time.time;
				}
			}
		}

		public override bool OnCheckObserver(NetworkConnection newObserver)
		{
			bool result;
			if (this.forceHidden)
			{
				result = false;
			}
			else
			{
				GameObject gameObject = null;
				for (int i = 0; i < newObserver.playerControllers.Count; i++)
				{
					PlayerController playerController = newObserver.playerControllers[i];
					if (playerController != null && playerController.gameObject != null)
					{
						gameObject = playerController.gameObject;
						break;
					}
				}
				if (gameObject == null)
				{
					result = false;
				}
				else
				{
					Vector3 position = gameObject.transform.position;
					result = ((position - base.transform.position).magnitude < (float)this.visRange);
				}
			}
			return result;
		}

		public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initial)
		{
			bool result;
			if (this.forceHidden)
			{
				NetworkIdentity component = base.GetComponent<NetworkIdentity>();
				if (component.connectionToClient != null)
				{
					observers.Add(component.connectionToClient);
				}
				result = true;
			}
			else
			{
				NetworkProximityChecker.CheckMethod checkMethod = this.checkMethod;
				if (checkMethod != NetworkProximityChecker.CheckMethod.Physics3D)
				{
					if (checkMethod != NetworkProximityChecker.CheckMethod.Physics2D)
					{
						result = false;
					}
					else
					{
						foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(base.transform.position, (float)this.visRange))
						{
							NetworkIdentity component2 = collider2D.GetComponent<NetworkIdentity>();
							if (component2 != null && component2.connectionToClient != null)
							{
								observers.Add(component2.connectionToClient);
							}
						}
						result = true;
					}
				}
				else
				{
					foreach (Collider collider in Physics.OverlapSphere(base.transform.position, (float)this.visRange))
					{
						NetworkIdentity component3 = collider.GetComponent<NetworkIdentity>();
						if (component3 != null && component3.connectionToClient != null)
						{
							observers.Add(component3.connectionToClient);
						}
					}
					result = true;
				}
			}
			return result;
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
