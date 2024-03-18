using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
	private enum ScrollDirection
	{
		Left, Right, Up, Down, LeftUp, RightUp, LeftDown, RightDown
	};

	[SerializeField] private RawImage _rawImg;
	[SerializeField] private float _scrollSpeed = 0.05f;
	[SerializeField] private ScrollDirection _scrollDirection = ScrollDirection.LeftDown;
	[SerializeField] private Vector2 _uvOffset;

	void Start()
	{
		_rawImg.uvRect = new Rect(_uvOffset.x, _uvOffset.y, _rawImg.uvRect.width, _rawImg.uvRect.height);
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 direction = determineScrollDirection();

		Vector2 velocity = direction * _scrollSpeed * Time.deltaTime;

		_rawImg.uvRect = new Rect(
			_rawImg.uvRect.x + velocity.x,
			_rawImg.uvRect.y + velocity.y,
			_rawImg.uvRect.width,
			_rawImg.uvRect.height
		);
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
