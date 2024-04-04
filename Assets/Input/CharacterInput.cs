using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterInput : MonoBehaviour
{
	[SerializeField] private float jumpHeight = 3;
	[SerializeField] private float speed = 1.0f;
	[SerializeField] private Transform view = default;
	[SerializeField] private Animator animator = default;
	[SerializeField] AudioSource backgroundMusic;
	[SerializeField] AudioSource runningSFX;
	[SerializeField] TMP_Text PointsUI;
	private int points = 0;

	CharacterController characterController;
	GameControls gameControls;

	Vector3 velocity;
	Vector3 direction;
	Vector3 groundNormal;

	private void OnEnable() {
		gameControls ??= new GameControls();
		gameControls.Enable();
	}

	private void OnDisable() {
		gameControls.Disable();
	}

	void Start() {
		characterController = GetComponent<CharacterController>();
		view ??= Camera.main.transform;
	}

	void Update() {
        if (!backgroundMusic.isPlaying) {
			backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
        animator.SetBool("OnGround", characterController.isGrounded);

		// ground
		UpdateGroundNormal();
		if (characterController.isGrounded && velocity.y < 0) {
			velocity.y = 0;
		}

		// movement
		Vector2 movement = gameControls.Player.Movement.ReadValue<Vector2>();
		velocity.x = movement.x;
		velocity.z = movement.y;
		if (characterController.isGrounded && groundNormal.y < 1) {
			velocity.y = -groundNormal.y;
		}

		animator.SetFloat("Speed", new Vector3(velocity.x * speed, 0, velocity.z * speed).magnitude);


		// view space
		Quaternion qview = Quaternion.AngleAxis(view.eulerAngles.y, Vector3.up);
		velocity = qview * velocity;
		
		// jump
		if (gameControls.Player.Jump.phase == InputActionPhase.Performed && characterController.isGrounded) {
			velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * Physics.gravity.y);
			animator.SetTrigger("Jump");
		}
		animator.SetFloat("VelocityY", velocity.y);

		characterController.Move(velocity * speed * Time.deltaTime);

		// face direction
		direction.x = velocity.x;
		direction.z = velocity.z;
		if (direction.sqrMagnitude > 0) {
			transform.forward = direction;
		}

		// apply velocity
		velocity.y += Physics.gravity.y * Time.deltaTime;
	}

	void UpdateGroundNormal() {
		if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit)) {
			groundNormal = hit.normal;
		} else {
			groundNormal = Vector3.up;
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Crate") {
			points += 10;
			PointsUI.text = "Points: " + points.ToString();
		}
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, groundNormal * 2);
	}
}