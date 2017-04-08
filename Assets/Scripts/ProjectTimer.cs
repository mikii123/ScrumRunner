using UnityEngine;
using System.Collections;

public class ProjectTimer : MonoBehaviour
{
	public static ProjectTimer This;
	public static float ProjectTime = 3*60;

	private AudioSource AS;
	private UnityEngine.UI.Text timerText;

	void Awake()
	{
		This = this;
		timerText = GetComponent<UnityEngine.UI.Text>();
		AS = GetComponent<AudioSource>();
	}

	void Update()
	{
		ProjectTime -= Time.deltaTime;
		int minutes = (int)(ProjectTime/60f);
		timerText.text = minutes + "." + (ProjectTime - (minutes*60)).ToString("F2");
		if(ProjectTime <= (60+54))
		{
			if(!AS.isPlaying)
			{
				AS.Play();
			}
		}
	}
}
