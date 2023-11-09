using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG;
using DG.Tweening;

public class KeyTigger : MonoBehaviour
{
    public int index;
    private bool isOver = false;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Fire") return;
        if (isOver) return;
        isOver = true;
        this.transform.parent.Find(string.Format("Light_{0}", index)).GetComponent<TriggerHandle>().isLock = false;
        this.transform.Find("lock").transform.DOLocalMoveY(-1000, 0.5f).OnComplete(() =>
        {
            if(gameObject != null)
                GameObject.Destroy(gameObject);
        });
    }
}
