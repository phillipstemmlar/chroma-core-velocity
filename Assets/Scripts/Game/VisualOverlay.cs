using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualOverlay : MonoBehaviour
{

	[SerializeField] private GameManager gameManager;
	public int selectedColorIndex => gameManager.selectedColorIndex;

	[SerializeField] private List<Texture2D> overlays;

	private RawImage rawImage;

	private Texture2D selectedOverlay => overlays[selectedColorIndex];
	void Start()
	{
		rawImage = GetComponent<RawImage>();
	}


	void Update()
	{
		rawImage.texture = selectedOverlay;
	}
}
