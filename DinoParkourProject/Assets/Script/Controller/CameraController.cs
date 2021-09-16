using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 startPagePosition;
    public Vector3 gamePagePosition1;
    public Vector3 gamePagePosition2;

    public float moveSpeed;
    public TransfomTools TT;
    public float leftBorder;
    public float rightBorder;
    public float topBorder;
    public float downBorder;
    public float height;
    public float width;
    public Vector3 scale;
    public bool hasScaled = false;

    private void Start()
        => TT = GameObject.Find("GameController").GetComponent<TransfomTools>();

    public void moveToPos(int gameState)
    {
        switch(gameState)
        {
            //��ʼҳ��
            case 0:
                TT.moveToPositionByCoroutine(transform, startPagePosition, moveSpeed, 1,initBorder);
                break;
            //С����ģʽ
            case 1:
                TT.moveToPositionByCoroutine(transform, gamePagePosition1, moveSpeed, 1, initBorder);
                break;
            //����ģʽ
            case 2:
                TT.moveToPositionByCoroutine(transform, gamePagePosition2, moveSpeed, 1, initBorder);
                break;
        }
        
    }

    public void initBorder()
    {
        GameController GC = GameObject.Find("GameController").GetComponent<GameController>();
        if (GC.gameState != 1 || GC.gameState == 4)
            return;
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
        if (!hasScaled)
            initScale();
    }
    private void initScale()
    {
        GameController GC = GameObject.Find("GameController").GetComponent<GameController>();
        float deffultH = 13.85643f;
        float deffultW = 24.60183f;
        scale = new Vector3(width / deffultW, height / deffultH, 0);
        GC.Scale = scale;
        hasScaled = true;
    }
}
