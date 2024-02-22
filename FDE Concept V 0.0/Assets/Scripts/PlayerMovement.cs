using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    #region Variables
    // player specific data
    [SerializeField]
    Rigidbody playerRigidBody;

    [SerializeField, Tooltip("Modifies Player movement in X and Z")]
    float PlayerWalkingModifer;

    [SerializeField, Tooltip("Modifies Player jumping power")]
    float PlayerJumpModifer;

    [SerializeField, Tooltip("Modifies Player flying power")]
    float PlayerFlyModifer;


    private Vector3 playerInput;

    #endregion

    #region Methods
    // Start is called before the first frame update
    void Start()
    {
        // Initializes Player Input vectore
        playerInput = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerRigidBody.AddForce(playerInput, ForceMode.Force);

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
        playerInput.x = Input.GetAxis("Horizontal");
        //playerInput.y = Input.GetAxis("Jump");
        playerInput.z = Input.GetAxis("Vertical");

        // adjusts new player input
        ModifyPlayerInput();
    }

    /// <summary>
    /// Applies changes to player input to adjust strength
    /// </summary>
    private void ModifyPlayerInput()
    {
        playerInput.Normalize();
        playerInput *= PlayerWalkingModifer;
    }
    #endregion
}
