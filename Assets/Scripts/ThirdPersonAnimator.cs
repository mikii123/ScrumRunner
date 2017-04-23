using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPersonAnimator : MonoBehaviour
{
	public LayerMask mask;
	public float DashSpeed = 100;
	public List<Transform> Trails = new List<Transform>();
	private List<Transform> TrailParents = new List<Transform>();
	public GameObject TrailPrefab;

	private Animator Anim;
	private Rigidbody Rigid;
	private WeaponManager WM;
	private bool push = false;
	private Vector3 fromPushPos;
	private bool once = true;

	void Start()
	{
		Anim = GetComponent<Animator>();
		WM = GetComponent<WeaponManager>();
		Rigid = GetComponent<Rigidbody>();
		foreach(var ob in Trails)
		{
			TrailParents.Add(ob.parent);
			Destroy(ob.gameObject);
		}
		Trails.Clear();
	}

	void FixedUpdate()
	{
		if(GameManager.This.PlayerStats.Stamina <= 0)
		{
			push = false;
		}

		if(push)
		{
			if(once)
			{
				fromPushPos = transform.position;
				foreach(var ob in TrailParents)
				{
					GameObject go = Instantiate(TrailPrefab, ob.position, ob.rotation) as GameObject;
					go.transform.SetParent(ob);
					go.transform.localPosition = Vector3.zero;
					go.transform.localScale = Vector3.one;
					go.transform.localRotation = Quaternion.identity;
					Trails.Add(go.transform);
				}
				once = false;
			}

			Rigid.isKinematic = true;
			transform.position = new Vector3(transform.position.x, fromPushPos.y, transform.position.z);
			transform.position += transform.forward * DashSpeed * Time.deltaTime;

			Anim.SetBool("Dash", true);
		}
		else
		{
			Rigid.isKinematic = false;
			RaycastHit hit = new RaycastHit();

			float distace = Vector3.Distance(transform.position + transform.up * 30, transform.position);

			if(Physics.Raycast(transform.position + transform.up * 30, -transform.up, out hit, distace, mask))
			{
				if(!hit.transform.GetComponent<ThirdPersonUserControl>())
					transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
					
			}
		}

		if(!push)
		{
			foreach(var ob in Trails)
			{
				if(ob != null)
					ob.SetParent(null);
			}
			Trails.Clear();
			once = true;
			Anim.SetBool("Dash", false);
		}

		Anim.SetBool("Weapon", WM.Weapon == null ? false : true);
	}

	public void UpdateAnimator(bool kick)
	{
		Anim.SetBool("Kick", kick);
	}

	public void PushForward()
	{
		GameManager.This.PlayerStats.Stamina -= Time.deltaTime * 3;
		push = true;
	}

	public void PushForward(float distance)
	{
		fromPushPos = transform.position;
		push = true;
		//LeftLeg.Play();
		//RightLeg.Play();
	}

	public void PushForward(float distance, Vector3 direction)
	{
		fromPushPos = transform.position;
		push = true;
		//LeftLeg.Play();
		//RightLeg.Play();
	}

	public void DontPush()
	{
		push = false;
	}
}
