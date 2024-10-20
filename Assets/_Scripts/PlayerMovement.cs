using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;

    // Reference to the generated input action class
    private PlayerController _playerController;

    // Vector2 to store the input from the input action
    private Vector2 _movementInput;

    // Reference to the player weapon
    private PlayerWeapon _playerWeapon;

    // on awake
    private void Awake()
    {
        // get the player controller
        _playerController = new PlayerController();

        // get the player weapon
        _playerWeapon = GetComponent<PlayerWeapon>();
    }

    // OnEnable is called when the script is enabled
    private void OnEnable()
    {
        // enable the input action
        _playerController.Gameplay.Enable();

        // subscribe to the move event
        _playerController.Gameplay.Movement.performed += OnMove;

        // cancel the move event
        _playerController.Gameplay.Movement.canceled += _ => _movementInput = Vector2.zero;

        // subscribe to the shoot event
        _playerController.Gameplay.Shoot.performed += _ => _playerWeapon.SetShooting(true);

        // cancel the shoot event
        _playerController.Gameplay.Shoot.canceled += _ => _playerWeapon.SetShooting(false);
    }

    private void OnDisable()
    {
        // disable the input action
        _playerController.Gameplay.Disable();

        // unsubscribe to the move event
        _playerController.Gameplay.Movement.performed -= OnMove;
    }

    // movement for top-down view using new input system
    private void OnMove(InputAction.CallbackContext value)
    {
        _movementInput = value.ReadValue<Vector2>();
    }

    private void OnShoot(InputAction.CallbackContext value)
    {
    }

    // Move the player
    private void MovePlayer()
    {
        Vector3 movement = new Vector3(_movementInput.x, _movementInput.y, 0) * (speed * Time.deltaTime);
        transform.position += movement;
    }

    // fixed update is called every fixed frame
    private void FixedUpdate()
    {
        // move the player
        MovePlayer();
    }
}