using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace trn
{
	namespace ui
	{
		public class PresetButton : Button 
		{
			public delegate void PointerDelegate(Button extButton);

			public PointerDelegate OnPointerDownDelegate;
			public PointerDelegate OnPointerUpDelegate;

			
			public override void OnPointerDown (PointerEventData eventData)
			{
				if (interactable)
				{
					base.OnPointerDown (eventData);

					OnPointerDownDelegate(this);
				}
			}

			public override void OnPointerUp (PointerEventData eventData)
			{
				if (interactable)
				{
					base.OnPointerUp (eventData);

					OnPointerUpDelegate(this);

				}
			}


		}
	}
}
