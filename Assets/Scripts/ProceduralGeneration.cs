using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralGeneration : MonoBehaviour
{
	[System.Serializable]
	public class PowerUp
	{
		public bool Turret = false;
		public int Chance;
		public GameObject Prefab;
		[HideInInspector]
		public float random;
	}

	public List<GameObject> Prefabs = new List<GameObject>();
	public List<GameObject> StartingQueue = new List<GameObject>();
	public List<PowerUp> PowerUps = new List<ProceduralGeneration.PowerUp>();
	public int MaxBlocks;
	public bool AtStart;

	private int lastPart = 0;
	private int lastIndex = 0;

	void Start()
	{
		foreach(var ob in StartingQueue)
		{
			GameObject go = Instantiate(ob, transform.position, transform.rotation) as GameObject;
			go.transform.SetParent(transform);
			go.transform.localPosition = Vector3.zero + Vector3.left * (lastPart * 6.2f);
			go.transform.localRotation = Quaternion.identity;

			lastPart++;
		}

		lastIndex = Prefabs.Count-1;

		if(AtStart)
		{
			for(int i = lastPart; i < MaxBlocks; i++)
			{
				Randomize();
			}
		}
	}

	void Update()
	{
		if(!AtStart && Camera.main.transform != null)
		{
			int X = (int)(Camera.main.transform.position.x / 6.2f);

			if((lastPart + X) < 20 && lastPart < MaxBlocks)
			{
				Randomize();
			}
		}
	}

	void Randomize()
	{
		int rand = 99999;
		while((lastIndex - rand) < -2 || (rand == -1 && lastIndex == -1))
		{
			rand = Random.Range(-1, Prefabs.Count);
		}

		if(rand != -1)
		{
			GameObject go = Instantiate(Prefabs[rand], transform.position, transform.rotation) as GameObject;
			go.transform.SetParent(transform);
			go.transform.localPosition = Vector3.zero + Vector3.left * (lastPart * 6.2f);
			go.transform.localRotation = Quaternion.identity;

			Powerup(go.transform.position);
		}

		lastIndex = rand;
		lastPart++;
	}

	void Powerup(Vector3 position)
	{
		foreach(var pu in PowerUps)
		{
			float rand = Random.Range(0.0001f, 101);
			pu.random = (float)(pu.Chance) / rand;
		}

		PowerUp PUuse = null;
		float effect = -1;

		foreach(var pu in PowerUps)
		{
			if(pu.random > 1 && pu.random > effect)
			{
				effect = pu.random;
				PUuse = pu;
			}
		}

		if(PUuse != null)
		{
			GameObject go = Instantiate(PUuse.Prefab, position + Vector3.up * 100, PUuse.Prefab.transform.rotation) as GameObject;

			if(PUuse.Turret)
			{
				go.transform.position = new Vector3(go.transform.position.x, 14.3f, go.transform.position.z);
			}
			else
			{
				RaycastHit hit = new RaycastHit();
				if(Physics.Raycast(go.transform.position, Vector3.down, out hit, Mathf.Infinity))
				{
					go.transform.position = hit.point + Vector3.up;
				}
			}
		}
	}
}
