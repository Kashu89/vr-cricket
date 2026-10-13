using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] stadiumCameras;
    private int currentCamIndex = 0;

    void Start()
    {
        if (stadiumCameras == null || stadiumCameras.Length == 0)
        {
            Debug.LogError("CameraSwitcher: no cameras assigned. Assign stadiumCameras in the Inspector.");
            enabled = false;
            return;
        }
        ActivateCamera(0);
    }

    void Update()
    {
        // 1-9 number keys se switch karein
        int keyCount = Mathf.Min(stadiumCameras.Length, 9);
        for (int i = 0; i < keyCount; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActivateCamera(i);
                break;
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
        if (stadiumCameras == null || stadiumCameras.Length == 0) return;
        index = Mathf.Clamp(index, 0, stadiumCameras.Length - 1);
        for (int i = 0; i < stadiumCameras.Length; i++)
        {
            if (stadiumCameras[i] != null)
                stadiumCameras[i].gameObject.SetActive(i == index);
        }
        currentCamIndex = index;
    }
}
