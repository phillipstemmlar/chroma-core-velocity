using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClicked : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	[SerializeField] private Image _img;
	[SerializeField] private Sprite _default, _pressed;

	[SerializeField] private AudioClip _compressClip;
	[SerializeField] private AudioClip _unCompressClip;
	[SerializeField] private AudioSource _source;

	public void OnPointerDown(PointerEventData eventData)
	{
		_img.sprite = _pressed;
		if(_source != null && _compressClip != null) _source.PlayOneShot(_compressClip);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_img.sprite = _default;
		if (_source != null && _unCompressClip != null) _source.PlayOneShot(_unCompressClip);
	}

	public void onButtonClicked()
	{
		Debug.Log("Button Clicked!");
	}

}
