using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

	private Rigidbody2D m_Rigidbody2D;
	
	private bool isFacingRight = true; 
	private Vector3 m_Velocity = Vector3.zero;


	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();		
	}

	public void Move(float moveH, float moveV)
	{
		Vector2 direction = new Vector2(moveH, moveV).normalized;
		Vector3 targetVelocity = new Vector2(direction.x * 5f, direction.y * 5f);

		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

		if (moveH > 0 && !isFacingRight)
		{
			Flip();
		}
		else if (moveH < 0 && isFacingRight)
		{
			Flip();
		}
	}



	private void Flip()
	{
		isFacingRight = !isFacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
