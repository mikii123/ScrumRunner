using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
	public float HP;
	public GameObject Particles;
	public GameObject Explosion;

	public void Damage(float damage, Vector3 force)
	{
		HP -= damage;
		if(HP <= 0)
		{
			Kill(force);
		}
	}

	public void Kill(Vector3 force)
	{
		if(Particles != null)
		{
			Particles.transform.SetParent(null);
			Particles.SetActive(true);
			Rigidbody[] rg = Particles.GetComponentsInChildren<Rigidbody>();
			force.Normalize();
			foreach (var rb in rg)
			{
				rb.AddForce(force * 1000);
			}
			Destroy(gameObject);
		}
		else
		{
			GameObject go = Instantiate(Explosion, transform.position, transform.rotation) as GameObject;
			Destroy(gameObject);
		}

		if(GetComponent<ThirdPersonCharacter>())
		{
			GameManager.This.StartPlay();
		}
	}
}
