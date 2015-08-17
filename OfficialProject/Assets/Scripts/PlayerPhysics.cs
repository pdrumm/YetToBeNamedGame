using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {


	public LayerMask collisionMask;

	private BoxCollider collider;
	private Vector3 size;
	private Vector3 center;

	private float skin = .005f;

	[HideInInspector]
	public bool grounded;

	[HideInInspector]
	public bool canAirJump;

	Ray currRay;
	RaycastHit currHit;

	void Start() {
		collider = GetComponent<BoxCollider> ();
		size = collider.size;
		center = collider.center;
	}

	public void Move(Vector2 moveAmount) {

		float deltaX = moveAmount.x;
		float deltaY = moveAmount.y;
		Vector2 pos = transform.position;

		//print ("X: " + pos.x + " Y: " + pos.y);

		for (int i = 0; i < 3; i++) {
			float direction = Mathf.Sign(deltaY);
			float x = (pos.x + center.x - size.x / 2) + size.x / 2 * i; // Left, center, then rightmost
			float y = pos.y + center.y + size.y / 2 * direction; // Bottom

			currRay = new Ray(new Vector2(x, y), new Vector2(0, direction));

			Debug.DrawRay(currRay.origin, currRay.direction);

			if (Physics.Raycast(currRay, out currHit, Mathf.Abs(deltaY), collisionMask)) {
				float distance = Vector3.Distance(currRay.origin, currHit.point);

				if (distance > skin) {
					deltaY = -distance + skin;
				}
				else {
					deltaY = 0;
				}
				grounded = true;
				break;
			}

			if (grounded) {
				canAirJump = true;
			}
			grounded = false;
		}

		for (int i = 0; i < 3; i++) {
			float direction = Mathf.Sign(deltaX);
			float y = (pos.y + center.y - size.y / 2) + size.y / 2 * i; // Left, center, then rightmost
			float x = pos.x + center.x + size.x / 2 * direction; // Bottom
			
			currRay = new Ray(new Vector2(x, y), new Vector2(direction, 0));
			
			Debug.DrawRay(currRay.origin, currRay.direction);
			
			if (Physics.Raycast(currRay, out currHit, Mathf.Abs(deltaX), collisionMask)) {
				float distance = Vector3.Distance(currRay.origin, currHit.point);
				
				if (distance > skin) {
					deltaX = -distance + skin;
				}
				else {
					deltaX = 0;
				}
				break;
			}
			
		}

		Vector2 finalTransform = new Vector2(deltaX, deltaY);

		transform.Translate (finalTransform);
	}
}
