using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public float FireRate;
	public GameObject Bullet;
	public Transform Spawn;
	public Vector3 EquipPos;
	public Vector3 EquipRot;

	public bool Equipped = false;

	private float curFire = 0;
	private AudioSource AS;

	void Start()
	{
		AS = GetComponent<AudioSource>();
	}

	void Update()
	{
		curFire += Time.deltaTime;

		if(transform.parent == null)
			Equipped = false;
		else
			Equipped = true;

		if(!Equipped)
		{
			transform.rotation = Quaternion.Euler(0, 90, 0);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.transform.GetComponent<ThirdPersonUserControl>())
		{
			WeaponManager wm = other.transform.GetComponent<WeaponManager>();
			if(!Equipped)
			{
				if(wm.Weapon != null)
					Destroy(wm.Weapon.gameObject);
				wm.Equip(this);
			}
		}
	}

	public void Equip(Transform tr)
	{
		transform.SetParent(tr);
		transform.localPosition = EquipPos;
		transform.localRotation = Quaternion.Euler(EquipRot);
	}

	public void Fire(GameObject Owner)
	{
		if(curFire >= FireRate)
		{
			curFire = 0;

			GameObject go = Instantiate(Bullet, Spawn.position, Spawn.rotation) as GameObject;
			BulletTracer bt = go.GetComponent<BulletTracer>();
			bt.Owner = Owner;
			bt.Weapon = Spawn;
			AS.Play();
		}
	}
}
