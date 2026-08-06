using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance { get; private set; }

    public PlayerInput playerInput;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if(playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.Enable();
            //playerInput.Player.Movement.started += i => { Debug.Log("started"); };
            //playerInput.Player.Movement.performed += i => { Debug.Log("performed"); };
            //playerInput.Player.Movement.canceled += i => { Debug.Log("canceled"); };
        }
    }

    private void OnDisable()
    {
        if(playerInput != null)
        {
            playerInput.Disable();
        }
    }

    private void Update()
    {
        
    }
}
