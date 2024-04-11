using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundShaderMaterial : MonoBehaviour, ObserverListener<GameState>
{
	// ===== OBSERVER =====
	private ObserverBroadcaster<GameState> broadcaster;
	public bool isRegistered => broadcaster != null;

	// ====================

	[SerializeField] private GameManager gameManager;
	public int selectedColorIndex => gameManager.selectedColorIndex;

	[SerializeField] private List<Material> overlays;

	private MeshRenderer _renderer;

	private Material selectedOverlay => overlays[selectedColorIndex];
	void Start()
	{
		_renderer = GetComponent<MeshRenderer>();
		gameManager.registerObserver(this);
	}

	void OnDestroy()
	{
		gameManager.deregisterObserver(this);
	}

	// ===== OBSERVER =====

	public void registerBroadcaster(ObserverBroadcaster<GameState> b) => broadcaster = b;
	public void deregisterBroadcaster() => broadcaster = null;
	public void onObserverStateChange(GameState state, GameState prevState)
	{
		if (_renderer == null) return;
		if (state == null) return;
		if (prevState == null || state.selectedColorIndex != prevState.selectedColorIndex)
		{
			_renderer.material = selectedOverlay;
		}
	}
}
