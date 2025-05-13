
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public Fade fadeScript; // ลากมาจาก Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ตรวจ Tag ของ Player
        {
            fadeScript.StartFadeOut();
        }
    }
}
