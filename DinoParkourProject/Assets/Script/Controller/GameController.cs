using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CameraController CC;
    public UIManager UIM;
    public SoundsController SC;
    public TimeRingController timeRing;
    private List<CarrierController> carrierList;
    private GameObject starPageLand;

    public DinoController DC;
    public PterosaurController PC;
    private CarrierFactory carrierF;

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
    private int nowScore;
    private int bestScore0 = 0;
    private int bestScore1 = 0;

    //游戏当前状态 0:开始界面，1:游戏中，2:暂停中，3:游戏失败
    public int gameState = 0;
    public bool IsDino;

    void Start()
    {
        CC = GameObject.Find("MainCamera").GetComponent<CameraController>();
        UIM = GameObject.Find("UI").GetComponent<UIManager>();
        SC = gameObject.GetComponent<SoundsController>();
        SC.setBGMClip("Sounds/BGM");
        timeRing = GameObject.Find("TimeRing").GetComponent<TimeRingController>();
        starPageLand = GameObject.Find("LandPrefab");
        IsDino = true;
    }
    void Update()
    {
        if (gameState != 1)
            return;
        if (carrierList != null && carrierList.Count <= 4)
        {
            if (carrierList[0].transform.position.x + carrierF.width - 0.25f <= CC.rightBorder && carrierList[1].isMoving == false)
            {
                float speed = carrierF.baseSpeed * (float)(1 + carrierF.addSpeedStep * ((nowScore-10)/100.0));
                Vector3 startPosition = carrierF.initPosition;
                startPosition.x = CC.rightBorder + carrierF.width;
                carrierList[1].move(startPosition, speed);
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
    public void changePlayer()
    {
        IsDino = !IsDino;
        if (IsDino)
        {
            DC.gameObject.SetActive(true);
            PC.gameObject.SetActive(false);
            carrierF = new LandFactory();
        }
        else
        {
            DC.gameObject.SetActive(false);
            PC.gameObject.SetActive(true);
            PC.transform.position = new Vector3(0, 0, 0);
            carrierF = new SkyFactory();
        }
        destroyEntity();
        carrierList = carrierF.getStartCarriers();
    }

    public void GameStart()
    {
        gameState = 1;
        timeRing.IsDayTime = true;
        timeRing.IsPlaying = true;
        time = 0;
        nowScore = 0;
        LoadBestScore();
        SC.PlayBGM();
        SC.PlayVoice("Sounds/Start");
        destroyEntity();

        if (IsDino)
        {
            carrierF = new LandFactory();
            DC.State = DinoController.DinoState.Run;
            DC.dinoRigidbody.isKinematic = false;
            UIM.UpdateBestScore(bestScore0);
            CC.moveToPos(1);
            carrierList = carrierF.getStartCarriers();
            carrierList[0].move(carrierList[0].transform.position, carrierF.baseSpeed);
        }
        else
        {
            PC.transform.position = new Vector3(0, 5, 0);
            carrierF = new SkyFactory();
            PC.r2d.constraints = (RigidbodyConstraints2D)5;
            PC.r2d.isKinematic = false;
            UIM.UpdateBestScore(bestScore1);
            CC.moveToPos(2);
            carrierList = carrierF.getStartCarriers();
            carrierList[0].move(carrierList[0].transform.position, carrierF.baseSpeed);
        }
    }
    public void GamePause()
    {
        gameState = 2;
        timeRing.IsPlaying = false;
        SC.PauseBGM();
        foreach (CarrierController i in carrierList)
            i.stopMove();

        if (IsDino)
            DC.State = DinoController.DinoState.Start;
        else
            PC.r2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void GameContinue()
    {
        gameState = 1;
        timeRing.IsPlaying = true;
        SC.PlayBGM();
        for (int i = 0; i < carrierList.Count; i++)
        {
            if (carrierList[i].transform.position.x <= CC.rightBorder + 24f)
                carrierList[i].move(carrierList[i].transform.position, carrierList[i].moveSpeed);
        }

        if (IsDino)
            DC.State = DinoController.DinoState.Run;
        else
            PC.r2d.constraints = (RigidbodyConstraints2D)5;
    }
    public void GameOver()
    {
        gameState = 3;
        timeRing.IsPlaying = false;
        SC.PauseBGM();
        SC.PlayVoice("Sounds/Dead");

        bool isNewScore = SaveBestScore();
        UIM.UpdateScore(nowScore);
        UIM.OnGG(isNewScore);
        if(isNewScore)
        {
            if (Random.Range(0f, 1f) > 0.5)
                SC.PlayVoice("Sounds/NewScore1");
            else
                SC.PlayVoice("Sounds/NewScore2");
        }
        else
            UIM.OnGG(false);
        foreach (CarrierController i in carrierList)
            i.stopMove();
    }
    public void ExitGame()
    {
        CC.moveToPos(0);
        gameState = 0;
        SaveBestScore();
        if (IsDino)
        {
            DC.State = DinoController.DinoState.Start;
            DC.dinoRigidbody.isKinematic = true;
        }  
        else
        {
            PC.transform.position = PC.initPos;
            PC.r2d.constraints = RigidbodyConstraints2D.FreezeAll;
            PC.r2d.isKinematic = true;
        }
    }
    public bool SaveBestScore()
    {
        if(IsDino)
        {
            if(nowScore > bestScore0)
            {
                GameArchive.saveGameArchive(new GameArchive(nowScore, bestScore1));
                return true;
            }
            return false;
        }
        else
        {
            if (nowScore > bestScore1)
            {
                GameArchive.saveGameArchive(new GameArchive(bestScore0, nowScore));
                return true;
            }
            return false;
        }
    }
    public void LoadBestScore()
    {
        GameArchive ga = GameArchive.loadGameArchive();
        if (ga != null)
        {
            bestScore0 = ga.bestScore0;
            bestScore1 = ga.bestScore1;
        }
        else
        {
            bestScore0 = 0;
            bestScore1 = 0;
        }   
    }

    public void getNextCarrier()
    {
        carrierList.RemoveAt(0);
        carrierList.Add(carrierF.getCarrier(false));
    }
    private void destroyEntity()
    {
        if (starPageLand != null)
            Destroy(starPageLand);
        if (carrierList == null)
            return;
        foreach(CarrierController i in carrierList)
            Destroy(i.gameObject);
        carrierList.Clear();
    }
}
