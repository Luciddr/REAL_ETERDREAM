using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MemoryTrigger : MonoBehaviour
{
    public GameObject messageUI;   // UI ที่จะแสดง
    public float displayTime = 4f; // ระยะเวลาแสดงข้อความ (วินาที)
    private bool hasTriggered = false; // ป้องกันไม่ให้แสดงซ้ำ

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(ShowMessageTemporarily());
        }
    }

    private IEnumerator ShowMessageTemporarily()
    {
        messageUI.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        messageUI.SetActive(false);
    }
}

