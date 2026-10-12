using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] stadiumCameras;
    private int currentCamIndex = 0;

    void Start()
    {
        ActivateCamera(0);
    }

    void Update()
    {
        // 1-9 number keys se switch karein
        for (int i = 0; i < stadiumCameras.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActivateCamera(i);
            }
        }

        // 'C' key se sequential switch karein
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCamIndex = (currentCamIndex + 1) % stadiumCameras.Length;
            ActivateCamera(currentCamIndex);
        }
    }

    public void ActivateCamera(int index)
    {
        for (int i = 0; i < stadiumCameras.Length; i++)
        {
            if (stadiumCameras[i] != null)
                stadiumCameras[i].gameObject.SetActive(i == index);
        }
        currentCamIndex = index;
    }
}
