using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private PlayerController m_PlayerController; // Reference to the generated input action class
    private Vector2 m_MovementInput; // Vector2 to store the input from the input action


    // on awake 
    private void Awake()
    {
        //get the player controller
        m_PlayerController = new PlayerController();
    }

    //OnEnable is called when the script is enabled
    private void OnEnable()
    {
        //enable the input action
        m_PlayerController.Gameplay.Movement.Enable();

        //subscribe to the move event
        m_PlayerController.Gameplay.Movement.performed += OnMove;
        //cancel the move event
        m_PlayerController.Gameplay.Movement.canceled += ctx => m_MovementInput = Vector2.zero;
    }

    private void OnDisable()
    {
        //disable the input action
        m_PlayerController.Gameplay.Movement.Disable();

        //unsubscribe to the move event
        m_PlayerController.Gameplay.Movement.performed -= OnMove;

    }


    //movement for top down view using new input system
    public void OnMove(InputAction.CallbackContext value)
    {
        m_MovementInput = value.ReadValue<Vector2>();
    }

    //Move the player
    private void MovePlayer()
    {
        Vector3 movement = new Vector3(m_MovementInput.x, m_MovementInput.y, 0) * speed * Time.deltaTime;
        transform.position += movement;
    }

    // fixed update is called every fixed frame
    private void FixedUpdate()
    {
        //move the player
        MovePlayer();
    }
}