
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTrans;

    public LineRenderer lineRenderer;

    Vector3 sourcePos;

    void Start()
    {
        

        rectTrans = this.transform.parent.GetComponent<RectTransform>();

        lineRenderer.startWidth = 0.1f;

        //lineRenderer.loop = true;

        lineRenderer.endWidth = 0.1f;
        lineRenderer.endColor = Color.red;
    }

    bool isDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Vector3 sourcePos = this.transform.Find("LighterGo").localPosition;
        //sourcePos.z = -100;        
        if (Game.gameStep != GameStep.Start) return;
        isDrag = true;
        lineRenderer.positionCount = 1;
        sourcePos = this.transform.Find("LighterGo").localPosition;
        lineRenderer.SetPosition(0, sourcePos);
        lineRenderer.enabled = true;
        getLocalPostion();
       

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Game.gameStep != GameStep.Start) return;
        if (!isDrag) return;
        pathPointList.Clear();
        pathPointList.Add(sourcePos);
        getLocalPostion();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Game.gameStep != GameStep.Start) return;
        if (!isDrag) return;
        isDrag = false;
        //getLocalPostion();
        lineRenderer.enabled = false;
        if (lineRenderer.positionCount > 0)
        {
            List<Vector3> lineRenderPosList = new List<Vector3>();
            for(int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderPosList.Add(lineRenderer.GetPosition(i));
            }

            SendMessageUpwards("shoot", lineRenderPosList, SendMessageOptions.RequireReceiver);

        }
        
    }

    

    List<Vector3> pathPointList = new List<Vector3>();

   

    public void getLocalPostion()
    {
        Vector2 globalMousePos;
        //Vector3 worldMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrans, Input.mousePosition, Camera.main, out globalMousePos);

        //RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, Input.mousePosition, Camera.main, out worldMousePos);
        //worldMousePos.z = -100;
        //globalMousePos.z = -100;

        float hypotenuse = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(globalMousePos.x - sourcePos.x), 2) + Mathf.Pow(Mathf.Abs(globalMousePos.y - sourcePos.y), 2));
        float mul = 1000 / hypotenuse;

        Vector3 aa = new Vector3(globalMousePos.x - sourcePos.x, globalMousePos.y - sourcePos.y, 0) * mul;
        

        closeTransList.Clear();
        if (Game.curLevel < 6 || (Game.curLevel > 10 && Game.curLevel < 16))
        {
            checkReflect2(sourcePos, aa);
        }
        else
        {
            checkReflect(sourcePos, aa);
        }
        

        for(int i = 1;i< pathPointList.Count; i++)
        {
            lineRenderer.positionCount = i+1;
            lineRenderer.SetPosition(i, pathPointList[i]);
        }        
    }
    
    RectTransform wallRect;
    float width, height;
    Vector3 wallPos, horizontalV3, verticalV3;
    List<Transform> closeTransList = new List<Transform>();
    float radian = 0;
    xianduan shu, heng;
    void checkReflect(Vector3 A, Vector3 B)
    {
        if (Game.wallTransList.Count < 1)
        {
            pathPointList.Add(B);
            return;
        }
        float angle = 0;
        bool isIntersecion = false;
        Vector3 IntersectionPos = Vector3.zero;
        float x = 0, y = 0;
        float distance = 0;
        Vector3 normal = Vector3.zero, tangent = Vector3.zero;
        for (int i = 0; i < Game.wallTransList.Count; i++)
        {
            if (closeTransList.IndexOf(Game.wallTransList[i]) != -1) continue;
            //wallPos = Game.wallTransList[i].localPosition;
            //Wall wall = Game.wallTransList[i].GetComponent<Wall>();
            WallEx wall = Game.wallTransList[i].GetComponent<WallEx>();
            //wallRect = Game.wallTransList[i].GetComponent<RectTransform>();
            //width = wallRect.sizeDelta.x;
            //height = wallRect.sizeDelta.y;
            if(A.x <= wall.left.A.x)
            {
                if(A.y <= wall.down.A.y)
                {
                    heng = wall.down;
                    shu = wall.left;
                }
                else if(A.y > wall.up.A.y)
                {
                    heng = wall.up;
                    shu = wall.left;
                }
                else
                    heng = wall.left;
            }else if(A.x > wall.right.A.x)
            {
                if (A.y <= wall.down.A.y)
                {
                    heng = wall.down;
                    shu = wall.right;
                }
                else if (A.y > wall.up.A.y)
                {
                    heng = wall.up;
                    shu = wall.right;
                }
                else
                    heng = wall.right;
            }
            else if(A.y <= wall.down.A.y)
            {
                if (A.x <= wall.left.A.x)
                {
                    heng = wall.left;
                    shu = wall.down;
                }
                else if (A.x > wall.right.A.x)
                {
                    heng = wall.right;
                    shu = wall.down;
                }
                else
                    heng = wall.down;
            }
            else if(A.y > wall.up.A.y)
            {
                if (A.x <= wall.left.A.x)
                {
                    heng = wall.left;
                    shu = wall.up;
                }
                else if (A.x > wall.right.A.x)
                {
                    heng = wall.right;
                    shu = wall.up;
                }
                else
                    heng = wall.up;
            }

            //verticalV3 = wall.xianduanList[0].B;// new Vector3(wallPos.x, wallPos.y + height, 0);
            //horizontalV3 = wall.xianduanList.Count > 1 ? wall.xianduanList[1].B :  new Vector3(wallPos.x + width, wallPos.y, 0);
            distance = Mathf.Abs(Vector3.Distance(A, B));

            if (Tools.TryGetIntersectPoint(A, B, heng.A, heng.B, out IntersectionPos))
            {
                isIntersecion = true;

                Vector3 v3 = Vector3.Reflect((B - A).normalized, heng.normal);
                x = v3.x * distance;
                y = v3.y * distance;

                //angle = Vector3.Angle((verticalV3 - wallPos).normalized, (B - A).normalized);
                ////Debug.LogError(angle);
                //radian = ((wall.what * angle + wall.angle) * Mathf.PI) / 180;
                ////x = IntersectionPos.x + distance  * Mathf.Sin(radian);
                ////y = IntersectionPos.y + distance  * Mathf.Cos(radian);
                //x = IntersectionPos.x + distance * Mathf.Cos(radian);
                //y = IntersectionPos.y + distance * Mathf.Sin(radian);
            }
            else if (Tools.TryGetIntersectPoint(A, B, shu.A, shu.B, out IntersectionPos))
            {
                isIntersecion = true;
                Vector3 v3 = Vector3.Reflect((B - A).normalized, shu.normal);
                x = v3.x * distance;
                y = v3.y * distance;
                //angle = Vector3.Angle( (horizontalV3 - wallPos).normalized, (B - A).normalized);
                //radian = (-angle * Mathf.PI) / 180;
                //x = IntersectionPos.x - distance  * Mathf.Sin(radian);
                //y = IntersectionPos.y - distance  * Mathf.Cos(radian);
            }
            if(isIntersecion)
            {
                //closeTransList.Add(Game.wallTransList[i]);
                break;
            }
        }

        if (isIntersecion)
        {
            pathPointList.Add(IntersectionPos);

            checkReflect(IntersectionPos, new Vector3(x, y, 0));
        }
        else{

            pathPointList.Add(B);
        }
    }

    void checkReflect2(Vector3 A, Vector3 B)
    {
        if (Game.wallTransList.Count < 1)
        {
            pathPointList.Add(B);
            return;
        }
        float angle = 0;
        bool isIntersecion = false;
        Vector3 IntersectionPos = Vector3.zero;
        float x = 0, y = 0;
        float distance = 0;
        Vector3 normal = Vector3.zero, tangent = Vector3.zero;
        for (int i = 0; i < Game.wallTransList.Count; i++)
        {
            if (closeTransList.IndexOf(Game.wallTransList[i]) != -1) continue;
            //wallPos = Game.wallTransList[i].localPosition;
            //Wall wall = Game.wallTransList[i].GetComponent<Wall>();
            WallEx wall = Game.wallTransList[i].GetComponent<WallEx>();
            //wallRect = Game.wallTransList[i].GetComponent<RectTransform>();
            //width = wallRect.sizeDelta.x;
            //height = wallRect.sizeDelta.y;
            if (A.x <= wall.left.A.x)
            {
                if (A.y <= wall.down.A.y)
                {
                    heng = wall.down;
                }
                else if (A.y > wall.up.A.y)
                {
                    heng = wall.up;
                }
                shu = wall.left;
            }
            else if (A.x > wall.right.A.x)
            {
                if (A.y <= wall.down.A.y)
                {
                    heng = wall.down;
                }
                else if (A.y > wall.up.A.y)
                {
                    heng = wall.up;
                }
                shu = wall.right;
            }
            else if (A.y <= wall.down.A.y)
            {
                if (A.x <= wall.left.A.x)
                {
                    shu = wall.left;
                }
                else if (A.x > wall.right.A.x)
                {
                    shu = wall.right;
                }
                heng = wall.down;
            }
            else if (A.y > wall.up.A.y)
            {
                if (A.x <= wall.left.A.x)
                {
                    shu = wall.left;
                }
                else if (A.x > wall.right.A.x)
                {
                    shu = wall.right;
                }
                heng = wall.up;
            }

            //verticalV3 = wall.xianduanList[0].B;// new Vector3(wallPos.x, wallPos.y + height, 0);
            //horizontalV3 = wall.xianduanList.Count > 1 ? wall.xianduanList[1].B :  new Vector3(wallPos.x + width, wallPos.y, 0);
            distance = Mathf.Abs(Vector3.Distance(A, B));

            if (heng.A.x != 0 && heng.A.y != 0 && Tools.TryGetIntersectPoint(A, B, heng.A, heng.B, out IntersectionPos))
            {
                isIntersecion = true;

                Vector3 v3 = Vector3.Reflect((B - A).normalized, heng.normal);
                x = v3.x * distance;
                y = v3.y * distance;

                //angle = Vector3.Angle((verticalV3 - wallPos).normalized, (B - A).normalized);
                ////Debug.LogError(angle);
                //radian = ((wall.what * angle + wall.angle) * Mathf.PI) / 180;
                ////x = IntersectionPos.x + distance  * Mathf.Sin(radian);
                ////y = IntersectionPos.y + distance  * Mathf.Cos(radian);
                //x = IntersectionPos.x + distance * Mathf.Cos(radian);
                //y = IntersectionPos.y + distance * Mathf.Sin(radian);
            }
            else if (shu.A.x != 0 && shu.A.y != 0 && Tools.TryGetIntersectPoint(A, B, shu.A, shu.B, out IntersectionPos))
            {
                isIntersecion = true;
                Vector3 v3 = Vector3.Reflect((B - A).normalized, shu.normal);
                x = v3.x * distance;
                y = v3.y * distance;
                //angle = Vector3.Angle( (horizontalV3 - wallPos).normalized, (B - A).normalized);
                //radian = (-angle * Mathf.PI) / 180;
                //x = IntersectionPos.x - distance  * Mathf.Sin(radian);
                //y = IntersectionPos.y - distance  * Mathf.Cos(radian);
            }
            if (isIntersecion)
            {
                closeTransList.Add(Game.wallTransList[i]);
                break;
            }
        }

        if (isIntersecion)
        {
            pathPointList.Add(IntersectionPos);

            checkReflect2(IntersectionPos, new Vector3(x, y, 0));
        }
        else
        {

            pathPointList.Add(B);
        }
    }

}
