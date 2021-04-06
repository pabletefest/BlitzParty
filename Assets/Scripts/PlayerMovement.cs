using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController controller;
	public FloatingJoystick floatingJoystick;
	Animator m_Animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	float verticalMove = 0f;

    private void Start()
    {
		m_Animator = gameObject.GetComponent<Animator>();
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
}
