using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Fields
    // the maximum amount of player enegergy
    [SerializeField, Tooltip("Maximum Player Energy")]
    float MaxPlayerEngergy;

    private bool playerJumping = false;

    #endregion

    #region Methods
    private void Update()
    {

    }
    #endregion
}
