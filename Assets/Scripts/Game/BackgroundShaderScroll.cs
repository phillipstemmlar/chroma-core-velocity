using Baracuda.Monitoring;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundShaderScroll : MonoBehaviour
{
	private enum ScrollDirection
	{
		Left, Right, Up, Down, LeftUp, RightUp, LeftDown, RightDown
	};

	[SerializeField] private GameManager gameManager;

	[SerializeField] private float _baseScrollSpeed = 0.05f;
	public float baseScrollSpeed => _baseScrollSpeed;
	[Monitor] public float scrollSpeed { get; set; }
	[SerializeField] private ScrollDirection _scrollDirection = ScrollDirection.LeftDown;
	[SerializeField] private Vector2 _uvOffset;

	private Vector2 _uv;

	private MeshRenderer _renderer;

	private Material material => _renderer.material;

	void Start()
	{
		_renderer = GetComponent<MeshRenderer>();
		scrollSpeed = _baseScrollSpeed;
		_uv = _uvOffset;
		updateUV(_uvOffset);
	}

	void Update()
	{
		Vector2 direction = determineScrollDirection();

		scrollSpeed = calcScrollSpeed();

		Vector2 velocity = direction * scrollSpeed * Time.deltaTime;

		_uv += velocity;
		updateUV(_uv);

		Debug.Log("" + scrollSpeed);
	}

	float calcScrollSpeed() => _baseScrollSpeed / gameManager.baseLevelSpeed * gameManager.levelSpeed;

	private void updateUV(Vector2 vec)
	{
		material.SetTextureOffset("_BaseMap", vec);
	}

	private Vector2 determineScrollDirection()
	{
		switch (_scrollDirection)
		{
			case ScrollDirection.Left:
				return new Vector2(1f, 0f);
			case ScrollDirection.Right:
				return new Vector2(-1f, 0f);
			case ScrollDirection.Down:
				return new Vector2(0f, 1f);
			case ScrollDirection.Up:
				return new Vector2(0f, -1f);
			case ScrollDirection.LeftDown:
				return new Vector2(1f, 1f);
			case ScrollDirection.LeftUp:
				return new Vector2(1f, -1f);
			case ScrollDirection.RightDown:
				return new Vector2(-1f, 1f);
			case ScrollDirection.RightUp:
				return new Vector2(-1f, -1f);
			default: return Vector2.zero;
		}
	}
}
