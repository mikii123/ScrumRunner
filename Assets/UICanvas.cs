using UnityEngine;
using System.Collections;

public class UICanvas : MonoBehaviour
{
	public static UICanvas This;

	void OnEnable()
	{
		This = this;
	}
}
