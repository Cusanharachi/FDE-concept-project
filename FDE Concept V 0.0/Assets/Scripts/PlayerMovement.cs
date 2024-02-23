using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class PlayerMovement : MonoBehaviour
{
    // all variebles for the Player Movement class
    #region Variables

    // player specific data
    [SerializeField]
    Rigidbody playerRigidBody;

    // modifiers for player input
    [SerializeField, Tooltip("Modifies Player jumping power")]
    float playerJumpModifer;
    [SerializeField, Tooltip("Modifies Player flying power")]
    float playerFlyModifer;
    [SerializeField, Tooltip("Modifies Player movement in X and Z plane")]
    float playerWalkingModifer;
    [SerializeField, Tooltip("Modifies Player rotation about the Y axis")]
    float playerTurnModifier;
    [SerializeField, Tooltip("Modifies Player view speed rotation")]
    float playerViewModifier;

    // fields for recieved player input
    private float playerJumpInput = new float();
    private float playerFlyInput = new float();
    private Vector3 playerMovementInput = new Vector2(0, 0);
    private Vector3 playerRotationInput = new Vector2(0, 0);
    private Vector3 playerViewInput = new Vector2(0, 0);

    // input action item so this script can handle input
    InputActions inputActions;

    // variables that may be handles by other scripts
    #region TempVariables
    private bool playerJumping = false;
    private bool playerFlying = false;
    #endregion

    #endregion

    // all methods for the Player Movement Class
    #region Methods

    // Start is called before the first frame update
    void Awake()
    {
        // initiallizes all of the inputs to this class.
        inputActions = new InputActions();
        inputActions.Standard.Enable();
        inputActions.Standard.Jump.performed += MakePlayerJump;
        //inputActions.Standard.Fly.performed += PlayerFly;
        //inputActions.Standard.Fly.canceled += PlayerFall;
        //inputActions.Standard.Movement.performed += PlayerMove;   
        //inputActions.Standard.Direction.performed += PlayerTurn;
        //inputActions.Standard.ViewControl.performed += PlayerLook;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //playerRigidBody.AddForce(playerInput, ForceMode.Force);

    }

    // Used specifically to get player input at anytime
    private void Update()
    {
        // Gets Player Input Information
        AssesPlayerInput();      
    }

    /// <summary>
    /// Used to gather player input data.
    /// </summary>
    private void AssesPlayerInput()
    {
        // Puts data into player movement data from Input Manager.
        //playerInput.x = Input.GetAxis("Horizontal");
        //playerInput.y = Input.GetAxis("Jump");
        //playerInput.z = Input.GetAxis("Vertical");

        playerJumpInput = inputActions.Standard.Jump.ReadValue<float>();
        playerFlyInput = inputActions.Standard.Fly.ReadValue<float>();
        playerMovementInput = inputActions.Standard.Movement.ReadValue<Vector2>();
        //playerRotationInput = inputActions.Standard.Direction.ReadValue<Vector2>();
        playerViewInput = inputActions.Standard.ViewControl.ReadValue<Vector2>();

        // adjusts new player input
        ModifyPlayerInput();
    }

    /// <summary>
    /// Applies changes to player input to adjust strength
    /// </summary>
    private void ModifyPlayerInput()
    {
        //playerInput.Normalize();
        //playerInput *= PlayerWalkingModifer;

        // applies the modifiers to each of the inputs
        //playerJumpInput *= playerJumpModifer;
        playerFlyInput *= playerFlyModifer;
        playerMovementInput *= playerWalkingModifer;
        playerRotationInput *= playerTurnModifier;
        playerViewInput *= playerViewModifier;
    }

    /// <summary>
    /// Event Style method that makes player jump on event
    /// </summary>
    /// <param name="context"> Data that should be passed from the invoking event </param>
    public void MakePlayerJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump" + context);
        //Debug.Log("Jump" + context.ReadValue<float>());

        // Applies force to rigidbody based on the product of: the current read in of input, modifer and a standard up vector
        playerRigidBody.AddForce(transform.up * playerJumpModifer, ForceMode.Impulse);
    }

    /// <summary>
    /// calls necesary methods to make the player move
    /// </summary>
    public void PerformPlayerMovements()
    {
        // movement methods
        MakePlayerFly();
        MovePlayer();
        RotatePlayer();
    }

    /// <summary>
    /// Handles necessary logic when the player is flying
    /// </summary>
    private void MakePlayerFly()
    {
        // Applies force to rigidbody based on the product of: a standard up vector and the (hopefuly) modified player input.
        playerRigidBody.AddForce(transform.up * playerFlyInput, ForceMode.Force);
    }

    private void MovePlayer()
    {

    }

    private void RotatePlayer()
    {

    }

    #region OldEventBasedInputHandling
    //public void PlayerJump(InputAction.CallbackContext  context)
    //{
    //    //Debug.Log("Jump" + context);
    //    //Debug.Log("Jump" + context.ReadValue<float>());

    //    //playerRigidBody.AddForce(transform.up * context.ReadValue<float>() * playerJumpModifer, ForceMode.Impulse);
    //}

    public void PlayerFly(InputAction.CallbackContext context)
    {
        //if (!playerFlying)
        //{
        //    Debug.Log("Fly" + context);
        //    //playerRigidBody.AddForce(transform.up * context.ReadValue<float>() * playerFlyModifer, ForceMode.Force);
        //    playerFlying = true;
        //    StartCoroutine(PlayerFlying(context.ReadValue<float>()));
        //}
    }

    public void PlayerFall(InputAction.CallbackContext context)
    {
        //if (playerFlying)
        //{
        //    Debug.Log("Fall" + context);
        //    playerFlying = false;
        //}
    }

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
}
