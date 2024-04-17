using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public FrameInput frameInput { get; private set; }

    private void Update() => frameInput = Gather();

    private FrameInput Gather()
    {
        return new FrameInput
        {
            ThrustDown = Input.GetKeyDown(KeyCode.W),
            ThrustHeld = Input.GetKey(KeyCode.W),

            BoostDown = Input.GetKeyDown(KeyCode.LeftShift),
            BoostHeld = Input.GetKey(KeyCode.LeftShift),

            AttackDown = Input.GetMouseButtonDown(0),
            AttackHeld = Input.GetMouseButton(0),

            MousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition),
        };
    }
}

public struct FrameInput
{
    public Vector3 MousePosition;

    public bool ThrustDown;
    public bool ThrustHeld;

    public bool BoostDown;
    public bool BoostHeld;

    public bool AttackDown;
    public bool AttackHeld;
}
