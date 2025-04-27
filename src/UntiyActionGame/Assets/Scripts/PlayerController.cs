using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 movementInput;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput.action.Enable();
        moveInput.action.performed += OnMove;
        moveInput.action.canceled += OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime);
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
  