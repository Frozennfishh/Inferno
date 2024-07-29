using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debugging : MonoBehaviour
{
    public TextMeshProUGUI debugLog;  // Reference to the TextMeshProUGUI component

    public void print(string msg)
    {
        debugLog.text = msg;
    }
}
