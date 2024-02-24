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
    
    // Moving to a different script
    //[SerializeField, Tooltip("Modifies Player view speed rotation")]
    //float playerViewModifier;

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

    // Moving out to a different script
    //private Vector3 playerViewInput = new Vector2(0, 0);

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
        //inputActions.Standard.Fly.performed += PlayerFly;
        inputActions.Standard.Fly.canceled += PlayerFall;
        //inputActions.Standard.Movement.performed += PlayerMove;
        //inputActions.Standard.ViewControl.performed += PlayerLook;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        
        // Moving out to a different script
        //playerViewInput = inputActions.Standard.View.ReadValue<Vector2>();

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

        // Changed how movement input works ... again whoops that is learning for you
        //playerMovementInput.Scale(new Vector3(playerMovementModifer, playerTurnModifier, 0));

        // Moving out to a different script
        //playerViewInput *= playerViewModifier;
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
    /// An event style method that causes the player to jump
    /// </summary>
    /// <param name="context"></param>
    public void PlayerJump(InputAction.CallbackContext context)
    {
        // Gathers jump input data
        playerJumpInput = context.ReadValue<float>();

        // Modifies player jump input
        modifiedPlayerJumpInput = playerJumpInput * playerJumpModifer;

        // Applies modified input to a force
        playerRigidbody.AddForce(transform.up * modifiedPlayerJumpInput, ForceMode.Impulse);
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


    // code to be removed
    #region Code that may be removed
    // Moving into a seperate script \/ \/ \/ \/ \/ 

    /// <summary>
    /// may be moved out to a seperate camera script
    /// </summary>
    //private void RotateView()
    //{
    //    //playerRigidBody.MoveRotation(Quaternion.FromToRotation(transform.forward, transform.forward + new Vector3(0, playerMovementInput.y, 0)));
    //    //playerCamera.transform.rotation.SetFromToRotation(playerCamera.transform.forward, playerCamera.transform.forward + new Vector3(playerViewInput.x, playerViewInput.y, 0));
    //    //Camera.main.transform.rotation.SetFromToRotation(playerCamera.transform.forward, playerCamera.transform.forward + new Vector3(playerViewInput.x, playerViewInput.y, 0));
    //    //playerCamera.transform.Rotate(new Vector3(playerViewInput.y, 0, 0));
    //    //playerCamera.transform.Rotate(new Vector3(0, playerViewInput.x, 0));

    //    // creates a vector 3 for the camera rotation
    //    // ** when it its own script move fields out to save on performance
    //    Vector3 newCameraRotation = (new Vector3(playerViewInput.x, 0, 0));
    //    //newCameraRotation.Normalize();

    //    playerCamera.transform.rotation = Quaternion.LookRotation(newCameraRotation);
    //}

    #region OldEventBasedInputHandling
    //public void PlayerJump(InputAction.CallbackContext  context)
    //{
    //    //Debug.Log("Jump" + context);
    //    //Debug.Log("Jump" + context.ReadValue<float>());

    //    //playerRigidBody.AddForce(transform.up * context.ReadValue<float>() * playerJumpModifer, ForceMode.Impulse);
    //}

    //public void PlayerFly(InputAction.CallbackContext context)
    //{
    //    //if (!playerFlying)
    //    //{
    //    //    Debug.Log("Fly" + context);
    //    //    //playerRigidBody.AddForce(transform.up * context.ReadValue<float>() * playerFlyModifer, ForceMode.Force);
    //    //    playerFlying = true;
    //    //    StartCoroutine(PlayerFlying(context.ReadValue<float>()));
    //    //}
    //}

    //public void PlayerFall(InputAction.CallbackContext context)
    //{
    //    //if (playerFlying)
    //    //{
    //    //    Debug.Log("Fall" + context);
    //    //    playerFlying = false;
    //    //}
    //}

    //IEnumerator PlayerFlying(float inputValue)
    //{
    //    while (playerFlying)
    //    {
    //        Debug.Log("- - - - - - - - - - - - - - - - - - - - ");
    //        playerRigidBody.AddForce(transform.up * inputValue * playerFlyModifer, ForceMode.Force);
    //        yield return new WaitForFixedUpdate();
    //    }
    //    yield return null;
    //}

    public void PlayerMove(InputAction.CallbackContext context)
    {
        //Debug.Log("Player Movement: " + inputActions.Standard.Movement.ReadValueAsObject());
        //Debug.Log("Move" + context);
    }

    public void PlayerTurn(InputAction.CallbackContext context)
    {
        //Debug.Log("Turn" + context);
    }

    public void PlayerLook(InputAction.CallbackContext context)
    {
        // Debug.Log("Look" + context);
    }
    #endregion
    #endregion
    #endregion
}
