using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
	public LayerMask mask;
	[SerializeField] float KillDepth = 6f;
	[SerializeField] float Speed = 6f;
	[SerializeField] float m_JumpPower = 12f;
	[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
	[SerializeField] float m_RunCycleLegOffset = 0.2f;
	[SerializeField] float m_MoveSpeedMultiplier = 1f;
	[SerializeField] float m_AnimSpeedMultiplier = 1f;
	[SerializeField] float m_GroundCheckDistance = 0.1f;
	[SerializeField] bool m_AlwaysLeft = false;

	public bool IgnoreInput = false;

	Rigidbody m_Rigidbody;
	ThirdPersonAnimator m_TPAnimator;
	Animator m_Animator;
	bool m_IsGrounded;
	float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_ForwardAmount;
	Vector3 m_GroundNormal;
	bool m_Crouching;
	bool m_JumpedInAir = false;

	bool canDash = false;
	bool requireDouble = true;

	void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_TPAnimator = GetComponent<ThirdPersonAnimator>();
		m_Rigidbody = GetComponent<Rigidbody>();

		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;
	}

	public void Move(Vector3 move, Vector3 mouse, bool jump)
	{
		if (move.magnitude > 1f) move.Normalize();
		move = transform.InverseTransformDirection(move);
		CheckGroundStatus();
		move = Vector3.ProjectOnPlane(move, m_GroundNormal);
		m_ForwardAmount = move.z;

		ApplyTurn(mouse);

		if (m_IsGrounded)
		{
			HandleGroundedMovement(jump);
		}
		else
		{
			HandleAirborneMovement(move, jump);
		}

		UpdateAnimator(move);

		if(transform.position.y <= KillDepth)
		{
			GetComponent<CharacterStats>().Kill(transform.up);
		}
	}

	void Update()
	{
		if(!m_IsGrounded)
		{
//			if(requireDouble)
//			{
				if(Input.GetKeyUp(KeyCode.Space))
				{
					canDash = true;
				}
//			}
//			else
//			{
//				canDash = true;
//			}

			if(canDash && Input.GetKey(KeyCode.Space))
			{
				m_TPAnimator.PushForward();
			}
			else
			{
				m_TPAnimator.DontPush();
			}
		}
		else
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				requireDouble = true;
			}
			else
			{
				requireDouble = false;
			}
			canDash = false;
			m_TPAnimator.DontPush();
		}
	}

	void UpdateAnimator(Vector3 move)
	{
		m_Animator.SetFloat("Forward", m_ForwardAmount);
	}


	void HandleAirborneMovement(Vector3 move, bool jump)
	{
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
		m_Rigidbody.AddForce(extraGravityForce);

		if(!IgnoreInput)
			m_Rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * Speed, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);

		if(m_Rigidbody.velocity.y < 0)
		{
			m_Animator.SetBool("JumpDown", true);
		}

		//m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
	}


	void HandleGroundedMovement(bool jump)
	{
		m_Rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * Speed, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);

		if (jump)
		{
			m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
			m_IsGrounded = false;
			m_Animator.applyRootMotion = false;
			//m_GroundCheckDistance = 0.1f;
			m_Animator.SetBool("Jump", true);
			m_Animator.SetBool("JumpDown", false);
		}
	}

	void ApplyTurn(Vector3 mouse)
	{
		if(m_AlwaysLeft)
		{
			transform.rotation = Quaternion.Euler(0, -90, 0);
		}
		else
		{
			mouse.y = transform.position.y;
			transform.LookAt(mouse);
		}
	}


	public void OnAnimatorMove()
	{
		if (m_IsGrounded && Time.deltaTime > 0)
		{
			Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

			v.y = m_Rigidbody.velocity.y;
			m_Rigidbody.velocity = v;
		}
	}


	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance), Color.blue);

		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance, mask.value))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			m_JumpedInAir = false;
			m_Animator.SetBool("Jump", false);
			m_Animator.SetBool("JumpDown", false);
			m_Animator.applyRootMotion = true;
			transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			m_Animator.SetBool("Jump", true);
			m_Animator.applyRootMotion = false;
		}
	}
}
