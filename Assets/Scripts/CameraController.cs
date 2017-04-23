using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public AnimationCurve ToDesktop;
	public float Speed;
	public bool Zero = false;
	public ProceduralGeneration PG;

	private float time = 0;
	private Vector3 fromPos;
	private Vector3 toPos;

	void FixedUpdate()
	{
		if(ThirdPersonUserControl.This != null)
		{
			if(Zero)
			{
				if(GameManager.This.Desktop)
				{
					transform.position = Vector3.Lerp(fromPos, toPos, ToDesktop.Evaluate(time));
					time += Time.fixedDeltaTime * 4;
					time = Mathf.Clamp01(time);
					transform.position = new Vector3(transform.position.x, 0, -10);
				}
				else
				{
					time = 0;
					fromPos = transform.position;
					toPos = (ThirdPersonUserControl.This.transform.position) + ThirdPersonUserControl.This.transform.forward * 25;

					transform.position = Vector3.Lerp(transform.position, (ThirdPersonUserControl.This.transform.position) + ThirdPersonUserControl.This.transform.forward * 10, Time.fixedDeltaTime * Speed);
					float X = Mathf.Clamp(transform.position.x, -(6.2f * (PG.MaxBlocks - 6)), 0);
					transform.position = new Vector3(X, 0, -10);
				}
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, (ThirdPersonUserControl.This.transform.position) + ThirdPersonUserControl.This.transform.forward * 5, Time.fixedDeltaTime * Speed);
				transform.position = new Vector3(transform.position.x, transform.position.y, -10);
			}
		}
	}

	public void DisableLevel()
	{
		PG.gameObject.SetActive(false);
		UICanvas.This.gameObject.SetActive(false);
	}
}
