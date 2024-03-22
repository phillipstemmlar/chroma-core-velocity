using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
	private Animator animator;
	private Rigidbody2D rb;
	private RobotMovement robotMovement;

	private const string ap_velocity_y = "Y-Velocity";
	private const string ap_is_grounded = "isGrounded";


	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		robotMovement = GetComponent<RobotMovement>();
	}


	void Update()
	{
		animator.SetFloat(ap_velocity_y, rb.velocity.y);
		animator.SetBool(ap_is_grounded, robotMovement.isGrounded);
	}
}
