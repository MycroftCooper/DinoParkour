using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 startPagePosition;
    public Vector3 gamePagePosition;
    public float moveSpeed;
    public TransfomTools TT;
    public float leftBorder;
    public float rightBorder;
    public float topBorder;
    public float downBorder;
    public float height;
    public float width;
    public Vector3 scale;
    public bool isOnPosition= false;

    private void Start()
        => TT = GameObject.Find("GameController").GetComponent<TransfomTools>();
    private void Update()
    {
        if (isOnPosition)
            return;
        if (transform.localPosition == gamePagePosition)
        {
            initBorder();
            GameObject.Find("GameController").GetComponent<GameController>().Scale = scale;
            isOnPosition = true;
        }
    }

    public void moveToStartPgaePos() 
        => TT.moveToPositionByCoroutine(transform, startPagePosition, moveSpeed);
    public void moveToGamePagePos()
        => TT.moveToPositionByCoroutine(transform, gamePagePosition, moveSpeed);

    public void initBorder()
    {
        //������������Ͻ�  ��Ϊ�ӿ��������Ͻ���1,1,��
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f,
         Mathf.Abs(-Camera.main.transform.position.z)));
        //����������߽�
        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        //���������ұ߽�
        rightBorder = cornerPos.x;
        //���������ϱ߽�
        topBorder = cornerPos.y;
        //���������±߽�
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        width = rightBorder - leftBorder;
        height = topBorder - downBorder;

        float deffultH = 13.85643f;
        float deffultW = 24.60183f;
        scale = new Vector3(width/deffultW, height/deffultH, 0);
    }
}
