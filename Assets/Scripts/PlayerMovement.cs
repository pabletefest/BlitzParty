using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Rabbit")
        {
			touchingRabbit = false;
        }
		objectCollided = null;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Rabbit")
		{
			touchingRabbit = true;
		}
		objectCollided = collision.gameObject;
	}

	public void CatchButtonHandler()
    {
		animator.SetTrigger("Catching");
		ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("NetSwing");
		if (touchingRabbit)
        {
			//Destroy(objectCollided);
			objectCollided.SetActive(false);
			scoreController.P1ScorePoint();
		}
    }
}
