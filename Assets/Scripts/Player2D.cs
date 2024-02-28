using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : Character2D, IDamagable, IHealable, IScoreable {
	[SerializeField] IntVariable score;
	[SerializeField] FloatVariable healthVar;
	[SerializeField, Range(0, 20)] float jump = 12;
	[SerializeField] Weapon2D weapon;

	private void Update() {
		if (characterController.onGround && Input.GetButtonDown("Jump")) {
			movement.y = jump;
		}
		animator.SetBool("OnGround", characterController.onGround);

		if (Input.GetButtonDown("Fire1")) {
			//animator.SetTrigger("Attack");
			weapon.Use(animator);
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
		Weapon2D.eDirection direction = (facing == eFace.Right) ? Weapon2D.eDirection.Right : Weapon2D.eDirection.Left;
		weapon.Attack(direction);
	}

	public void ApplyDamage(float damage) {
		healthVar.value -= damage;
	}

	public void Heal(float health) {
		healthVar.value += health;
	}

	public void addScore(int score) {
		this.score.value += score;
	}
}