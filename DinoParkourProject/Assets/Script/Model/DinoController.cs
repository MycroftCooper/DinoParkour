using System.Collections.Generic;
using UnityEngine;

public class DinoController : MonoBehaviour
{
    public enum DinoState { Start, Run, Jump, Down, Dead }
    //Ð¡¿ÖÁú×´Ì¬£º0:¿ªÊ¼ 1:ÅÜ 2:Ìø 3:µÍÍ· 4:ËÀÍö
    public DinoState state;
    public DinoState State
    {
        get
        {
            state = (DinoState)animator.GetInteger("DinoState");
            return state; 
        }
        set
        {
            if (state == value)
                return;
            state = value;
            animator.SetInteger("DinoState", (int)value);
            updateCollision(state);
            if (state == DinoState.Dead)
                dinoRigidbody.isKinematic = true;
        }
    }

    private GameController GC;
    public Animator animator;
    private List<Sprite> dinoSprites;
    public Rigidbody2D dinoRigidbody;
    private SpriteRenderer SR;

    private void Start()
    {
        GC = GameObject.Find("GameController").GetComponent<GameController>();
        animator = gameObject.GetComponent<Animator>();
        dinoRigidbody = gameObject.GetComponent<Rigidbody2D>();
        gameObject.AddComponent<PolygonCollider2D>();
        dinoSprites = new List<Sprite>(Resources.LoadAll<Sprite>("EntityRes/Dino"));
        SR = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (GC.gameState != 1 || !GC.IsDino)
            return;
        if (Input.GetKey(KeyCode.UpArrow) && state != DinoState.Jump && transform.position.y == 0)
            State = DinoState.Jump;
        if (Input.GetKey(KeyCode.DownArrow) && state != DinoState.Down)
            State = DinoState.Down;
        if (Input.GetKeyUp(KeyCode.DownArrow) && state != DinoState.Run)
            State = DinoState.Run;
    }
    void updateCollision(DinoState nowState)
    {
        if (nowState == DinoState.Run)
            SR.sprite = dinoSprites.Find(p => p.name == "Dino_Run1");
        else if (nowState == DinoState.Down)
            SR.sprite = dinoSprites.Find(p => p.name == "Dino_Down1");
        else
            return;
        List<PolygonCollider2D> pc2ds = new List<PolygonCollider2D>(animator.gameObject.GetComponents<PolygonCollider2D>());
        foreach (PolygonCollider2D i in pc2ds)
            Destroy(i);
        animator.gameObject.AddComponent<PolygonCollider2D>();
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (GC.gameState != 1 || !GC.IsDino)
            return;
        State = DinoState.Dead;
        GC.gameObject.GetComponent<GameController>().GameOver(); 
    }
}
