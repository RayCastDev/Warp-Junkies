using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    float deltaTime = 0.0f;

    private const int COIN_SCORE_AMOUNT = 1;
    public static GameController Instance { get; set; }

    public bool IsDead { get; set; }

    //private PlayerEngine playerController;
    public bool isGameStarted { get; set; }

    public Text scoreText, coinText, modifierText, fpsText;

    public float score, coinScore, modifierScore;

    private int lastScore;

    public GameEvent OnGameStart;
    float fps;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance !=this)
        {
            Destroy(gameObject);
        }

        modifierScore = 1;
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEngine>();
        modifierText.text = "x" + modifierScore.ToString("0.0");
        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString("0");       
    }

    private void Start()
    {
       QualitySettings.vSyncCount = 0;
       Application.targetFrameRate = 80;
       StartCoroutine(onCoroutine());
    }
    IEnumerator onCoroutine()
    {
        while (true)
        {
            fpsText.text = Mathf.Ceil(fps).ToString();
            yield return new WaitForSeconds(1f);
        }
    }
    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
       


        if (MobileInput.Instance.Tap && !isGameStarted && !IsDead)
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGameStarted = true;
            IsDead = false;
            SceneManager.LoadScene("GameScene");
            //StartGame();
        }

        if (isGameStarted && !IsDead)
        {
            
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }
    }

    private void StartGame()
    {
        IsDead = false;
        isGameStarted = true;
        //playerController.StartRuninig();
        OnGameStart.Raise();
    }

    private void StopGame()
    {
        isGameStarted = false;
        IsDead = true;
    }
    public void GetCoin()
    {
        
        coinScore += COIN_SCORE_AMOUNT;
        coinText.text = coinScore.ToString("0");
        score += 1;
        //scoreText.text = score.ToString("0");
    }


    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }
}
