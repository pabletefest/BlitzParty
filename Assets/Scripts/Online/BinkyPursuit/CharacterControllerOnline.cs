using Mirror;
using UnityEngine;

namespace Online.BinkyPursuit
{
	public class CharacterControllerOnline : NetworkBehaviour
	{
		[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

		private Rigidbody2D m_Rigidbody2D;

		private bool isFacingRight = true; 
		private Vector3 m_Velocity = Vector3.zero;

		private Collider2D playerCollider;

		private void Awake()
		{
			m_Rigidbody2D = GetComponent<Rigidbody2D>();	
			playerCollider = GetComponent<Collider2D>();
			
			
		}

		public override void OnStartClient()
		{
			int playerNumber = GetComponent<PlayerMovementOnline>().PlayerNumber;
			if (playerNumber == 1)
			{
				isFacingRight = true;
			}
			else if (playerNumber == 2)
			{
				isFacingRight = false;
			}
		}

		public void Move(float moveH, float moveV)
		{
			Vector2 direction = new Vector2(moveH, moveV).normalized;
			Vector3 targetVelocity = new Vector2(direction.x * 5f, direction.y * 5f);

			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			if (moveH > 0 && !isFacingRight)
			{
				CmdFlip();
			}
			else if (moveH < 0 && isFacingRight)
			{
				CmdFlip();
			}
		}

		[Command]
		private void CmdFlip()
		{
			//Flip();
			RpcFlip();
		}


		[ClientRpc]
		private void RpcFlip()
		{
			isFacingRight = !isFacingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		private void Flip()
		{
			isFacingRight = !isFacingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		private void OnCollisionEnter2D(Collision2D other)	
		{
			if (other.gameObject.tag == "Player2")
			{
				Physics2D.IgnoreCollision(other.collider, playerCollider);
			}
		}

		public void ResetController()
		{
			m_Rigidbody2D.velocity = Vector2.zero;
			isFacingRight = true;
		}
	}
}
