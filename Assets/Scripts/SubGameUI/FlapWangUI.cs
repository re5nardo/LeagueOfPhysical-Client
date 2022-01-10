using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class FlapWangUI : MonoBehaviour
{
    [SerializeField] private TextureButton jumpButton = null;

    private void Awake()
    {
        jumpButton.button.onClick.AddListener(OnJumpButtonClick);
    }

    private void OnDestroy()
    {
        jumpButton.button.onClick.RemoveListener(OnJumpButtonClick);
    }

    private void OnJumpButtonClick()
    {
        var jumpInput = new JumpInput();
        jumpInput.normalizedPower = 1;
        jumpInput.direction = Vector3.up;

        SceneMessageBroker.Publish(jumpInput);
    }
}
