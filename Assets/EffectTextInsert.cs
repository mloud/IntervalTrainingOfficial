using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EffectTextInsert : MonoBehaviour 
{

	[SerializeField]
	Text txtUI;

	private string Text { get; set; }

	private int Index { get; set; }

	private void Start()
	{
		Text = txtUI.text;

		Index = 1;

		txtUI.text = "";
	
		StartCoroutine(PlayInternalCoroutine());
	}

	public IEnumerator PlayInternalCoroutine()
	{
		while (Index < Text.Length)
		{
			yield return new WaitForSeconds(1.0f);
			txtUI.text = Text.Substring(0, Index + 1);
			Index++;
		}
	}

}
