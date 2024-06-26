using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
	public ulong Id { get; set; }
	public PlatformManager Manager { get; set; }
	public bool isStartingPlatform { get; set; } = false;

	private BoxCollider2D _collider;
	private Rigidbody2D rb;


	void Start()
	{
		_collider = GetComponent<BoxCollider2D>();
		//rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		Vector2 velocity = new Vector2(-1 * Manager.levelSpeed, 0);
		transform.Translate(velocity * Time.deltaTime);
		//rb.velocity = velocity;

		double x = transform.position.x + _collider.size.x / 2;
		if (x < Manager.destroyX) Destroy(gameObject);
	}

	public void registerPlatform(PlatformManager manager)
	{
		this.Manager = manager;
		this.Manager.registerPlatform(gameObject);
	}

	private void Awake()
	{
		if (Manager != null) Manager.registerPlatform(gameObject);
	}

	private void OnDestroy()
	{
		if (Manager != null) Manager.unregisterPlatform(gameObject);
	}
}
