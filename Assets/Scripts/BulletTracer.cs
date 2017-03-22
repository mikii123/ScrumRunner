using UnityEngine;
using System.Collections;

public class BulletTracer : MonoBehaviour
{
	public GameObject Owner;
	public Transform Weapon;
	public float Distance;
	public float Speed;
	public float Damage;
	public LayerMask Mask;
	public GameObject Muzzle;
	public GameObject Impact;

	private Vector3 startPos;
	private float curDistance;

	private Transform tr;

	private RaycastHit HitUse = new RaycastHit();

	void Start()
	{
		tr = transform;
		startPos = tr.position;
		GameObject go = Instantiate(Muzzle, tr.position, tr.rotation) as GameObject;
		go.transform.SetParent(Weapon);
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;

		RaycastHit[] hits = Physics.RaycastAll(tr.position - tr.forward * 2, tr.forward, Vector3.Distance(tr.position - tr.forward * 2, tr.position), Mask.value);
		float dist = Mathf.Infinity;
		bool found = false;
		foreach(var hit in hits)
		{
			if(Vector3.Distance(hit.point, tr.position) < dist && hit.transform.gameObject != Owner)
			{
				HitUse = hit;
				dist = Vector3.Distance(hit.point, tr.position);
				found = true;
			}
		}

		if(found)
		{
			GameObject go2 = Instantiate(Impact, HitUse.point + HitUse.normal * 0.1f, Quaternion.LookRotation(HitUse.normal)) as GameObject;
			Destroy(gameObject);
		}
	}

	void Update()
	{
		if(Vector3.Distance(tr.position, startPos) >= Distance)
		{
			Destroy(gameObject);
		}
		else
		{
			RaycastHit[] hits = Physics.RaycastAll(tr.position, tr.forward, Speed * Time.deltaTime, Mask.value);
			float dist = Mathf.Infinity;
			bool found = false;
			foreach(var hit in hits)
			{
				if(Vector3.Distance(hit.point, tr.position) < dist && hit.transform.gameObject != Owner)
				{
					HitUse = hit;
					dist = Vector3.Distance(hit.point, transform.position);
					found = true;
				}
			}

			if(found)
			{
				GameObject go = Instantiate(Impact, HitUse.point + HitUse.normal * 0.1f, Quaternion.LookRotation(HitUse.normal)) as GameObject;
				Destroy(gameObject);
			}
			else
			{
				Vector3 forw = tr.forward;
				forw.z = 0;
				tr.forward = forw;
				tr.position += tr.forward * (Speed * Time.deltaTime);
				Vector3 pos = transform.position;
				pos.z = 0;
				tr.position = pos;
			}
		}
	}
}
