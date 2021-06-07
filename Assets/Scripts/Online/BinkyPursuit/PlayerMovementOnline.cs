using System.Collections.Generic;
using Mirror;
using Services;
using UnityEngine;

namespace Online.BinkyPursuit
{
	public class PlayerMovementOnline : NetworkBehaviour
	{

		[SerializeField]
		private Animator animator;

		public CharacterControllerOnline controller;
		public FloatingJoystick floatingJoystick;

		public float runSpeed = 40f;

		float horizontalMove = 0f;
		float verticalMove = 0f;

		bool touchingRabbit = false;

		PlayersScoreOnline scoreController;

		GameObject objectCollided;

		private bool isClientReady;
		
		[SyncVar]
		public int playerNumber;

		public int PlayerNumber => playerNumber;

		public override void OnStartClient()
		{
			//m_Animator = gameObject.GetComponent<Animator>();
			scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScoreOnline>();
			floatingJoystick = GameObject.FindGameObjectWithTag("FloatingJoystick").GetComponent<FloatingJoystick>();
			isClientReady = true;
		}
		
		// Update is called once per frame
		void Update()
		{
			if (!isClientReady) return;
			
			if (!isLocalPlayer) return;
			
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
				animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
			}
			else
			{
				animator.SetFloat("Speed", Mathf.Abs(verticalMove));
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.CompareTag("Rabbit"))
			{
				touchingRabbit = true;
				objectCollided = collision.gameObject;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.CompareTag("Rabbit"))
			{
				touchingRabbit = false;
				objectCollided = null;
			}
		}

		public void CatchButtonHandler()
		{
			if (!isLocalPlayer) return;
			
			ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("NetSwing");
			if (touchingRabbit)
			{
				//Destroy(objectCollided);
				ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("CaptureBinkySFX");
				Debug.Log($"Player {PlayerNumber} is catching some weird mammal");
				//scoreController.PlayerScorePoints(1, PlayerNumber);
				CmdScorePoint(1, PlayerNumber);
			}
		}

		[Command]
		private void CmdScorePoint(int amount, int playerIdentity)
		{
			scoreController.PlayerScorePoints(amount, playerIdentity);
			//NetworkServer.UnSpawn(objectCollided);
			RpcDestroyEnemyOnClients();
			
			if (objectCollided)
				Destroy(objectCollided);
		}

		[ClientRpc]
		private void RpcDestroyEnemyOnClients()
		{
			Debug.Log($"Is enemy null? {objectCollided}");

			if (objectCollided)
				Destroy(objectCollided);
		}
	}
}
