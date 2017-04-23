using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaminaDisplay : MonoBehaviour
{
	private UnityEngine.UI.Image StaminaImg;
	private List<UnityEngine.UI.Image> Helpers = new List<UnityEngine.UI.Image>();

	void Start()
	{
		StaminaImg = GetComponent<UnityEngine.UI.Image>();
		Helpers = new List<UnityEngine.UI.Image>(StaminaImg.GetComponentsInChildren<UnityEngine.UI.Image>());
	}

	void Update()
	{
		StaminaImg.fillAmount = GameManager.This.PlayerStats.Stamina;

		if(GameManager.This.PlayerStats.Stamina == 1)
		{
			StaminaImg.color = Color.green;
			foreach(var ob in Helpers)
			{
				ob.color = Color.green;
			}
		}
		else if(GameManager.This.PlayerStats.Stamina > 1)
		{
			StaminaImg.color = Color.yellow;
			foreach(var ob in Helpers)
			{
				ob.color = Color.yellow;
			}
		}
		else
		{
			StaminaImg.color = Color.white;
			foreach(var ob in Helpers)
			{
				ob.color = Color.white;
			}
		}
	}
}
