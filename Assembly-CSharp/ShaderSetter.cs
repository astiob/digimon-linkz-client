using System;
using UnityEngine;

[AddComponentMenu("GUI/ShaderSetter")]
public class ShaderSetter : MonoBehaviour
{
	private void Start()
	{
		SkinnedMeshRenderer component = base.gameObject.GetComponent<SkinnedMeshRenderer>();
		if (component != null)
		{
			for (int i = 0; i < component.materials.Length; i++)
			{
				string name = component.materials[i].shader.name;
				Shader shader = Shader.Find(name);
				if (shader != null)
				{
					component.materials[i].shader = shader;
				}
			}
		}
		else
		{
			MeshRenderer component2 = base.gameObject.GetComponent<MeshRenderer>();
			if (component2 != null)
			{
				for (int i = 0; i < component2.materials.Length; i++)
				{
					string name2 = component2.materials[i].shader.name;
					Shader shader2 = Shader.Find(name2);
					if (shader2 != null)
					{
						component2.materials[i].shader = shader2;
					}
				}
			}
		}
	}
}
