
using System.Collections.Generic;
using UnityEngine;

public class PlatformColor : MonoBehaviour
{
	private List<Color> colorRange => new List<Color> { Color.blue, Color.red, Color.green, Color.yellow, Color.magenta, Color.cyan };
	public int numberOfColors => clamp(Manager.numberOfColors, 0, colorRange.Count);


	private int _colorIndex;
	public int colorIndex => clamp(_colorIndex, 0, numberOfColors);

	private SpriteRenderer _renderer;
	[SerializeField] private SpriteRenderer _overlayRenderer;
	private BoxCollider2D _collider;
	private PlatformMovement _movement;

	public PlatformManager Manager => _movement.Manager;


	void Start()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<BoxCollider2D>();
		_movement = GetComponent<PlatformMovement>();

		_overlayRenderer.size = _renderer.size;

		initColor();
	}

	void Update()
	{
		if (_colorIndex == Manager.selectedColorIndex && !_movement.isStartingPlatform)
		{
			_collider.enabled = false;
			_renderer.enabled = false;
			_overlayRenderer.enabled = false;
		}
		else
		{
			_collider.enabled = true;
			_renderer.enabled = true;
			_overlayRenderer.enabled = true;
		}
	}

	void initColor()
	{
		if (!_movement.isStartingPlatform)
		{
			_colorIndex = Random.Range(0, numberOfColors);
			_overlayRenderer.color = colorRange[colorIndex];
		}
	}

	private int clamp(int num, int min, int max)
	{
		if (num < min) return min;
		if (num > max) return max;
		return num;
	}
}
