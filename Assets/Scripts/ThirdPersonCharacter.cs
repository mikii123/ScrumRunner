using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
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
	float m_CapsuleHeight;
	Vector3 m_CapsuleCenter;
	CapsuleCollider m_Capsule;
	bool m_Crouching;
	bool m_JumpedInAir = false;

	void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_TPAnimator = GetComponent<ThirdPersonAnimator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();
		m_CapsuleHeight = m_Capsule.height;
		m_CapsuleCenter = m_Capsule.center;

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

		ScaleCapsuleForCrouching(false);
		PreventStandingInLowHeadroom();

		UpdateAnimator(move);
	}


	void ScaleCapsuleForCrouching(bool crouch)
	{
		if (m_IsGrounded && crouch)
		{
			if (m_Crouching) return;
			m_Capsule.height = m_Capsule.height / 2f;
			m_Capsule.center = m_Capsule.center / 2f;
			m_Crouching = true;
		}
		else
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
				return;
			}
			m_Capsule.height = m_CapsuleHeight;
			m_Capsule.center = m_CapsuleCenter;
			m_Crouching = false;
		}
	}

	void PreventStandingInLowHeadroom()
	{
		if (!m_Crouching)
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
			}
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
			m_Rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * 6, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);

		if(jump && !m_JumpedInAir)
		{
			m_JumpedInAir = true;
			m_TPAnimator.PushForward(5);
		}

		if(m_Rigidbody.velocity.y < 0)
		{
			m_Animator.SetBool("JumpDown", true);
		}

		//m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
	}


	void HandleGroundedMovement(bool jump)
	{
		m_Rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * 6, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);

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

		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			m_JumpedInAir = false;
			m_Animator.SetBool("Jump", false);
			m_Animator.SetBool("JumpDown", false);
			m_Animator.applyRootMotion = true;
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
