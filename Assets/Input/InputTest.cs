using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour {
	void OnJump() {
		print("jump");
	}

	public void OnJump(InputAction.CallbackContext context) {
		if (context.performed) print("jumpi");

		print(context.started);
		print(context.performed);
		print(context.canceled);
	}

	void OnMovement(InputValue input) {
		print(input.Get<Vector2>());
	}

	public void Movement(InputAction.CallbackContext context) {
		print(context.ReadValue<Vector2>());
	}
}