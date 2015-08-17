using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	public float gravity = 20;
	public float speed = 9;
	public float acceleration = 11;
	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode jumpKey2 = KeyCode.UpArrow;
	
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove = new Vector2();

	private PlayerPhysics playerPhysics;

	void Start () {
		playerPhysics = GetComponent<PlayerPhysics> ();
	}

	void Update () {
		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);

		if (playerPhysics.grounded) {
			if (Input.GetKeyDown (jumpKey) || Input.GetKeyDown (jumpKey2) ) {
				amountToMove.y = 10;
			} else {
				amountToMove.y = 0;
			}
		} else if (playerPhysics.canAirJump && (Input.GetKeyDown (jumpKey) || Input.GetKeyDown (jumpKey2) ) ) {
				amountToMove.y = 9;
				playerPhysics.canAirJump = false;

		}



		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove * Time.deltaTime);
	}

	private float IncrementTowards(float vel, float target, float a){
		if (vel == target) {//if youre going the inputted speed...
			return vel;
		} else {//else, increase or decrease the speed towards the target(inputted speed)
			float dir = Mathf.Sign (target - vel);
			vel += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target-vel)) ? vel : target;
		}
	}
}
