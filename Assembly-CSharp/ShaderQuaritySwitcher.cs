using System;
using UnityEngine;

public sealed class ShaderQuaritySwitcher
{
	private const string quarityPassHigh = "High";

	private const string quarityPassMiddle = "Middle";

	private const string quarityPassLow = "Low";

	private static string[] supportShaderPass = new string[]
	{
		"Toon/{0}/Directional",
		"Toon/{0}/Directional (Grass)",
		"Toon/{0}/Directional (Scroll)",
		"Digimon/Chara{0}",
		"Digimon/CharaAlpha{0}",
		"Digimon/CharaGrass{0}"
	};

	public static void SetQuarity(ShaderQuarity quarity, Material material)
	{
		material.shader = ShaderQuaritySwitcher.GetSpecifiedQuarityShader(material.shader, quarity);
	}

	public static void SetQuarity(ShaderQuarity quarity, GameObject rootObject)
	{
		Renderer[] componentsInChildren = rootObject.GetComponentsInChildren<Renderer>();
		ShaderQuaritySwitcher.SetQuarity(quarity, componentsInChildren);
	}

	public static void SetQuarity(ShaderQuarity quarity, params Renderer[] renderers)
	{
		foreach (Renderer renderer in renderers)
		{
			if (!(renderer == null))
			{
				for (int j = 0; j < renderer.sharedMaterials.Length; j++)
				{
					if (!(renderer.sharedMaterials[j] == null))
					{
						Shader specifiedQuarityShader = ShaderQuaritySwitcher.GetSpecifiedQuarityShader(renderer.sharedMaterials[j].shader, quarity);
						if (!renderer.sharedMaterials[j].shader.name.Equals(specifiedQuarityShader.name))
						{
							renderer.materials[j].shader = specifiedQuarityShader;
						}
					}
				}
			}
		}
	}

	public static Shader GetSpecifiedQuarityShader(Shader shader, ShaderQuarity quarity)
	{
		string text = shader.name.Replace("High", "{0}");
		text = text.Replace("Middle", "{0}");
		text = text.Replace("Low", "{0}");
		string[] array = ShaderQuaritySwitcher.ShaderStringSplit(text);
		for (int i = 0; i < ShaderQuaritySwitcher.supportShaderPass.Length; i++)
		{
			string[] array2 = ShaderQuaritySwitcher.ShaderStringSplit(ShaderQuaritySwitcher.supportShaderPass[i]);
			if (array2[0].Equals(array[0]) && array2[1].Equals(array[1]))
			{
				return Shader.Find(string.Format(ShaderQuaritySwitcher.supportShaderPass[i], ShaderQuaritySwitcher.ShaderQuarityToString(quarity)));
			}
		}
		return shader;
	}

	private static string[] ShaderStringSplit(string shaderPass)
	{
		string text = shaderPass.Replace("{0}", "#");
		return text.Split(new char[]
		{
			'#'
		});
	}

	private static string ShaderQuarityToString(ShaderQuarity quarity)
	{
		if (quarity == ShaderQuarity.High)
		{
			return "High";
		}
		if (quarity != ShaderQuarity.Middle)
		{
			return "Low";
		}
		return "Middle";
	}
}
