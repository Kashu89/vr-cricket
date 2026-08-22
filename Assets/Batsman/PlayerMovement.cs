using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    void Update()
    {
        float move = 0f;

        if (Keyboard.current.dKey.isPressed)
            move = -1f;
        else if (Keyboard.current.aKey.isPressed)
            move = 1f;

        // Move along local front/back (pitch breadth)
        transform.Translate(-transform.forward * move * moveSpeed * Time.deltaTime, Space.World);
    }
}
