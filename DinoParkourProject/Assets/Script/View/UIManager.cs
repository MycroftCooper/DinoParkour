using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameController GC;

    public GameObject stratPanel;
    public GameObject aboutPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject GGPanel;
    public GameObject NewBestScoreText;

    private Text nowScoreText;
    private Text bestScoreText;



    private LinkedList<Image> startPageBtnImg;
    private LinkedListNode<Image> onBtnImg;
    private List<Sprite> uiSprites;
    private Sprite btnBGSprite;
    private Sprite onBtnBGSprite;

    void Start()
    {
        GC = GameObject.Find("GameController").GetComponent<GameController>();

        uiSprites = new List<Sprite>(Resources.LoadAll<Sprite>("UI/UI"));
        btnBGSprite = uiSprites.Find(p => p.name == "UI_BtnBG");
        onBtnBGSprite = uiSprites.Find(p => p.name == "UI_OnBtnBG");

        startPageBtnImg = new LinkedList<Image>();
        startPageBtnImg.AddLast(GameObject.Find("StartBtn").GetComponent<Image>());
        startPageBtnImg.AddLast(GameObject.Find("AboutBtn").GetComponent<Image>());
        startPageBtnImg.AddLast(GameObject.Find("ExitBtn").GetComponent<Image>());
        onBtnImg = startPageBtnImg.First;
    }
    //-----------------------------------开始页面-------------------------------------------
    public void nextBtn()
    {
        onBtnImg.Value.sprite = btnBGSprite;
        onBtnImg = onBtnImg.Next;
        if (onBtnImg == null)
            onBtnImg = startPageBtnImg.First;
        onBtnImg.Value.sprite = onBtnBGSprite;
    }
    public void lastBtn()
    {
        onBtnImg.Value.sprite = btnBGSprite;
        onBtnImg = onBtnImg.Previous;
        if (onBtnImg == null)
            onBtnImg = startPageBtnImg.Last;
        onBtnImg.Value.sprite = onBtnBGSprite;
    }
    public void setSelectBtn(Image btnImg)
    {
        if (btnImg == onBtnImg.Value)
            return;
        onBtnImg.Value.sprite = btnBGSprite;
        if (btnImg != null)
        {
            onBtnImg = startPageBtnImg.Find(btnImg);
            onBtnImg.Value.sprite = onBtnBGSprite;
        }
        else
        {
            onBtnImg = startPageBtnImg.First;
            onBtnImg.Value.sprite = onBtnBGSprite;
        }
    }
    public void OnTheBtn()
    {
        string btnName = onBtnImg.Value.gameObject.name;
        if (btnName == "StartBtn")
            OnStartGame();
        else if (btnName == "AboutBtn")
            OnAboutGame();
        else
            OnExitGame();
    }
    //-----------------------------------游戏页面-------------------------------------------
    public void OnStartGame()
    {
        stratPanel.SetActive(false);
        gamePanel.SetActive(true);
        nowScoreText = GameObject.Find("NowScoreText").GetComponent<Text>();
        bestScoreText = GameObject.Find("BestScoreText").GetComponent<Text>();

        GC.GameStart();
    }
    public void OnAboutGame()
    {
        if(aboutPanel.activeInHierarchy)
            aboutPanel.SetActive(false);
        else
            aboutPanel.SetActive(true);
    }
    public void OnExitGame()
        => Application.Quit();
    public void OnPause()
    {
        pausePanel.SetActive(true);
        GC.GamePause();
    }
    public void OnContinue()
    {
        pausePanel.SetActive(false);
        GC.GameContinue();
    }
    public void OnRestart()
    {
        stratPanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        GGPanel.SetActive(false);
        aboutPanel.SetActive(false);
        OnStartGame();
    }
    public void OnGG(bool isNewScore)
    {
        GGPanel.SetActive(true);
        Text finalScoreText = GameObject.Find("FinalScoreText").GetComponent<Text>();
        finalScoreText.text = nowScoreText.text;
        if(isNewScore)
            NewBestScoreText.SetActive(true);
        else
            NewBestScoreText.SetActive(false);
    }
    public void OnBackToStartPage()
    {
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        GGPanel.SetActive(false);
        stratPanel.SetActive(true);
        GC.CC.moveToStartPgaePos();
        GC.gameState = 0;
    }

    public void UpdateScore(int score)
    => nowScoreText.text = score.ToString();
    public void UpdateBestScore(int score)
        => bestScoreText.text = score.ToString();

    void Update()
    {
        switch (GC.gameState)
        {
            case 0:
                //游戏开始界面
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                    OnTheBtn();
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    lastBtn();
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    nextBtn();
                break;
            case 1:
                //游戏中
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
                    OnPause();
                break;
            case 2:
                //暂停中
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
                    OnContinue();
                break;
            case 3:
                //游戏失败界面
                if (Input.GetKeyDown(KeyCode.Space))
                    OnRestart();
                break;
        }
    }
}
