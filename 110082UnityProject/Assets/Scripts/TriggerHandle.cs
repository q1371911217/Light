using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandle : MonoBehaviour
{
    private bool isHighlight = false;
    public bool isLock = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Fire") return;
        if (isHighlight) return;
        if (isLock) return;
        isHighlight = true;
        this.transform.Find("spHighlight").gameObject.SetActive(true);
        SendMessageUpwards("faguang", SendMessageOptions.RequireReceiver);
    }
}
