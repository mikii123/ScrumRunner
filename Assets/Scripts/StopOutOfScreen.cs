using UnityEngine;
using System.Collections;

public class StopOutOfScreen : MonoBehaviour
{
	private MeshRenderer mr;
	private Rigidbody rb;

	void Start()
	{
		mr = GetComponent<MeshRenderer>();
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if(!mr.isVisible)
		{
			rb.isKinematic = true;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
		else
		{
			rb.isKinematic = false;
		}
	}
}
