using UnityEngine;
using System.Collections;

public class ThirdPersonAnimator : MonoBehaviour
{
	public AnimationCurve Push;
	public float PushTime;
	private float curPushT;
	public float PushDistance;
	private float distance;
	private Vector3 direction;

	public ParticleSystem LeftLeg;
	public ParticleSystem RightLeg;

	private Animator Anim;
	private WeaponManager WM;
	private bool push = false;
	private Vector3 fromPushPos;

	void Start()
	{
		Anim = GetComponent<Animator>();
		distance = PushDistance;
		WM = GetComponent<WeaponManager>();
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
		}
		if(curPushT >= 1)
		{
			push = false;
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
