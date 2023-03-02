using UnityEngine;
#if UNITY_EDITOR && DEVELOPER_MODE
using TMPro;
using UnityEngine.UI;
#endif
using System;
#if UNITY_EDITOR
using UnityEditor.Events;
using UnityEditor;
#endif


public sealed class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    static Vector3 _desiredPosition;
    Action _onFollow;
    public Transform target;

    [SerializeField] bool useSmooth;

    [SerializeField] float smoothSpeed = 10f;

    [SerializeField] Vector3 _followOffset = Vector3.zero;
    [SerializeField] Vector3 _lookAtOffset = Vector3.zero;

    [Header("Debug")]
    [SerializeField] bool isDebug;
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!isDebug) return;
        transform.eulerAngles = _lookAtOffset;
        if (Application.isPlaying) return;
        if (target == null) return;
        transform.position = target.position + _followOffset;
    }
#endif

    protected void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        GameController.OnGameStart += CameraFollowOnGameStart;
        GameController.OnGameFinish += CameraFollowOnGameFinish;
        if (target == null) FindPlayer();
        FindUpdateMethod();
        enabled = false;
    }

    void CameraFollowOnGameStart()
    {
        GameController.OnGameStart -= CameraFollowOnGameStart;
        enabled = true;
    }

    void OnDestroy()
    {
        GameController.OnGameStart -= CameraFollowOnGameStart;
        GameController.OnGameFinish -= CameraFollowOnGameFinish;
    }

    void CameraFollowOnGameFinish(bool state)
    {
        GameController.OnGameFinish -= CameraFollowOnGameFinish;
        enabled = false;
    }

    void FindPlayer()
    {
        var localTarget = GameObject.FindGameObjectWithTag("Player");
        if (localTarget == null)
        {
            Debug.LogError("GameObject with Tag : <color=#00ff00>" + "Player" + "</color> Not Found");
            return;
        }
        enabled = false;
        this.target = localTarget.transform;
    }

    void FindUpdateMethod()
    {
        if (useSmooth) _onFollow = SmoothFollow;
        else _onFollow = Follow;
    }

    void FixedUpdate()
    {
        _onFollow?.Invoke();
    }

    Vector3 GetDesiredPosition()
    {
        _desiredPosition = target.position;
        _desiredPosition.x += _followOffset.x;
        _desiredPosition.y += _followOffset.y;
        _desiredPosition.z += _followOffset.z;
        return _desiredPosition;
    }

    void Follow()
    {
        transform.position = GetDesiredPosition();
    }

    void SmoothFollow()
    {
        transform.position = Vector3.Lerp(transform.position, GetDesiredPosition(), smoothSpeed * Time.deltaTime);
    }

    internal void MoveTo(Vector3 position, float delay = 0, float duration = .5f, Action action = null)
    {
        enabled = false;
        iTween.MoveTo(gameObject, iTween.Hash(
            IArg.POSITION, position,
            IArg.DELAY, delay,
            IArg.TIME, duration,
            IArg.ON_COMPLETE, action
            ));
    }

    internal void MoveTo(Vector3 position, Vector3 rotation, float delay = 0, float duration = .5f, Action action = null)
    {
        MoveTo(position, delay, duration, action);
        iTween.RotateTo(gameObject, iTween.Hash(
            IArg.ROTATION, rotation,
            IArg.TIME, duration
            ));
    }

    internal void MoveTo(Transform target, float delay = 0, float duration = .5f, Action action = null)
    {
        MoveTo(target.position, target.eulerAngles, delay, duration, action);
    }

    internal void ReturnToNormal(float delay = 0f, float duration = .5f, Action action = null)
    {
        MoveTo(GetDesiredPosition(), _lookAtOffset, delay, duration);
    }
}
