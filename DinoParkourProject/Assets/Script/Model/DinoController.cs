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
            return state; }
        set
        {
            state = value;
            animator.SetInteger("DinoState", (int)value);
            switch (value)
            {
                case DinoState.Run:
                    run();
                    break;
                case DinoState.Jump:
                    jump();
                    break;
                case DinoState.Down:
                    down();
                    break;
                case DinoState.Dead:
                    dead();
                    break;
            }
        }
    }

    private GameController GC;
    public Animator animator;
    private List<Vector3> dinoBoxColliderSize
        = new List<Vector3> { new Vector3(2.5f, 3f, 1f), new Vector3(4f, 2f, 1f) };
    public Rigidbody dinoRigidbody;

    private void Start()
    {
        GC = GameObject.Find("GameController").GetComponent<GameController>();
        animator = gameObject.GetComponent<Animator>();
        dinoRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (GC.gameState != 1)
            return;
        if (Input.GetKey(KeyCode.UpArrow))
            State = DinoState.Jump;
        if (Input.GetKey(KeyCode.DownArrow))
            State = DinoState.Down;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            State = DinoState.Run;
    }

    private void setBoxCollider(bool isDown)
    {
        BoxCollider bc = gameObject.GetComponent<BoxCollider>();
        if (isDown)
            bc.size = dinoBoxColliderSize[1];
        else
            bc.size = dinoBoxColliderSize[0];
    }
    private void run()
        => setBoxCollider(false);
    private void jump()
    {
        if (transform.position.y > 0)
            return;
        setBoxCollider(false);
    }
    private void down()
        => setBoxCollider(true);
    private void dead()
        => dinoRigidbody.isKinematic = true;

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.name == "LandPrefab" || collisionInfo.gameObject.name == "LandPrefab(Clone)")
            return;
        State = DinoState.Dead;
        GameObject.Find("GameController").GetComponent<GameController>().GameOver(); 
    }
}
