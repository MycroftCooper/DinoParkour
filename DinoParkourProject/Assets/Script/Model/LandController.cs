using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandController : MonoBehaviour
{
    //初始化位置
    private Vector3 initPosition;
    //销毁位置
    private Vector3 destroyPosition;
    //障碍列表
    private List<GameObject> entitys;

    private TransfomTools TT;
    public float moveSpeed;
    public Coroutine movement;
    public bool isMoving;

    public void init(Vector3 initP, Vector3 destoryP)
    {
        TT = GameObject.Find("GameController").GetComponent<TransfomTools>();
        this.initPosition = initP;
        this.destroyPosition = destoryP;
        entitys = new List<GameObject>();
        setPosition(initPosition);
        isMoving = false;
    }
    public void setPosition(Vector3 position) 
        => gameObject.transform.position = position;

    public void addEntity(GameObject entity, Vector3 position)
    {
        entity.transform.parent = gameObject.transform;
        entity.transform.localPosition = position;
        entitys.Add(entity);
    }

    public void move(Vector3 startPosition, float moveSpeed)
    {
        if(startPosition != null)
            setPosition(startPosition);
        this.moveSpeed = moveSpeed;
        isMoving = true;
        movement = TT.moveToPositionByCoroutine(transform, destroyPosition, moveSpeed);
    }
    public void stopMove()
    {
        if (movement == null)
            return;
        TT.stopCoroutine(movement);
        isMoving = false;
    }

    void OnBecameInvisible()
    {
        if (!isMoving)
            return;
        GameObject.Find("GameController").GetComponent<GameController>().getNextLand();
        TT.stopCoroutine(movement);
        Destroy(gameObject);
    }
}