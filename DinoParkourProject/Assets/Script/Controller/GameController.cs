using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CameraController CC;
    public UIManager UIM;
    public SoundsController SC;
    public TimeRingController timeRing;
    private List<LandController> landList;
    private GameObject starPageLand;
    private DinoController DC;
    private Vector3 scale;
    public Vector3 Scale 
    { 
        get => scale; 
        set
        {
            scale = value;
            float x = timeRing.transform.localScale.x * scale.x;
            float y = timeRing.transform.localScale.y * scale.y;
            timeRing.transform.localScale = new Vector3(x+5f, y+5f, 1);
        }
    }
    private int time;
    public int nowScore;
    public int bestScore;

    //游戏当前状态 0:开始界面，1:游戏中，2:暂停中，3:游戏失败
    public int gameState = 0;

    void Start()
    {
        CC = GameObject.Find("MainCamera").GetComponent<CameraController>();
        UIM = GameObject.Find("UI").GetComponent<UIManager>();
        SC = gameObject.GetComponent<SoundsController>();
        SC.setBGMClip("Sounds/BGM");
        timeRing = GameObject.Find("TimeRing").GetComponent<TimeRingController>();
        starPageLand = GameObject.Find("LandPrefab");
        DC = GameObject.Find("DinoPrefab").GetComponent<DinoController>();
    }
    void Update()
    {
        if (gameState != 1)
            return;
        if (landList != null && landList.Count <= 3)
        {
            if (landList[0].transform.position.x + 24 - 0.25f <= CC.rightBorder && landList[1].isMoving == false)
            {
                float speed = LandFactory.baseSpeed * (float)(1 + LandFactory.addSpeedStep * ((nowScore-10)/100.0));
                Vector3 startPosition = LandFactory.initPosition;
                startPosition.x = CC.rightBorder + 24f;
                landList[1].move(startPosition, speed);
            }   
        }
        nowScore = time / 10;
        UIM.UpdateScore(nowScore);
    }
    private void FixedUpdate()
    {
        if(gameState == 1)
        {
            time++;
            if((time/10.0f)%100 == 0 && time!= 0)
                SC.PlayVoice("Sounds/NextScore");
        }
            
    }

    public void GameStart()
    {
        gameState = 1;
        DC.State = DinoController.DinoState.Run;
        DC.dinoRigidbody.isKinematic = false;

        CC.moveToGamePagePos();
        SC.PlayBGM();
        SC.PlayVoice("Sounds/Start");

        timeRing.IsDayTime = true;
        timeRing.IsPlaying = true;

        time = 0;
        nowScore = 0;
        LoadBestScore();
        UIM.UpdateBestScore(bestScore);

        destroyEntity();
        landList = LandFactory.getStartLands();
        landList[0].move(landList[0].transform.position, LandFactory.baseSpeed);
    }
    public void GamePause()
    {
        gameState = 2;
        timeRing.IsPlaying = false;
        foreach (LandController i in landList)
            i.stopMove();
        DC.State = DinoController.DinoState.Start;
        SC.PauseBGM();
    }
    public void GameContinue()
    {
        gameState = 1;
        timeRing.IsPlaying = true;
        for (int i = 0; i < landList.Count; i++)
        {
            if (landList[i].transform.position.x <= CC.rightBorder + 24f)
                landList[i].move(landList[i].transform.position, landList[i].moveSpeed);
        }
        DC.State = DinoController.DinoState.Run;
        SC.PlayBGM();
    }
    public void GameOver()
    {
        gameState = 3;
        timeRing.IsPlaying = false;
        SC.PauseBGM();
        foreach (LandController i in landList)
            i.stopMove();
        UIM.UpdateScore(nowScore);

        if (nowScore > bestScore)
        {
            UIM.OnGG(true);
            SaveBestScore();
            if(Random.Range(0f,1f)>0.5)
                SC.PlayVoice("Sounds/NewScore1");
            else
                SC.PlayVoice("Sounds/NewScore2");
            Debug.Log(time + "_" + nowScore + "_" + bestScore);
        }
        else
        {
            SC.PlayVoice("Sounds/Dead");
            UIM.OnGG(false);
        }
    }
    public void SaveBestScore()
        => GameArchive.saveGameArchive(new GameArchive(nowScore));
    public void LoadBestScore()
    {
        GameArchive ga = GameArchive.loadGameArchive();
        if (ga != null)
            bestScore = ga.bestScore;
        else
            bestScore = 0;
    }

    public void getNextLand()
    {
        landList.RemoveAt(0);
        landList.Add(LandFactory.getLand(false));
    }
    private void destroyEntity()
    {
        if (starPageLand != null)
            Destroy(starPageLand);
        if (landList == null)
            return;
        foreach(LandController i in landList)
            Destroy(i.gameObject);
        landList.Clear();
    }
}
