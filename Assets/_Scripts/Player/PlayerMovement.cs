using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;

    // Reference to the generated input action class
    private PlayerController _playerController;

    // Vector2 to store the input from the input action
    private Vector2 _movementInput;

    // Reference to the player weapon
    private PlayerWeapon _playerWeapon;

    private Player _player;

    private Rigidbody2D _rigidbody;

    // on awake
    private void Awake()
    {
        // Initialize the components
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Get the player
        _player = GetComponent<Player>();

        // get the player controller
        _playerController = new PlayerController();

        // get the player weapon
        _playerWeapon = GetComponent<PlayerWeapon>();

        // Get the rigidbody component
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // OnEnable is called when the script is enabled
    private void OnEnable()
    {
        // enable the input action
        _playerController.Gameplay.Enable();

        // subscribe to the move event
        _playerController.Gameplay.Movement.performed += OnMovePerformed;

        // cancel the move event
        _playerController.Gameplay.Movement.canceled += OnMoveCancelled;

        // subscribe to the shoot event
        _playerController.Gameplay.Shoot.performed += OnShoot;

        // cancel the shoot event
        _playerController.Gameplay.Shoot.canceled += _ => _playerWeapon.SetShooting(false);
    }


    private void OnDisable()
    {
        // disable the input action
        _playerController.Gameplay.Disable();

        // unsubscribe to the move event
        _playerController.Gameplay.Movement.performed -= OnMovePerformed;
    }

    #region Input Functions

    // movement for top-down view using new input system
    private void OnMovePerformed(InputAction.CallbackContext value)
    {
        // Return if the player is dead
        if (_player.CurrentHealth <= 0)
            return;

        _movementInput = value.ReadValue<Vector2>();
    }

    private void OnMoveCancelled(InputAction.CallbackContext obj)
    {
        _movementInput = Vector2.zero;
    }

    private void OnShoot(InputAction.CallbackContext value)
    {
        // Return if the player is dead
        if (_player.CurrentHealth <= 0)
        {
            // Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        _playerWeapon.SetShooting(true);
    }

    #endregion


    // Move the player
    private void MovePlayer()
    {
        // Reset the movement input to 0 if the player is dead
        if (_player.CurrentHealth <= 0)
            _movementInput = Vector2.zero;

        var movement = new Vector2(_movementInput.x, _movementInput.y) * (speed);
        _rigidbody.velocity = movement;
    }

    // fixed update is called every fixed frame
    private void FixedUpdate()
    {
        // move the player
        MovePlayer();
    }
}