using Mirror;
using Services;
using UnityEngine;

namespace Online.BinkyPursuit
{
	public class PlayerMovementOnline : NetworkBehaviour
	{
		private Camera mainCamera;
		
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

		
		private readonly SyncList<GameObject> enemiesCollided = new SyncList<GameObject>();
		
		void Start()
		{
			mainCamera = Camera.main;
			enemiesCollided.Callback += OnEnemyKilled;
		}

		private void OnEnemyKilled(SyncList<GameObject>.Operation op, int itemindex, GameObject olditem, GameObject newitem)
		{
			Destroy(newitem);
		}

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
			PerformAction();
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
		
		private void PerformAction()
		{
			bool playerTouched = CheckPlayerTouch();

			if (playerTouched)
			{
				CatchButtonHandler();
			}
		}
		
		private bool CheckPlayerTouch()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				Ray ray = mainCamera.ScreenPointToRay(touch.position);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                
				if (hit.collider.CompareTag("CatchButton"))
				{
					return true;
					//Debug.Log("Player clicked the screen");
				}
				
				ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HammerSwing");
			}

			return false;
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
				Debug.Log("Enemy got out");
				touchingRabbit = false;
				//objectCollided = null;
			}
		}

		public void CatchButtonHandler()
		{
			if (!isLocalPlayer) return;
			
			ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("NetSwing");

			RabbitHideOnline rabbitHide = default;
			bool isEnemyAlive;

			if (objectCollided)
			{
				rabbitHide = objectCollided.GetComponent<RabbitHideOnline>();
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
				Debug.Log($"Player {PlayerNumber} is catching some weird mammal");
				//scoreController.PlayerScorePoints(1, PlayerNumber);
				//objectCollided.GetComponent<RabbitHideOnline>().DestroyEnemy();
				CmdScorePoint(1, PlayerNumber);
			}
		}

		[Command]
		private void CmdScorePoint(int amount, int playerIdentity)
		{
			Debug.Log($"Is touching rabbit? {touchingRabbit}");
			/*if (objectCollided)
				NetworkServer.UnSpawn(objectCollided);
			
			if (objectCollided)
				Destroy(objectCollided);*/
			enemiesCollided.Add(objectCollided);
			scoreController.PlayerScorePoints(amount, playerIdentity);
			
			//RpcDestroyEnemyOnClients();
			//Destroy(objectCollided);
			/*RpcDestroyEnemyOnClients();
			
			if (objectCollided)
				Destroy(objectCollided);*/
		}

		[ClientRpc]
		private void RpcDestroyEnemyOnClients()
		{
			Debug.Log($"Is enemy null? {objectCollided}");
			
			//DestroyEnemiesKilled();
			/*if (objectCollided)
				Destroy(objectCollided);*/
		}

		private void DestroyEnemiesKilled()
		{
			for (int i = 0; i < enemiesCollided.Count; i++)
			{
				GameObject enemyKilled = enemiesCollided[i];

				enemiesCollided.Remove(enemyKilled);
				Destroy(enemyKilled);
			}
		}
	}
}
