using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpWangUI : MonoBehaviour
{
    [SerializeField] private DirectionInputController moveController = null;
    [SerializeField] private DirectionInputController jumpController = null;

    private void Awake()
    {
        moveController.onHold += OnMoveControllerHold;
        jumpController.onRelease += OnJumpControllerRelease;
    }

    private void OnDestroy()
    {
        moveController.onHold -= OnMoveControllerHold;
        jumpController.onRelease -= OnJumpControllerRelease;
    }

    private void OnMoveControllerHold(Vector2 position)
    {
        if (position == moveController.PressedPosition)
            return;

        float y = Util.Math.FindDegree(position - moveController.PressedPosition) + LOP.Game.Current.GameUI.CameraController.GetRotation_Y();

        float x = Mathf.Sin(Mathf.Deg2Rad * y);
        float z = Mathf.Cos(Mathf.Deg2Rad * y);

        var moveInput = new MoveInput();
        moveInput.direction = new Vector3(x, 0, z);

        SceneMessageBroker.Publish(moveInput);
    }

    private void OnJumpControllerRelease(Vector2 position)
    {
        var direction = jumpController.PressedPosition - position;

        var jumpInput = new JumpInput();
        jumpInput.normalizedPower = 1;
        jumpInput.direction = direction.normalized;

        SceneMessageBroker.Publish(jumpInput);
    }
}
