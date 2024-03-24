using Baracuda.Monitoring;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
	[Header("Movement System")]
	[SerializeField][Range(1.0f, 20.0f)] private float speed;

	[Header("Jump System")]
	//[Monitor]
	[SerializeField][Range(1.0f, 50.0f)] private float jumpForce;
	[SerializeField][Range(0.1f, 5.0f)] private float jumpTime = 1.5f;
	[SerializeField][Range(1f, 10.0f)] private float jumpMultiplier = 1.0f;
	[SerializeField][Range(1f, 10.0f)] private float fallMultiplier = 1.0f;

	//[Monitor]
	public bool isGrounded { get; private set; }

	//[Monitor]
	public bool isJumping { get; private set; }

	float jumpCounter = 0;

	[Header("Ground Check System")]
	[SerializeField] private Transform groudCheck;

	[SerializeField][Range(0.1f, 2.0f)] private float checkRadius;
	[SerializeField] private LayerMask platformLayer;

	private Rigidbody2D rb;
	private float gravity;
	void Start()
	{
		gravity = -Physics2D.gravity.y;
		rb = GetComponent<Rigidbody2D>();
		isGrounded = Physics2D.OverlapCircle(groudCheck.position, checkRadius, platformLayer);
		isJumping = false;
	}

	void Update()
	{
		Vector2 velocity = rb.velocity;
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = jumpForce;
			isJumping = true;
			jumpCounter = 0.0f;
		}

		if (velocity.y > 0 && isJumping)
		{
			jumpCounter += Time.deltaTime;
			if (jumpCounter > jumpTime) isJumping = false;

			float t = jumpCounter / jumpTime;
			float m = (t > 0.5f) ? jumpMultiplier * (1 - t) : jumpMultiplier;

			velocity.y += gravity * m * Time.deltaTime;
		}

		if (Input.GetButtonUp("Jump"))
		{
			isJumping = false;
		}

		if (!isJumping && !isGrounded)
		{
			velocity.y -= gravity * fallMultiplier * Time.deltaTime;
		}

		float horzInput = Input.GetAxisRaw("Horizontal");
		velocity.x = horzInput * speed;

		rb.velocity = velocity;

		Vector3 spriteScale = transform.localScale;
		if (rb.velocity.x > 0) spriteScale.x = Mathf.Abs(spriteScale.x);
		if (rb.velocity.x < 0) spriteScale.x = -1 * Mathf.Abs(spriteScale.x);
		transform.localScale = spriteScale;

		isGrounded = Physics2D.OverlapCircle(groudCheck.position, checkRadius, platformLayer);
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
		if (isGrounded) Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);

		Gizmos.DrawSphere(groudCheck.position, checkRadius);
	}

	private void Awake()
	{
		this.StartMonitoring();
	}

	private void OnDestroy()
	{
		this.StopMonitoring();
	}
}
