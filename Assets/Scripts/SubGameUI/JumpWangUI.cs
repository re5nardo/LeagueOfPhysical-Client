using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

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

        float y = Util.Math.FindDegree(position - moveController.PressedPosition) + LOP.Game.Current.GameUI.CameraController.RotationY;
        float x = Mathf.Sin(Mathf.Deg2Rad * y);
        
        SceneMessageBroker.Publish(new MoveInput(new Vector3(x, 0, 0)));    //  use only x
    }

    private void OnJumpControllerRelease(Vector2 position)
    {
        if (jumpController.NormalizedPower <= 0.1f)
        {
            return;
        }

        var jumpInput = new JumpInput();
        jumpInput.normalizedPower = Mathf.Sqrt(jumpController.NormalizedPower);
        jumpInput.direction = -jumpController.Direction;    //  reverse direction

        SceneMessageBroker.Publish(jumpInput);
    }
}
