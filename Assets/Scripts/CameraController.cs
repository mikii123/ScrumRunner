using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public float Speed;
	public bool Zero = false;

	void FixedUpdate()
	{
		if(ThirdPersonUserControl.This != null)
		{
			if(Zero)
			{
				transform.position = Vector3.Lerp(transform.position, (ThirdPersonUserControl.This.transform.position) + ThirdPersonUserControl.This.transform.forward * 10, Time.fixedDeltaTime * Speed);
				float X = Mathf.Clamp(transform.position.x, -Mathf.Infinity, 0);
				transform.position = new Vector3(X, 0, -10);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, (ThirdPersonUserControl.This.transform.position) + ThirdPersonUserControl.This.transform.forward * 5, Time.fixedDeltaTime * Speed);
				transform.position = new Vector3(transform.position.x, transform.position.y, -10);
			}
		}
	}
}
