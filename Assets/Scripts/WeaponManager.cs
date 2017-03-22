using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
	public Weapon Weapon;
	public Transform Hand;

	private RootMotion.FinalIK.AimIK AimIK;

	void Start()
	{
		AimIK = GetComponent<RootMotion.FinalIK.AimIK>();
		AimIK.GetIKSolver().SetIKPositionWeight(0);
	}

	void Update()
	{
		if(Input.GetMouseButton(0) && Weapon != null)
		{
			Weapon.Fire(gameObject);
		}
	}

	public void Equip(Weapon weapon)
	{
		weapon.Equip(Hand);
		Weapon = weapon;
		AimIK.solver.transform = weapon.transform;
		AimIK.GetIKSolver().SetIKPositionWeight(1);
	}

	public void UnEquipAll()
	{
		
	}
}
