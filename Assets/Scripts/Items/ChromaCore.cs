using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaCore : MonoBehaviour
{
	[SerializeField] private List<Sprite> sprites;

	private SpriteRenderer _renderer;
	private BoxCollider2D _collider;
	private PlatformMovement _platformMovement;
	private PlatformColor _platformColor;
	public PlatformManager Manager { get; set; }

	private int _colorIndex;
	public int colorIndex => _colorIndex;


	void Start()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<BoxCollider2D>();
		_platformMovement = GetComponentInParent<PlatformMovement>();
		_platformColor = GetComponentInParent<PlatformColor>();

		initColor();
	}

	void Update()
	{
		if (_platformMovement.isStartingPlatform || _platformColor.colorIndex == Manager.selectedColorIndex || _colorIndex == Manager.selectedColorIndex)
		{
			_collider.enabled = false;
			_renderer.enabled = false;
		}
		else
		{
			_collider.enabled = true;
			_renderer.enabled = true;
		}
	}

	//Upon collision with another GameObject, this GameObject will reverse direction
	void OnTriggerEnter2D(UnityEngine.Collider2D other)
	{
		Manager.updateSelectedColorIndex(_colorIndex);
	}

	public void initColor()
	{
		if (_platformMovement.isStartingPlatform) return;

		float p = Random.Range(0.0f, 1.0f);

		if (p < Manager.coreSpawningProbability)
		{
			int numberOfColors = clamp(Manager.numberOfColors, 0, sprites.Count);
			do
			{
				_colorIndex = Random.Range(0, numberOfColors);
			} while (_colorIndex == _platformColor.colorIndex || _colorIndex == Manager.selectedColorIndex);
			_renderer.sprite = sprites[colorIndex];
		}
		else
		{
			_collider.enabled = false;
			_renderer.enabled = false;
			this.enabled = false;
		}
	}

	private int clamp(int num, int min, int max)
	{
		if (num < min) return min;
		if (num > max) return max;
		return num;
	}
}
