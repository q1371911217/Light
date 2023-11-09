using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    Vector3 pos1 = new Vector3(-271, 556, 0);
    Vector3 pos2 = new Vector3(106, 556, 0);
    Vector3 targetPos;
    int index = 1;

    private void Start()
    {
        targetPos = pos1;
        index = 1;

        move();
    }

    void move()
    {
        targetPos = index == 1 ? pos2 : pos1;
        index = index % 2 + 1;
        transform.localScale = new Vector3(index == 2 ? 1 : -1, 1, 1);

        transform.DOLocalMove(targetPos, 5).SetEase(Ease.Linear).SetDelay(0.2f).OnComplete(() =>
        {
            move();
        });
    }
}
