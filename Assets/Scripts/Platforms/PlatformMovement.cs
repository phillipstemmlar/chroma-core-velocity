using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
	public float speed { get; set; }
	public float destroyX { get; set; }

	private BoxCollider2D collider;


	void Start()
	{
		collider = GetComponent<BoxCollider2D>();
	}

	void Update()
	{
		transform.Translate(new Vector2(speed * Time.deltaTime, 0));

		double x = transform.position.x + collider.size.x / 2;
		if (x < destroyX) Destroy(gameObject);
	}
}
