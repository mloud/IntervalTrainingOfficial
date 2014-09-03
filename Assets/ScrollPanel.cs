using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace MUI
{

	public class ScrollPanel : MonoBehaviour, ITouchListener
	{
		[SerializeField]
		private GameObject TestGo;

		[SerializeField]
		private RectTransform Container;

		[SerializeField]
		private RectTransform Window;



		private List<GameObject> ItemList { get; set; }

		private bool ScrollInX { get; set;}
		private bool ScrollInY { get; set;}


		private Vector3 ScrollOffset;
		private Vector3 NScrollOffset;

		private Vector3? DstScrollOffset;

		private Vector3 ContainerPosition;
		private Vector2 ContainerSize;

		private Vector3 Speed;

		private class ScrollTouch
		{
			public Vector3 StartPosition;
			public float Time; 
		}


		private enum State
		{
			Stopped = 0,
			Moving,
			Slowing
		}

		private enum LayoutType
		{
			Horizontal = 0,
			Vertical
		}

		private State ActualState { get; set; }
		private ScrollTouch CurrentTouch { get; set; }

		Vector3 LastScrollOffset;

		void Start () 
		{
			ScrollInX = true;

			ItemList = new List<GameObject>();
			CurrentTouch = new ScrollTouch();
			ScrollOffset = new Vector3();
			NScrollOffset = new Vector3();
			LastScrollOffset = new Vector3();
			ContainerPosition = Container.position;

			TestFill();

			FindObjectOfType<TouchManager>().Register(this);
		}

		void OnDestroy()
		{
			FindObjectOfType<TouchManager>().Unregister(this);
		}

		void TestFill()
		{
			for (int i = 0; i < 20; ++i)
			{
				Add(Instantiate(TestGo) as GameObject);
			}

			Layout(LayoutType.Horizontal);
		}


		void Update ()
		{
			if (DstScrollOffset != null)
			{
				ScrollOffset = Vector3.Lerp(CurrentScrollOffset(), DstScrollOffset.Value, Time.deltaTime * 10.0f);
			
				if ( (ScrollOffset - DstScrollOffset.Value).magnitude < 0.01f)
				{
					ScrollOffset = DstScrollOffset.Value;
					DstScrollOffset = null;
				}
			}


			if (Speed.magnitude > 0.1f)
			{
				ScrollOffset += Speed * Time.deltaTime;

				Speed = Vector3.Lerp(Speed, Vector3.zero, Time.deltaTime * 10.0f);
			
				CheckLimits();
			}
		

			Speed = (LastScrollOffset - ScrollOffset) * Time.deltaTime;

			LastScrollOffset = ScrollOffset;

			Debug.Log(CurrentScrollOffset().ToString());
			UpdateState();
		}

		private void UpdateState()
		{
			Container.position = ContainerPosition + CurrentScrollOffset();
		}


		private void CheckLimits()
		{
			Vector3 currScrollOffset = CurrentScrollOffset();

			if (ScrollInX)
			{
				float maxScrollOffsetX = (ContainerSize.x - Window.rect.width) * 0.5f;

				if (currScrollOffset.x > maxScrollOffsetX)
				{
					currScrollOffset.x -= currScrollOffset.x - maxScrollOffsetX;

					DstScrollOffset = currScrollOffset;
				}
				else if (-currScrollOffset.x > maxScrollOffsetX)
				{
					currScrollOffset.x -= currScrollOffset.x + maxScrollOffsetX;
					
					DstScrollOffset = currScrollOffset;
				}
			}
			else
			{
				float maxScrollOffsetY = (ContainerSize.y - Window.rect.height) * 0.5f;
				
				if (currScrollOffset.y > maxScrollOffsetY)
				{
					currScrollOffset.y -= currScrollOffset.y - maxScrollOffsetY;
					
					DstScrollOffset = currScrollOffset;
				}
				else if (-currScrollOffset.y > maxScrollOffsetY)
				{
					currScrollOffset.y -= currScrollOffset.y + maxScrollOffsetY;
					
					DstScrollOffset = currScrollOffset;
				}
			}
		}

		private void EnterState(State state)
		{
			ActualState = state;
		}

		public void TouchBegan(MUI.Touch touch)
		{
			CurrentTouch.StartPosition = touch.Position;
			CurrentTouch.Time = Time.time;
			DstScrollOffset = null;
		}

		public void TouchEnded(MUI.Touch  touch)
		{
		
			ScrollOffset += NScrollOffset;
		
			NScrollOffset = Vector3.zero;

			CheckLimits();
		}

		private Vector3 CurrentScrollOffset()
		{
			return ScrollOffset + NScrollOffset;
		}


		public void TouchMoved(MUI.Touch touch)
		{
			NScrollOffset = touch.Position - CurrentTouch.StartPosition;

			if (ScrollInX)
				NScrollOffset.y = 0;
			else
				NScrollOffset.x = 0;
		}


		public void Add(GameObject gameObject)
		{
			// add to inner list
			ItemList.Add(gameObject);

			gameObject.transform.parent = Container.transform;
		}

		private Vector2 ComputeContainerSize()
		{
			Vector2 size = new Vector2();

			for (int i = 0; i < ItemList.Count; ++i)
			{
				RectTransform rectTr = ItemList[i].GetComponent<RectTransform>();
				
				if (rectTr != null)
				{
					size.x += rectTr.rect.width;
					size.y += rectTr.rect.height;
				}
			}

			return size;
		}


		private void Layout(LayoutType layout)
		{

			ContainerSize = ComputeContainerSize();


			if (layout == LayoutType.Horizontal)
			{
				Vector3 position = new Vector3(-ContainerSize.x * 0.5f, Container.position.z);

				for (int i = 0; i < ItemList.Count; ++i)
				{
					RectTransform rectTr = ItemList[i].GetComponent<RectTransform>();

					if (rectTr != null)
					{
						Vector3 rectTrPos = position;
						rectTrPos.x += rectTr.rect.width * 0.5f;
						rectTr.localPosition =  rectTrPos;
			
						position.x += rectTr.rect.width;
					}
				}
			}
			else if (layout == LayoutType.Vertical)
			{
				Vector3 position = new Vector3(0, ContainerSize.y * 0.5f, Container.position.z);

				for (int i = 0; i < ItemList.Count; ++i)
				{
					RectTransform rectTr = ItemList[i].GetComponent<RectTransform>();
					
					if (rectTr != null)
					{
						Vector3 rectTrPos = position;
						rectTrPos.y -= rectTr.rect.height * 0.5f;
						rectTr.localPosition =  rectTrPos;
						
						position.y -= rectTr.rect.height;
					}
				}
			}
		}


	}
}
