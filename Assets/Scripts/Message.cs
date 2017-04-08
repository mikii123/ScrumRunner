using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Message : MonoBehaviour
{
	public GameObject TimerGO;
	private static UnityEngine.UI.Text LogText;
	private static float timer = 0;
	private static List<LogMessage> Queue = new List<LogMessage>();

	public class LogMessage
	{
		public float time;
		public string message;
		public bool activate;
	}

	void Start()
	{
		LogText = GetComponent<UnityEngine.UI.Text>();
	}

	void Update()
	{
		if(Queue.Count > 0)
		{
			timer += Time.deltaTime;
			if(Queue[0].time >= timer)
			{
				LogText.text = Queue[0].message;
			}
			else
			{
				if(Queue[0].activate)
				{
					TimerGO.SetActive(true);
				}
				Queue.RemoveAt(0);
				timer = 0;
			}
		}
		else
		{
			LogText.text = "";
			timer = 0;
		}
	}

	public static void Log(string message, float time, bool activateAfter = false)
	{
		LogMessage log = new LogMessage();
		log.message = message;
		log.time = time;
		log.activate = activateAfter;
		Queue.Add(log);
	}
}
