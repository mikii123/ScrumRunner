using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager This;

	public Transform TPSSpawn;
	public GameObject TPSPrefab;
	public Animator UIAnimator;
	public Animator CameraAnimator;

	public class Stats
	{
		public int Tries = 0;
		public int TurretsKilled = 0;
	}
	public Stats PlayerStats = new Stats();
	public bool Play;
	public GameObject TPS;

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
//			if(TPS == null)
//			{
//				StopPlay();
//			}
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
		yield return new WaitForSeconds(2);

		if(PlayerStats.Tries == 1)
		{
			Message.Log("Every Project is a race against time.", 3f, true);
			ProjectTimer.ProjectTime = 3*60;
			Message.Log("So there you go.", 2f);
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
