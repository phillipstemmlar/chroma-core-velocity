using UnityEngine;

public class RobotMovement : MonoBehaviour
{
	[SerializeField] private float jumpSpeed;
	[SerializeField] private float runSpeed;

	private Rigidbody2D rb;
	private Animator animator;

	const string ap_velocity_y = "Y-Velocity";

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{

		Vector2 velocity = rb.velocity;

		bool isOnGround = Mathf.Abs(velocity.y) < 0.001;

		if (isOnGround)
		{
			float vel_x = 0;
			if (Input.GetKeyDown(KeyCode.Space)) velocity += new Vector2(0, jumpSpeed);
			if (Input.GetKey(KeyCode.A)) vel_x = -1 * runSpeed;
			if (Input.GetKey(KeyCode.D)) vel_x = runSpeed;
			velocity.x = vel_x;
		}



		Vector3 spriteScale = transform.localScale;
		if (velocity.x > 0) spriteScale.x = Mathf.Abs(spriteScale.x);
		if (velocity.x < 0) spriteScale.x = -1 * Mathf.Abs(spriteScale.x);

		animator.SetFloat(ap_velocity_y, velocity.y);

		rb.velocity = velocity;
		transform.localScale = spriteScale;
	}
}
