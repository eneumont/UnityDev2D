using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : Character2D, IDamagable {
	[SerializeField, Range(0, 20)] float jump = 12;

	private void Update() {
		if (characterController.onGround && Input.GetButtonDown("Jump")) {
			movement.y = jump;
		}
		animator.SetBool("OnGround", characterController.onGround);

		if (Input.GetButtonDown("Fire1")) {
			animator.SetTrigger("Attack");
		}
	}

	protected override void FixedUpdate() {
		// horizontal movement
		movement.x = Input.GetAxis("Horizontal") * speed;
		animator.SetFloat("Speed", Mathf.Abs(movement.x));
		if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Right : eFace.Left;

		base.FixedUpdate();
	}

	public void Attack() {
		var colliders = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (var item in colliders) {
			if (item.gameObject.TryGetComponent(out IDamagable damageable)) {
				damageable.ApplyDamage(1000);
			} 
        }
    }

	public void ApplyDamage(float damage) {
		
	}
}
