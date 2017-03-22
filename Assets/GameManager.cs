using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager This;

	public Transform TPSSpawn;
	public GameObject TPSPrefab;
	public Animator UIAnimator;
	public Animator CameraAnimator;

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

	void StartPlay()
	{
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
		yield return new WaitForSeconds(3);

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
