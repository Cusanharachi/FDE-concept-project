using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class PlayerMovement : MonoBehaviour
{
    // All variebles for the Player Movement class
    #region Fields

    // Player object specific data
    [SerializeField]
    Rigidbody playerRigidbody;
    [SerializeField, Tooltip("Camera used for specific player")]
    Camera playerCamera;

    // Modifiers for player input
    [SerializeField, Tooltip("Modifies Player jumping power")]
    float playerJumpModifer;
    [SerializeField, Tooltip("Modifies Player flying power")]
    float playerFlyModifer;
    [SerializeField, Tooltip("Modifies Player movement forward and backward")]
    float playerMovementModifer;
    [SerializeField, Tooltip("Modifies Player rotation about the Y axis")]
    float playerRotationModifier;

    // Fields for recieved player input
    private float playerJumpInput = new float();
    private float playerFlyInput = new float();
    private float playerMovementInput = new float();
    private float playerRotationInput = new float();

    // Fields for player input after modification
    private float modifiedPlayerJumpInput = new float();
    private float modifiedPlayerFlyInput = new float();
    private float modifiedPlayerMovementInput = new float();
    private float modifiedPlayerRotationInput = new float();

    // Input action item so this script can handle input
    InputActions inputActions;

    // Variables that may be handled by other script(s)
    #region TempVariables
    private bool playerJumping = false;
    private bool playerFlying = false;
    #endregion

    #endregion

    // All methods for the Player Movement Class
    #region Methods

    // Start is called before the first frame update
    void Awake()
    {
        // Initiallizes all of the inputs to this class.
        inputActions = new InputActions();
        inputActions.Standard.Enable();
        inputActions.Standard.Jump.performed += MakePlayerJump;
        inputActions.Standard.Fly.performed += PlayerFly;
        inputActions.Standard.Fly.canceled += PlayerFall;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Performs movements to player base on inputs, modifiers, and other method logic
        PerformPlayerMovements();
    }

    // Used specifically to get player input at anytime
    private void Update()
    {
        // Gets Player Input Information
        ManagePlayerInput();      
    }

    /// <summary>
    /// Used to gather player input data.
    /// </summary>
    private void ManagePlayerInput()
    {
        // Gets player input data from the input actions
        playerJumpInput = inputActions.Standard.Jump.ReadValue<float>();
        playerFlyInput = inputActions.Standard.Fly.ReadValue<float>();
        playerMovementInput = inputActions.Standard.Movement.ReadValue<float>();
        playerRotationInput = inputActions.Standard.Rotation.ReadValue<float>();

        // Adjusts new player input
        ModifyPlayerInput();
    }

    /// <summary>
    /// Applies changes to player input to adjust strength
    /// </summary>
    private void ModifyPlayerInput()
    {
        // Applies the modifiers to each of the inputs
        modifiedPlayerJumpInput = playerJumpModifer * playerJumpInput;
        modifiedPlayerFlyInput = playerFlyModifer * playerFlyInput;
        modifiedPlayerMovementInput = playerMovementInput * playerMovementModifer;
        modifiedPlayerRotationInput = playerRotationInput * playerRotationModifier;
    }

    /// <summary>
    /// Calls necesary methods to make the player move
    /// </summary>
    public void PerformPlayerMovements()
    {
        // movement methods
        MakePlayerFly();
        MovePlayer();
        RotatePlayer();

        // Removing to a different script
        //RotateView();
    }

    /// <summary>
    /// Event Style method that makes player jump on event
    /// </summary>
    /// <param name="context"> Data that should be passed from the invoking event </param>
    public void MakePlayerJump(InputAction.CallbackContext context)
    {
        // Applies force to rigidbody based on the product of: the current read in of input, modifer and a standard up vector
        playerRigidbody.AddForce(transform.up * playerJumpModifer, ForceMode.Impulse);
    }

    /// <summary>
    /// Handles necessary logic when the player is flying
    /// </summary>
    private void MakePlayerFly()
    {
        // Returns from method if player is not actually flying
        if (!playerFlying)
        {
            return;
        }

        // Applies force to rigidbody based on the product of: a standard up vector and the modified player input.
        playerRigidbody.AddForce(transform.up * modifiedPlayerFlyInput, ForceMode.Force);
    }

    /// <summary>
    /// Handles necessary Logic when the player is moving
    /// </summary>
    private void MovePlayer()
    {
        // Applies force to rigidbody based on the prodcut of: a standard forward vector and the modified player input.
        playerRigidbody.AddForce(transform.forward * modifiedPlayerMovementInput, ForceMode.Force);
    }

    /// <summary>
    /// Handles necessary logic when the player is rotating
    /// </summary>
    private void RotatePlayer()
    {
        // Applies force to rigidbody based on a standard forward vector and the product of: a standard forward vector and the modified player input
        //playerRigidBody.MoveRotation(Quaternion.FromToRotation(transform.forward, transform.forward + new Vector3(modifiedPlayerRotationInput, 0, 0)));

        Vector3 playerRotationAdjustment = new Vector3(0, modifiedPlayerRotationInput, 0);

        Quaternion modifiedRotation = Quaternion.Euler(playerRotationAdjustment * Time.fixedDeltaTime);
        playerRigidbody.MoveRotation(playerRigidbody.rotation * modifiedRotation);
    }

    /// <summary>
    /// An event style method that toggles the state of whether or not the player is flying
    /// </summary>
    /// <param name="context"></param>
    public void PlayerFly(InputAction.CallbackContext context)
    {
        if (!playerFlying)
        {
            Debug.Log("Fly" + context);
            playerFlying = true;
        }
    }

    /// <summary>
    /// An event style method that toggles the state of whether or not they player is flying
    /// </summary>
    /// <param name="context"></param>
    public void PlayerFall(InputAction.CallbackContext context)
    {
        if (playerFlying)
        {
            Debug.Log("Fall" + context);
            playerFlying = false;
        }
    }
    #endregion
}
