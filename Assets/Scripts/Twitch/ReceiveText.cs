using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ReceiveText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeText(TwitchCommandData data)
    {
        textMesh.text = data.Author + " says: " + data.Message;
    }
}
