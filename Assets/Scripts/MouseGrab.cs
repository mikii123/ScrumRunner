using UnityEngine;
using System.Collections;

public class MouseGrab : MonoBehaviour
{
	public bool LimitLook;
	public LayerMask Mask;
	public static Vector3 MousePosition;

	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(ray, out hit, Mathf.Infinity, Mask.value);

		if(hit.transform != null)
		{
			MousePosition = hit.point;
		}

		if(LimitLook && ThirdPersonUserControl.This != null)
		{
			float x = Mathf.Clamp(MousePosition.x, -Mathf.Infinity, ThirdPersonUserControl.This.transform.position.x);
			MousePosition = new Vector3(x, MousePosition.y, MousePosition.z);
		}
	}
}
