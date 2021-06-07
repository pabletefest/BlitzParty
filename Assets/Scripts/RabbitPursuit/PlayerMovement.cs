using Services;
using UnityEngine;

namespace RabbitPursuit
{
	public class PlayerMovement : MonoBehaviour
	{

		[SerializeField]
		private Animator animator;

		public CharacterController controller;
		public FloatingJoystick floatingJoystick;
		Animator m_Animator;

		public float runSpeed = 40f;

		float horizontalMove = 0f;
		float verticalMove = 0f;

		bool touchingRabbit = false;

		PlayersScore scoreController;

		GameObject objectCollided;

		private void Start()
		{
			m_Animator = gameObject.GetComponent<Animator>();
			scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScore>();
		}
		// Update is called once per frame
		void Update()
		{
			#if UNITY_EDITOR
				if(Input.GetKeyDown(KeyCode.Space))	
					CatchButtonHandler();
			#endif

			ControlJoystickInput();
		}

		void FixedUpdate()
		{
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
			
			
			RabbitHide rabbitHide = default;
			bool isEnemyAlive;

			if (objectCollided)
			{
				rabbitHide = objectCollided.GetComponent<RabbitHide>();
				isEnemyAlive = rabbitHide.IsAlive;
			}
			else
			{
				isEnemyAlive = false;
			}
			
			
			if (touchingRabbit && isEnemyAlive)
			{
				touchingRabbit = false;
				rabbitHide.IsAlive = false;
				//Destroy(objectCollided);
				ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("CaptureBinkySFX");
				objectCollided.SetActive(false);
				scoreController.P1ScorePoints(1);
			}
		}
	}
}
