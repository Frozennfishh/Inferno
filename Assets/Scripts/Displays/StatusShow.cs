using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusShow : MonoBehaviour
{
    public void showStatus()
    {
        gameObject.SetActive(true);
    }

    public void hideStatus()
    {
        gameObject.SetActive(false);
    }
}
