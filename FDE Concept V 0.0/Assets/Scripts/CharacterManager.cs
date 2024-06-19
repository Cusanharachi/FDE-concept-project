using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
    // Class to Disable Living Player for secondary Player to work.
    [SerializeField]
    PlayerMovement livingPlayerMovementScript;
    [SerializeField]

    // Input action item so this script can handle input
    InputActions inputActions;


    // Start is called before the first frame update
    void Awake()
    {
        if (livingPlayerMovementScript != null)
        {
            inputActions = livingPlayerMovementScript.inputActions;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSecondForm()
    {
        
    }
}
