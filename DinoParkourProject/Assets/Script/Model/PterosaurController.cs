using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterosaurController : MonoBehaviour
{
    public Vector3 initPos;
    public Rigidbody2D r2d;
    private GameController GC;
    
    void Start()
    {
        r2d = gameObject.GetComponent<Rigidbody2D>();
        GC = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        input();
    }
    private void input()
    {
        if (GC.gameState != 1 || GC.IsDino)
            return;
        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetMouseButtonDown(0))
            r2d.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                r2d.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (GC.gameState != 1 || GC.IsDino)
            return;
        r2d.isKinematic = true;
        GC.GameOver();
    }
}
