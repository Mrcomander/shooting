using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private InputActionReference moveAction;

    [SerializeField]
    private float moveSpeed = 5f;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 move = new Vector3(input.x, input.y, 0);

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}