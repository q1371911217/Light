using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEx : MonoBehaviour
{
    //[SerializeField]
    //public xianduan up;
    //[SerializeField]
    //public xianduan down;
    //[SerializeField]
    //public xianduan left;
    //[SerializeField]
    //public xianduan right;

    public xianduan up;
    public xianduan down;
    public xianduan left;
    public xianduan right;

    void Start()
    {

        RectTransform rt = transform.GetComponent<RectTransform>();

        if (!left.auto)
        {
            left.A = transform.localPosition;
            left.B = new Vector3(transform.localPosition.x, transform.localPosition.y + rt.sizeDelta.y, 0);
            left.normal = Vector3.left;
        }

        if (!right.auto)
        {
            right.A = new Vector3(transform.localPosition.x + rt.sizeDelta.x, transform.localPosition.y, 0);
            right.B = new Vector3(transform.localPosition.x + rt.sizeDelta.x, transform.localPosition.y + rt.sizeDelta.y, 0);
            right.normal = Vector3.right;
        }

        if (!up.auto)
        {
            up.A = new Vector3(transform.localPosition.x, transform.localPosition.y + rt.sizeDelta.y, 0);
            up.B = new Vector3(transform.localPosition.x + rt.sizeDelta.x, transform.localPosition.y + rt.sizeDelta.y, 0);
            up.normal = Vector3.up;
        }

        if (!down.auto)
        {
            down.A = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            down.B = new Vector3(transform.localPosition.x + rt.sizeDelta.x, transform.localPosition.y, 0);
            down.normal = Vector3.down;
        }
            
    }
}
