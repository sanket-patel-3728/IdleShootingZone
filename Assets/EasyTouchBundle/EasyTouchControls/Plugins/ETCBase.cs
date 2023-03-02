using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class ETCBase : MonoBehaviour
{
	public enum ControlType { Joystick, TouchPad, DPad, Button };
	public enum RectAnchor { UserDefined, BottomLeft, BottomCenter, BottonRight, CenterLeft, Center, CenterRight, TopLeft, TopCenter, TopRight };
	public enum DPadAxis { Two_Axis, Four_Axis };
	public enum CameraMode { Follow, SmoothFollow };
	public enum CameraTargetMode { UserDefined, LinkOnTag, FromDirectActionAxisX, FromDirectActionAxisY };

	protected RectTransform cachedRectTransform;
	protected Canvas cachedRootCanvas;

	public bool isUnregisterAtDisable = false;
	private bool visibleAtStart = true;
	private bool activatedAtStart = true;

	[SerializeField] protected RectAnchor _anchor;
	public RectAnchor anchor
	{
		get
		{
			return _anchor;
		}
		set
		{
			if (value != _anchor)
			{
				_anchor = value;
				SetAnchorPosition ();
			}
		}
	}

	[SerializeField] protected Vector2 _anchorOffet;
	public Vector2 anchorOffet
	{
		get
		{
			return _anchorOffet;
		}
		set
		{
			if (value != _anchorOffet)
			{
				_anchorOffet = value;
				SetAnchorPosition ();
			}
		}
	}

	[SerializeField] protected bool _visible;
	public bool visible
	{
		get
		{
			return _visible;
		}
		set
		{
			if (value != _visible)
			{
				_visible = value;
				SetVisible ();
			}
		}
	}

	[SerializeField] protected bool _activated;
	public bool activated
	{
		get
		{
			return _activated;
		}
		set
		{
			if (value != _activated)
			{
				_activated = value;
				SetActivated ();
			}
		}
	}

	public bool enableCamera = false;
	public CameraMode cameraMode;
	public string camTargetTag = "Player";

	public bool autoLinkTagCam = true;
	public string autoCamTag = "MainCamera";
	public Transform cameraTransform;

	public CameraTargetMode cameraTargetMode;
	public bool enableWallDetection = false;
	public LayerMask wallLayer = 0;
	public Transform cameraLookAt;
	protected CharacterController cameraLookAtCC;

	public Vector3 followOffset = new Vector3 (0, 6, -6);
	public float followDistance = 10;
	public float followHeight = 5;
	public float followRotationDamping = 5;
	public float followHeightDamping = 5;


	public int pointId = -1;

	public bool enableKeySimulation;
	public bool allowSimulationStandalone;
	public bool visibleOnStandalone = true;

	public DPadAxis dPadAxisCount;
	public bool useFixedUpdate;

	List<RaycastResult> uiRaycastResultCache = new List<RaycastResult> ();
	PointerEventData uiPointerEventData;
	EventSystem uiEventSystem;

	public bool isOnDrag;
	public bool isSwipeIn;
	public bool isSwipeOut;

	public bool showPSInspector;
	public bool showSpriteInspector;
	public bool showEventInspector;
	public bool showBehaviourInspector;
	public bool showAxesInspector;
	public bool showTouchEventInspector;
	public bool showDownEventInspector;
	public bool showPressEventInspector;
	public bool showCameraInspector;

	protected virtual void Awake ()
	{
		cachedRectTransform = transform as RectTransform;
		cachedRootCanvas = transform.parent.GetComponent<Canvas> ();

#if !UNITY_EDITOR
		if (!allowSimulationStandalone) enableKeySimulation = false;
#endif

		visibleAtStart = _visible;
		activatedAtStart = _activated;

		if (!isUnregisterAtDisable) ETCInput.Instance.RegisterControl (this);
	}

	public virtual void Start ()
	{
		if (enableCamera)
		{
			if (autoLinkTagCam)
			{
				cameraTransform = null;
				GameObject tmpobj = GameObject.FindGameObjectWithTag (autoCamTag);
				if (tmpobj) cameraTransform = tmpobj.transform;
			}
		}
	}

	public virtual void OnEnable ()
	{
		if (isUnregisterAtDisable) ETCInput.Instance.RegisterControl (this);

		visible = visibleAtStart;
		activated = activatedAtStart;
	}

	void OnDisable ()
	{
		if (ETCInput._instance)
		{
			if (isUnregisterAtDisable) ETCInput.Instance.UnRegisterControl (this);
		}

		visibleAtStart = _visible;
		activated = _activated;

		visible = false;
		activated = false;
	}

	void OnDestroy ()
	{
		if (ETCInput._instance) ETCInput.Instance.UnRegisterControl (this);
	}

	public virtual void Update ()
	{
		if (!useFixedUpdate) DoActionBeforeEndOfFrame ();
	}

	public virtual void FixedUpdate ()
	{
		if (useFixedUpdate) DoActionBeforeEndOfFrame ();
	}

	public virtual void LateUpdate ()
	{
		if (enableCamera)
		{
			// find camera 
			if (autoLinkTagCam && cameraTransform == null)
			{
				//cameraTransform = null;
				GameObject tmpobj = GameObject.FindGameObjectWithTag (autoCamTag);
				if (tmpobj) cameraTransform = tmpobj.transform;
			}

			switch (cameraMode)
			{
				case CameraMode.Follow:
				CameraFollow ();
				break;
				case CameraMode.SmoothFollow:
				CameraSmoothFollow ();
				break;
			}
		}

		UpdateControlState ();
	}

	protected virtual void UpdateControlState () { }

	protected virtual void SetVisible (bool forceUnvisible = true) { }

	protected virtual void SetActivated () { }

	public void SetAnchorPosition ()
	{
		switch (_anchor)
		{
			case RectAnchor.TopLeft:
			this.RectTransform ().anchorMin = new Vector2 (0, 1);
			this.RectTransform ().anchorMax = new Vector2 (0, 1);
			this.RectTransform ().anchoredPosition = new Vector2 (this.RectTransform ().sizeDelta.x / 2f + _anchorOffet.x, -this.RectTransform ().sizeDelta.y / 2f - _anchorOffet.y);
			break;
			case RectAnchor.TopCenter:
			this.RectTransform ().anchorMin = new Vector2 (0.5f, 1);
			this.RectTransform ().anchorMax = new Vector2 (0.5f, 1);
			this.RectTransform ().anchoredPosition = new Vector2 (_anchorOffet.x, -this.RectTransform ().sizeDelta.y / 2f - _anchorOffet.y);
			break;
			case RectAnchor.TopRight:
			this.RectTransform ().anchorMin = new Vector2 (1, 1);
			this.RectTransform ().anchorMax = new Vector2 (1, 1);
			this.RectTransform ().anchoredPosition = new Vector2 (-this.RectTransform ().sizeDelta.x / 2f - _anchorOffet.x, -this.RectTransform ().sizeDelta.y / 2f - _anchorOffet.y);
			break;

			case RectAnchor.CenterLeft:
			this.RectTransform ().anchorMin = new Vector2 (0, 0.5f);
			this.RectTransform ().anchorMax = new Vector2 (0, 0.5f);
			this.RectTransform ().anchoredPosition = new Vector2 (this.RectTransform ().sizeDelta.x / 2f + _anchorOffet.x, _anchorOffet.y);
			break;

			case RectAnchor.Center:
			this.RectTransform ().anchorMin = new Vector2 (0.5f, 0.5f);
			this.RectTransform ().anchorMax = new Vector2 (0.5f, 0.5f);
			this.RectTransform ().anchoredPosition = new Vector2 (_anchorOffet.x, _anchorOffet.y);
			break;

			case RectAnchor.CenterRight:
			this.RectTransform ().anchorMin = new Vector2 (1, 0.5f);
			this.RectTransform ().anchorMax = new Vector2 (1, 0.5f);
			this.RectTransform ().anchoredPosition = new Vector2 (-this.RectTransform ().sizeDelta.x / 2f - _anchorOffet.x, _anchorOffet.y);
			break;

			case RectAnchor.BottomLeft:
			this.RectTransform ().anchorMin = new Vector2 (0, 0);
			this.RectTransform ().anchorMax = new Vector2 (0, 0);
			this.RectTransform ().anchoredPosition = new Vector2 (this.RectTransform ().sizeDelta.x / 2f + _anchorOffet.x, this.RectTransform ().sizeDelta.y / 2f + _anchorOffet.y);
			break;
			case RectAnchor.BottomCenter:
			this.RectTransform ().anchorMin = new Vector2 (0.5f, 0);
			this.RectTransform ().anchorMax = new Vector2 (0.5f, 0);
			this.RectTransform ().anchoredPosition = new Vector2 (_anchorOffet.x, this.RectTransform ().sizeDelta.y / 2f + _anchorOffet.y);
			break;
			case RectAnchor.BottonRight:
			this.RectTransform ().anchorMin = new Vector2 (1, 0);
			this.RectTransform ().anchorMax = new Vector2 (1, 0);
			this.RectTransform ().anchoredPosition = new Vector2 (-this.RectTransform ().sizeDelta.x / 2f - _anchorOffet.x, this.RectTransform ().sizeDelta.y / 2f + _anchorOffet.y);
			break;
		}
	}

	protected GameObject GetFirstUIElement (Vector2 position)
	{
		uiEventSystem = EventSystem.current;
		if (uiEventSystem != null)
		{
			uiPointerEventData = new PointerEventData (uiEventSystem);
			uiPointerEventData.position = position;

			uiEventSystem.RaycastAll (uiPointerEventData, uiRaycastResultCache);
			if (uiRaycastResultCache.Count > 0) return uiRaycastResultCache[0].gameObject;
			else return null;
		}
		else return null;
	}


	protected void CameraSmoothFollow ()
	{
		if (!cameraTransform || !cameraLookAt) return;

		float wantedRotationAngle = cameraLookAt.eulerAngles.y;
		float wantedHeight = cameraLookAt.position.y + followHeight;

		float currentRotationAngle = cameraTransform.eulerAngles.y;
		float currentHeight = cameraTransform.position.y;

		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, followRotationDamping * Time.deltaTime);
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, followHeightDamping * Time.deltaTime);

		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

		Vector3 newPos = cameraLookAt.position;
		newPos -= currentRotation * Vector3.forward * followDistance;
		newPos = new Vector3 (newPos.x, currentHeight, newPos.z);

		if (enableWallDetection)
		{
			RaycastHit wallHit;

			if (Physics.Linecast (new Vector3 (cameraLookAt.position.x, cameraLookAt.position.y + 1f, cameraLookAt.position.z), newPos, out wallHit))
				newPos = new Vector3 (wallHit.point.x, currentHeight, wallHit.point.z);
		}
		cameraTransform.position = newPos;
		cameraTransform.LookAt (cameraLookAt);
	}


	protected void CameraFollow ()
	{
		if (!cameraTransform || !cameraLookAt) return;

		Vector3 localOffset = followOffset;

		cameraTransform.position = cameraLookAt.position + localOffset;
		cameraTransform.LookAt (cameraLookAt.position);
	}

	IEnumerator UpdateVirtualControl ()
	{
		DoActionBeforeEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		UpdateControlState ();
	}

	IEnumerator FixedUpdateVirtualControl ()
	{
		DoActionBeforeEndOfFrame ();
		yield return new WaitForFixedUpdate ();
		UpdateControlState ();
	}

	protected virtual void DoActionBeforeEndOfFrame () { }

}