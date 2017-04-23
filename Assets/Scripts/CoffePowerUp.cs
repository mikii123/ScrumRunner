using UnityEngine;
using System.Collections;

public class CoffePowerUp : MonoBehaviour
{
	public float Timer;

	void OnTriggerEnter(Collider other)
	{
		if(other.attachedRigidbody != null && other.attachedRigidbody.GetComponent<ThirdPersonUserControl>())
		{
			transform.GetChild(0).gameObject.SetActive(false);
			transform.SetParent(other.attachedRigidbody.transform);
		}
	}

	void Update()
	{
		if(transform.parent != null)
		{
			GameManager.This.PlayerStats.StaminaTimer = Timer;
			Destroy(gameObject);
		}
	}
}
