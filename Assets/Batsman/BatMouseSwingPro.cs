using UnityEngine;

public class BatMouseSwingPro : MonoBehaviour
{
    public float swingSpeed = 6f;

    public float maxXAngle = 60f;   // up-down swing
    public float maxYAngle = 50f;   // left-right direction

    private float rotX;
    private float rotY;

    void Update()
    {
        // Mouse delta (movement)
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Rotate angles calculate karo
        rotX -= mouseY * swingSpeed;
        rotY += mouseX * swingSpeed;

        // Limits set karo
        rotX = Mathf.Clamp(rotX, -maxXAngle, maxXAngle);
        rotY = Mathf.Clamp(rotY, -maxYAngle, maxYAngle);

        // Apply rotation
        transform.localRotation = Quaternion.Euler(rotX, rotY, 0f);
    }
}
