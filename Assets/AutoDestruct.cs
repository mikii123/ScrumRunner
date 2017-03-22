using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour
{
	public bool OnAwake;
	public bool Deactivate;
	public float Time;

	void Awake()
	{
		if(OnAwake)
		{
			StartCoroutine("Destruct");
		}
	}

	public void AutoDestroy()
	{
		StartCoroutine("Destruct");
	}

	public IEnumerator Destruct()
	{
		yield return new WaitForSeconds(Time);
		if(Deactivate)
			gameObject.SetActive(false);
		else
			Destroy(gameObject);
	}
}
