using UnityEngine;
using System.Collections;

public class LockOffset : MonoBehaviour
{
	private Transform transf;

	void Start()
	{
		transf = transform;
	}

	void Update()
	{
		transf.position = new Vector3(transf.position.x, transf.position.y, 0);
	}
}
