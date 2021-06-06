using Mirror;
using Services;
using UnityEngine;
using CharacterController = RabbitPursuit.CharacterController;

namespace Online.BinkyPursuit
{
	public class PlayerMovementOnline : NetworkBehaviour
	{

		[SerializeField]
		private Animator animator;

		public CharacterControllerOnline controller;
		public FloatingJoystick floatingJoystick;
		Animator m_Animator;

		public float runSpeed = 40f;

		float horizontalMove = 0f;
		float verticalMove = 0f;

		bool touchingRabbit = false;

		PlayersScoreOnline scoreController;

		GameObject objectCollided;

		private bool isClientReady;
		public int PlayerNumber { get; set; }

		public override void OnStartClient()
		{
			m_Animator = gameObject.GetComponent<Animator>();
			scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScoreOnline>();
			floatingJoystick = GameObject.FindGameObjectWithTag("FloatingJoystick").GetComponent<FloatingJoystick>();
			isClientReady = true;
		}
		
		// Update is called once per frame
		void Update()
		{
			if (!isClientReady) return;
			
			#if UNITY_EDITOR
				if(Input.GetKeyDown(KeyCode.Space))	
					CatchButtonHandler();
			#endif

			ControlJoystickInput();
		}

		void FixedUpdate()
		{
			if (!isClientReady) return;
			
			controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
		}

		private void ControlJoystickInput()
		{
			if (floatingJoystick.Horizontal >= .2f)
			{
				horizontalMove = runSpeed;
			}
			else if (floatingJoystick.Horizontal <= -.2f)
			{
				horizontalMove = -runSpeed;
			}
			else
			{
				horizontalMove = 0f;
			}

			if (floatingJoystick.Vertical >= .2f)
			{
				verticalMove = runSpeed;
			}
			else if (floatingJoystick.Vertical <= -.2f)
			{
				verticalMove = -runSpeed;
			}
			else
			{
				verticalMove = 0f;
			}

			if (horizontalMove != 0f)
			{
				m_Animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
			}
			else
			{
				m_Animator.SetFloat("Speed", Mathf.Abs(verticalMove));
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.CompareTag("Rabbit"))
			{
				touchingRabbit = true;
			}
			objectCollided = collision.gameObject;
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.CompareTag("Rabbit"))
			{
				touchingRabbit = false;
			}
			objectCollided = null;
		}

		public void CatchButtonHandler()
		{
			animator.SetTrigger("Catching");
			ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("NetSwing");
			if (touchingRabbit)
			{
				//Destroy(objectCollided);
				ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("CaptureBinkySFX");
				objectCollided.SetActive(false);
				scoreController.P1ScorePoints(1);
			}
		}
	}
}
