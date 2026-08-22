using UnityEngine;
using TMPro;

public class BowlingStyleUI : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public void Show(string text, Color color)
    {
        if (textMesh == null) return;

        textMesh.text = text;
        textMesh.color = color;
    }
}
