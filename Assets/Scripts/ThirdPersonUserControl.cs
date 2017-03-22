using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
	public static ThirdPersonUserControl This;

	public Transform AimTarget;

	private ThirdPersonCharacter m_Character; 
	private ThirdPersonAnimator m_TAnimator;
	private Transform m_Cam;                 
	private Vector3 m_CamForward;
	private Vector3 m_Move;
	private bool m_Jump;                      

	private float comboTimer = 0.2f;
	private float curComboT = 0;

	private bool kick = false;

	void Awake()
	{
		This = this;
	}

	private void Start()
	{
		if (Camera.main != null)
		{
			m_Cam = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
		}

		m_Character = GetComponent<ThirdPersonCharacter>();
		m_TAnimator = GetComponent<ThirdPersonAnimator>();
	}


	private void Update()
	{
		curComboT += Time.deltaTime;

		if (!m_Jump)
		{
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}

		if(Input.GetMouseButtonDown(1))
		{
			kick = true;
			curComboT = 0;
		}

		if(curComboT >= comboTimer)
		{
			kick = false;
		}

		m_TAnimator.UpdateAnimator(kick);
	}


	private void FixedUpdate()
	{
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		bool crouch = Input.GetKey(KeyCode.C);

		if (m_Cam != null)
		{
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
			m_Move = v*m_CamForward + h*m_Cam.right;
		}
		else
		{
			m_Move = v*Vector3.forward + h*Vector3.right;
		}
#if !MOBILE_INPUT
		if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif
		AimTarget.position = MouseGrab.MousePosition;
		m_Character.Move(m_Move, MouseGrab.MousePosition, m_Jump);

		m_Jump = false;
	}
}
