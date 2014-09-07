using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExtButton : Button 
{
	public delegate void PointerDownDelegate(ExtButton extButton);

	public PointerDownDelegate OnPointerDownDelegate;

	private static float _frequence = 0.1f;

	private bool _autoIncrementPassed;

	private bool _isPointerDown;

	private bool _coroutineRunning;

	public override void OnPointerDown (PointerEventData eventData)
	{
		base.OnPointerDown (eventData);

		_isPointerDown = true;

		_autoIncrementPassed = false;

		if (!_coroutineRunning)
			StartCoroutine (OnHoldCoroutine());
	}

	public override void OnPointerUp (PointerEventData eventData)
	{
		base.OnPointerUp (eventData);

		_isPointerDown = false;

		if (!_autoIncrementPassed)
			OnPointerDownDelegate(this);
	}

	private IEnumerator OnHoldCoroutine()
	{
		_coroutineRunning = true;
		yield return new WaitForSeconds(0.5f);

		while(_isPointerDown)
		{
			//for (int i = 0; i < 5; ++i)
			OnPointerDownDelegate(this);

			_autoIncrementPassed = true;

			yield return new WaitForSeconds(_frequence);
		}
		_coroutineRunning = false;
	}


}
