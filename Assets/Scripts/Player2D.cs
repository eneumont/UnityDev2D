using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player2D : MonoBehaviour {
	enum eFace {
		Left,
		Right
	}

	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Animator animator;

	[SerializeField, Range(0, 10)] float speed = 5;
	[SerializeField, Range(0, 20)] float jump = 12;
	[SerializeField, Range(0, 80)] float gravity = 60;
	[SerializeField] eFace spriteFacing = eFace.Right;

	CharacterController2D characterController;

	Vector2 movement = Vector2.zero;
	eFace facing = eFace.Right;

	void Start() {
		characterController = GetComponent<CharacterController2D>();
	}

	private void Update() {
		if (characterController.onGround && Input.GetButtonDown("Jump")) {
			movement.y = jump;
		}
	}

	void FixedUpdate() {
		// horizontal movement
		movement.x = Input.GetAxis("Horizontal") * speed;
		animator.SetFloat("speed", Mathf.Abs(movement.x));
		if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Right : eFace.Left;
		UpdateFacing();

		// vertical movement (gravity)
		movement.y -= gravity * Time.fixedDeltaTime;
		movement.y = Mathf.Max(movement.y, -gravity * Time.fixedDeltaTime * 3);

		characterController.Move(movement * Time.fixedDeltaTime);
	}

	void UpdateFacing() {
		if (facing == eFace.Left) { 
			spriteRenderer.flipX = (spriteFacing == eFace.Right);
		} else {
			spriteRenderer.flipX = !(spriteFacing == eFace.Right);
		}
	}
}