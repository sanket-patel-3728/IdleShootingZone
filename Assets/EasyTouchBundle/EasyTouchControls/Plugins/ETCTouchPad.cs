using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class ETCTouchPad : ETCBase, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

	[Serializable] public class OnMoveStartHandler : UnityEvent { }
	[Serializable] public class OnMoveHandler : UnityEvent<Vector2> { }
	[Serializable] public class OnMoveSpeedHandler : UnityEvent<Vector2> { }
	[Serializable] public class OnMoveEndHandler : UnityEvent { }

	[Serializable] public class OnTouchStartHandler : UnityEvent { }
	[Serializable] public class OnTouchUPHandler : UnityEvent { }

	[Serializable] public class OnDownUpHandler : UnityEvent { }
	[Serializable] public class OnDownDownHandler : UnityEvent { }
	[Serializable] public class OnDownLeftHandler : UnityEvent { }
	[Serializable] public class OnDownRightHandler : UnityEvent { }

	[Serializable] public class OnPressUpHandler : UnityEvent { }
	[Serializable] public class OnPressDownHandler : UnityEvent { }
	[Serializable] public class OnPressLeftHandler : UnityEvent { }
	[Serializable] public class OnPressRightHandler : UnityEvent { }

	[SerializeField] public OnMoveStartHandler onMoveStart;
	[SerializeField] public OnMoveHandler onMove;
	[SerializeField] public OnMoveSpeedHandler onMoveSpeed;
	[SerializeField] public OnMoveEndHandler onMoveEnd;

	[SerializeField] public OnTouchStartHandler onTouchStart;
	[SerializeField] public OnTouchUPHandler onTouchUp;

	[SerializeField] public OnDownUpHandler OnDownUp;
	[SerializeField] public OnDownDownHandler OnDownDown;
	[SerializeField] public OnDownLeftHandler OnDownLeft;
	[SerializeField] public OnDownRightHandler OnDownRight;

	[SerializeField] public OnDownUpHandler OnPressUp;
	[SerializeField] public OnDownDownHandler OnPressDown;
	[SerializeField] public OnDownLeftHandler OnPressLeft;
	[SerializeField] public OnDownRightHandler OnPressRight;

	public ETCAxis axisX;
	public ETCAxis axisY;
	public bool isDPI;

	Image cachedImage;

	Vector2 tmpAxis;
	Vector2 OldTmpAxis;

	GameObject previousDargObject;

	bool isOut;
	bool isOnTouch;

	bool cachedVisible;

	public ETCTouchPad ()
	{
		axisX = new ETCAxis ("Horizontal");
		axisX.speed = 1;

		axisY = new ETCAxis ("Vertical");
		axisY.speed = 1;

		_visible = true;
		_activated = true;

		showPSInspector = true;
		showSpriteInspector = false;
		showBehaviourInspector = false;
		showEventInspector = false;

		tmpAxis = Vector2.zero;
		isOnDrag = false;
		isOnTouch = false;

		axisX.unityAxis = "Horizontal";
		axisY.unityAxis = "Vertical";

		enableKeySimulation = true;
#if !UNITY_EDITOR
		enableKeySimulation = false;
#endif
		isOut = false;
		axisX.axisState = ETCAxis.AxisState.None;
		useFixedUpdate = false;
		isDPI = false;
	}

	protected override void Awake ()
	{
		base.Awake ();
		cachedVisible = _visible;
		cachedImage = GetComponent<Image> ();
	}

	public override void OnEnable ()
	{
		base.OnEnable ();
		if (!cachedVisible) cachedImage.color = new Color (0, 0, 0, 0);

		if (allowSimulationStandalone && enableKeySimulation && !Application.isEditor) SetVisible (visibleOnStandalone);
	}

	public override void Start ()
	{
		base.Start ();
		tmpAxis = Vector2.zero;
		OldTmpAxis = Vector2.zero;
		axisX.InitAxis ();
		axisY.InitAxis ();
	}

	protected override void UpdateControlState () => UpdateTouchPad ();

	protected override void DoActionBeforeEndOfFrame ()
	{
		axisX.DoGravity ();
		axisY.DoGravity ();
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (isSwipeIn && axisX.axisState == ETCAxis.AxisState.None && _activated && !isOnTouch)
		{
			if (eventData.pointerDrag != null && eventData.pointerDrag != gameObject) previousDargObject = eventData.pointerDrag;
			else if (eventData.pointerPress != null && eventData.pointerPress != gameObject) previousDargObject = eventData.pointerPress;

			eventData.pointerDrag = gameObject;
			eventData.pointerPress = gameObject;
			OnPointerDown (eventData);
		}
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (pointId == eventData.pointerId) onMoveStart.Invoke ();
	}

	public void OnDrag (PointerEventData eventData)
	{
		if (activated && !isOut && pointId == eventData.pointerId)
		{
			isOnTouch = true;
			isOnDrag = true;
			if (isDPI) tmpAxis = new Vector2 (eventData.delta.x / Screen.dpi * 100, eventData.delta.y / Screen.dpi * 100);
			else tmpAxis = new Vector2 (eventData.delta.x, eventData.delta.y);

			if (!axisX.enable) tmpAxis.x = 0;

			if (!axisY.enable) tmpAxis.y = 0;
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (_activated && !isOnTouch)
		{
			axisX.axisState = ETCAxis.AxisState.Down;
			tmpAxis = eventData.delta;
			isOut = false;
			isOnTouch = true;
			pointId = eventData.pointerId;

			onTouchStart.Invoke ();
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (pointId == eventData.pointerId)
		{
			isOnDrag = false;
			isOnTouch = false;
			tmpAxis = Vector2.zero;
			OldTmpAxis = Vector2.zero;

			axisX.axisState = ETCAxis.AxisState.None;
			axisY.axisState = ETCAxis.AxisState.None;

			if (!axisX.isEnertia && !axisY.isEnertia)
			{
				axisX.ResetAxis ();
				axisY.ResetAxis ();
				onMoveEnd.Invoke ();
			}

			onTouchUp.Invoke ();

			if (previousDargObject)
			{
				ExecuteEvents.Execute<IPointerUpHandler> (previousDargObject, eventData, ExecuteEvents.pointerUpHandler);
				previousDargObject = null;
			}
			pointId = -1;
		}
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		if (pointId == eventData.pointerId)
		{
			if (!isSwipeOut)
			{
				isOut = true;
				OnPointerUp (eventData);
			}
		}
	}

	void UpdateTouchPad ()
	{
		if (enableKeySimulation && !isOnTouch && _activated && _visible)
		{
			isOnDrag = false;
			tmpAxis = Vector2.zero;

			float x = Input.GetAxis (axisX.unityAxis);
			float y = Input.GetAxis (axisY.unityAxis);

			if (x != 0)
			{
				isOnDrag = true;
				tmpAxis = new Vector2 (1 * Mathf.Sign (x), tmpAxis.y);
			}

			if (y != 0)
			{
				isOnDrag = true;
				tmpAxis = new Vector2 (tmpAxis.x, 1 * Mathf.Sign (y));
			}
		}

		OldTmpAxis.x = axisX.axisValue;
		OldTmpAxis.y = axisY.axisValue;

		axisX.UpdateAxis (tmpAxis.x, isOnDrag, ETCBase.ControlType.DPad);
		axisY.UpdateAxis (tmpAxis.y, isOnDrag, ETCBase.ControlType.DPad);

		if (axisX.axisValue != 0 || axisY.axisValue != 0)
		{
			// X axis
			if (axisX.actionOn == ETCAxis.ActionOn.Down && (axisX.axisState == ETCAxis.AxisState.DownLeft || axisX.axisState == ETCAxis.AxisState.DownRight)) axisX.DoDirectAction ();
			else if (axisX.actionOn == ETCAxis.ActionOn.Press) axisX.DoDirectAction ();

			// Y axis
			if (axisY.actionOn == ETCAxis.ActionOn.Down && (axisY.axisState == ETCAxis.AxisState.DownUp || axisY.axisState == ETCAxis.AxisState.DownDown)) axisY.DoDirectAction ();
			else if (axisY.actionOn == ETCAxis.ActionOn.Press) axisY.DoDirectAction ();

			onMove.Invoke (new Vector2 (axisX.axisValue, axisY.axisValue));
			onMoveSpeed.Invoke (new Vector2 (axisX.axisSpeedValue, axisY.axisSpeedValue));
		}
		else if (axisX.axisValue == 0 && axisY.axisValue == 0 && OldTmpAxis != Vector2.zero) onMoveEnd.Invoke ();

		float coef = 1;
		if (axisX.invertedAxis) coef = -1;
		if (OldTmpAxis.x == 0 && Mathf.Abs (axisX.axisValue) > 0)
		{
			if (axisX.axisValue * coef > 0)
			{
				axisX.axisState = ETCAxis.AxisState.DownRight;
				OnDownRight.Invoke ();
			}
			else if (axisX.axisValue * coef < 0)
			{
				axisX.axisState = ETCAxis.AxisState.DownLeft;
				OnDownLeft.Invoke ();
			}
			else axisX.axisState = ETCAxis.AxisState.None;
		}
		else if (axisX.axisState != ETCAxis.AxisState.None)
		{
			if (axisX.axisValue * coef > 0)
			{
				axisX.axisState = ETCAxis.AxisState.PressRight;
				OnPressRight.Invoke ();
			}
			else if (axisX.axisValue * coef < 0)
			{
				axisX.axisState = ETCAxis.AxisState.PressLeft;
				OnPressLeft.Invoke ();
			}
			else axisX.axisState = ETCAxis.AxisState.None;
		}

		coef = 1;
		if (axisY.invertedAxis) coef = -1;
		if (OldTmpAxis.y == 0 && Mathf.Abs (axisY.axisValue) > 0)
		{
			if (axisY.axisValue * coef > 0)
			{
				axisY.axisState = ETCAxis.AxisState.DownUp;
				OnDownUp.Invoke ();
			}
			else if (axisY.axisValue * coef < 0)
			{
				axisY.axisState = ETCAxis.AxisState.DownDown;
				OnDownDown.Invoke ();
			}
			else axisY.axisState = ETCAxis.AxisState.None;
		}
		else if (axisY.axisState != ETCAxis.AxisState.None)
		{
			if (axisY.axisValue * coef > 0)
			{
				axisY.axisState = ETCAxis.AxisState.PressUp;
				OnPressUp.Invoke ();
			}
			else if (axisY.axisValue * coef < 0)
			{
				axisY.axisState = ETCAxis.AxisState.PressDown;
				OnPressDown.Invoke ();
			}
			else axisY.axisState = ETCAxis.AxisState.None;
		}
		tmpAxis = Vector2.zero;
	}

	protected override void SetVisible (bool forceUnvisible = false)
	{
		if (Application.isPlaying)
		{
			if (!_visible) cachedImage.color = new Color (0, 0, 0, 0);
			else cachedImage.color = new Color (1, 1, 1, 1);
		}
	}

	protected override void SetActivated ()
	{
		if (!_activated)
		{
			isOnDrag = false;
			isOnTouch = false;
			tmpAxis = Vector2.zero;
			OldTmpAxis = Vector2.zero;

			axisX.axisState = ETCAxis.AxisState.None;
			axisY.axisState = ETCAxis.AxisState.None;

			if (!axisX.isEnertia && !axisY.isEnertia)
			{
				axisX.ResetAxis ();
				axisY.ResetAxis ();
			}
			pointId = -1;
		}
	}
}