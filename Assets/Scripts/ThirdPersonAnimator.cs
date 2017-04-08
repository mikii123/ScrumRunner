using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPersonAnimator : MonoBehaviour
{
	public AnimationCurve Push;
	public float PushTime;
	private float curPushT;
	public float PushDistance;
	private float distance;
	private Vector3 direction;

	public List<Transform> Trails = new List<Transform>();
	private List<Transform> TrailParents = new List<Transform>();
	public GameObject TrailPrefab;

	private Animator Anim;
	private WeaponManager WM;
	private bool push = false;
	private Vector3 fromPushPos;
	private bool once = true;

	void Start()
	{
		Anim = GetComponent<Animator>();
		distance = PushDistance;
		WM = GetComponent<WeaponManager>();
		foreach(var ob in Trails)
		{
			TrailParents.Add(ob.parent);
			ob.SetParent(null);
		}
		Trails.Clear();
	}

	void Update()
	{
		curPushT += Time.deltaTime / PushTime;
		curPushT = Mathf.Clamp01(curPushT);
		if(push)
		{
			fromPushPos.y = transform.position.y;
			RaycastHit hit = new RaycastHit();

			if(Physics.Raycast(fromPushPos, direction, out hit, distance))
			{
				if(!hit.transform.GetComponent<ThirdPersonUserControl>())
					distance = Vector3.Distance(fromPushPos, hit.point);
			}
			transform.position = Vector3.Lerp(fromPushPos, fromPushPos + direction * distance, Push.Evaluate(curPushT));

			if(once)
			{
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
		}

		if(curPushT >= 1)
		{
			push = false;
		}

		if(!push)
		{
			foreach(var ob in Trails)
			{
				ob.SetParent(null);
			}
			Trails.Clear();
			once = true;
		}

		Anim.SetBool("Weapon", WM.Weapon == null ? false : true);
	}

	public void UpdateAnimator(bool kick)
	{
		Anim.SetBool("Kick", kick);
	}

	public void PushForward()
	{
		curPushT = 0;
		this.distance = PushDistance;
		this.direction = transform.forward;
		fromPushPos = transform.position;
		push = true;
		//LeftLeg.Play();
		//RightLeg.Play();
	}

	public void PushForward(float distance)
	{
		curPushT = 0;
		this.distance = distance;
		this.direction = transform.forward;
		fromPushPos = transform.position;
		push = true;
		//LeftLeg.Play();
		//RightLeg.Play();
	}

	public void PushForward(float distance, Vector3 direction)
	{
		curPushT = 0;
		this.distance = distance;
		this.direction = direction;
		fromPushPos = transform.position;
		push = true;
		//LeftLeg.Play();
		//RightLeg.Play();
	}
}
