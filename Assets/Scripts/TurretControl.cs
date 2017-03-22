using UnityEngine;
using System.Collections;

public class TurretControl : MonoBehaviour
{
	public Transform Spawn;
	public GameObject Prefab;
	public float FireRate;
	private float curFireRate = 0;
	private float speed;
	private Vector3 prevPos = Vector3.zero;

	Forge3D.F3DTurret Turret;

	void Start()
	{
		Turret = GetComponent<Forge3D.F3DTurret>();
		speed = Prefab.GetComponent<BulletTracer>().Speed;
	}

	void Update()
	{
		curFireRate += Time.deltaTime;

		if(ThirdPersonUserControl.This != null)
		{
			Turret.SetNewTarget(Forge3D.F3DPredictTrajectory.Predict(Spawn.position, ThirdPersonUserControl.This.transform.position + Vector3.up, prevPos, speed));
			prevPos = ThirdPersonUserControl.This.transform.position + Vector3.up;
		
			if(Turret.GetAngleToTarget() <= 1)
			{
				if(curFireRate >= FireRate && Vector3.Distance(transform.position, prevPos) < 50)
				{
					Fire();
				}
			}
		}
	}

	void Fire()
	{
		GameObject go = Instantiate(Prefab, Spawn.position, Spawn.rotation) as GameObject;
		BulletTracer bt = go.GetComponent<BulletTracer>();
		bt.Owner = gameObject;
		bt.Weapon = Spawn;
		curFireRate = 0;
	}
}
