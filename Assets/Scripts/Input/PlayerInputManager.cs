using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    public static PlayerInputManager instance => Instance;

    public PlayerInput playerInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playerInput ??= new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput ??= new PlayerInput();
        playerInput.Enable();
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.Disable();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            playerInput?.Dispose();
            Instance = null;
        }
    }

    public Vector2 ReadMovement()
    {
        return playerInput?.Player.Movement.ReadValue<Vector2>() ?? Vector2.zero;
    }

    public bool AttackWasPressedThisFrame()
    {
        return playerInput?.Player.Attack.WasPressedThisFrame() ?? false;
    }

    public bool DashWasPressedThisFrame()
    {
        return playerInput?.Player.Dash.WasPressedThisFrame() ?? false;
    }

    public bool SkillWasPressedThisFrame()
    {
        return Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame;
    }

    public Vector3 GetPointerWorldPosition(Camera camera)
    {
        if (camera == null || Mouse.current == null)
        {
            return Vector3.zero;
        }

        return camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
