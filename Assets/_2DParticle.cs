using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class _2DParticle : MonoBehaviour
{
	private ParticleSystem PS;
	private Transform tr;
	private ParticleSystem.Particle[] parts;

	void Update()
	{
		Init();

		int count = PS.GetParticles(parts);

		for(int i = 0; i < count; i++)
		{
			Vector3 localPos = parts[i].position;
			Vector3 worldPos = tr.TransformPoint(localPos);
			worldPos.z = tr.position.z;
			Vector3 finalPos = tr.InverseTransformPoint(worldPos);
			parts[i].position = finalPos;
		}

		PS.SetParticles(parts, count);
	}

	void Init()
	{
		if(PS == null)
			PS = GetComponent<ParticleSystem>();
		if(tr == null)
			tr = transform;
		if (parts == null || parts.Length < PS.maxParticles)
			parts = new ParticleSystem.Particle[PS.maxParticles];
	}
}
