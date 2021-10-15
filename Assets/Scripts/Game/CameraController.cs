using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UniRx;

public class CameraController : MonoBehaviour
{
    [Header("[3D Camera]")]
    [SerializeField] private Camera targetCamera;

    [Header("[2D UI]")]
    [SerializeField] private DirectionKey directionKey;
    [SerializeField] private Toggle followToggle;

    private Transform myTransform;

    public Camera Camera => targetCamera;
    public Vector3 Offset { get; set; }
    public Transform Target { get; set; }
    private bool followTarget;
    public bool FollowTarget
    {
        get => followTarget;
        set
        {
            followTarget = value;
            followToggle.SetIsOnWithoutNotify(value);
        }
    }
    public float RotationY => myTransform.rotation.eulerAngles.y;

    #region MonoBehaviour
    private void Start()
    {
        myTransform = transform;
        directionKey.onHold += OnDirectionKeyHold;
        followToggle.onValueChanged.AsObservable().Subscribe(value => FollowTarget = value).AddTo(this);
    }

    private void OnDestroy()
    {
        directionKey.onHold -= OnDirectionKeyHold;
    }

    private void LateUpdate()
    {
        if (FollowTarget && Target != null)
        {
            Vector3 destPosition = Target.position + Offset;

            myTransform.position = Vector3.Slerp(myTransform.position, destPosition, Time.smoothDeltaTime);
        }
    }
    #endregion

    #region Event Handler
    public void OnGoToTargetBtnClicked()
    {
        if (Target == null)
        {
            Debug.LogError("Target is null!");
            return;
        }

        myTransform.position = Target.position + Offset;
    }

    private void OnDirectionKeyHold(Vector2 vec2ScreenPosition)
    {
        FollowTarget = false;

        Vector2 vec2Direction = vec2ScreenPosition - directionKey.PressedPosition;
        Vector3 vec3Direction = new Vector3(vec2Direction.x, 0, vec2Direction.y);

        Vector3 offset = Quaternion.Euler(0, RotationY, 0) * vec3Direction.normalized;
        myTransform.position += offset;
    }
    #endregion
}
