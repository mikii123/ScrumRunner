using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager This;

	public Transform TPSSpawn;
	public GameObject TPSPrefab;
	public Animator UIAnimator;
	public Animator CameraAnimator;

	public GameObject ConstCanvas;

	public class Stats
	{
		public int Tries = 0;
		public int TurretsKilled = 0;
		public float Stamina = 1f;
		public float StaminaTimer = 0;
	}
	public Stats PlayerStats = new Stats();
	private bool onceStamina = true;
	public bool Play;
	public GameObject TPS;
	public int StageWinBlock;
	public bool Desktop = false;

	void Awake()
	{
		This = this;
	}

	void Update()
	{
		if(!Play)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				StartPlay();
			}
		}
		else
		{
			if(TPS != null)
			{
				if(TPS.transform.position.x < -(6.2f * (ProceduralGeneration.This.MaxBlocks - 2)))
				{
					CameraAnimator.SetBool("Desktop", true);
					Desktop = true;
				}
				else
				{
					CameraAnimator.SetBool("Desktop", false);
					Desktop = false;
				}
			}
		}

		if(PlayerStats.StaminaTimer > 0)
		{
			PlayerStats.StaminaTimer -= Time.deltaTime;
			PlayerStats.Stamina = 1.2f;
		}
		else
		{
			PlayerStats.Stamina += Time.deltaTime/2;
			PlayerStats.Stamina = Mathf.Clamp(PlayerStats.Stamina, -0.2f, 1);
			if(PlayerStats.Stamina <= 0)
			{
				if(onceStamina)
				{
					PlayerStats.Stamina = -1;
					onceStamina = false;
				}
			}
			else
			{
				onceStamina = true;
			}
		}
	}

	public void StartPlay()
	{
		PlayerStats.Tries++;
		UIAnimator.SetBool("Play", true);
		CameraAnimator.SetBool("Play", true);

		StartCoroutine(StartP());
		//TPS = Instantiate(TPSPrefab, TPSSpawn.position, TPSSpawn.rotation) as GameObject;

		Play = true;
	}

	void StopPlay()
	{
		UIAnimator.SetBool("Play", false);
		CameraAnimator.SetBool("Play", false);

		Play = false;
	}

	IEnumerator StartP()
	{
		ProceduralGeneration.This.gameObject.SetActive(true);
		yield return new WaitForSeconds(2);

		ConstCanvas.SetActive(true);

		if(PlayerStats.Tries == 1)
		{
			//Message.Log("Every Project is a race against time.", 3f, true);
			ProjectTimer.ProjectTime = 3*60;
			//Message.Log("So there you go.", 2f);
		}

		TPS = Instantiate(TPSPrefab, TPSSpawn.position, TPSSpawn.rotation) as GameObject;

		Rigidbody rigid = TPS.GetComponent<Rigidbody>();
		ThirdPersonCharacter TPCh = TPS.GetComponent<ThirdPersonCharacter>();

		float i = 0;
		while(i <= 2)
		{
			i += Time.deltaTime;
			yield return null;

			TPCh.IgnoreInput = true;
			rigid.velocity = new Vector3(-6, rigid.velocity.y, rigid.velocity.z);
		}

		TPCh.IgnoreInput = false;
	}
}
