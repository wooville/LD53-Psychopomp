
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private TextMeshProUGUI winsText;
    [SerializeField] private TextMeshProUGUI retryText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI computerScoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float maxTimeSeconds;

    private bool _timerOn;
    public bool TimerOn
    {
        get => _timerOn;

        set
        {
            if (_timerOn == value) return;

            _timerOn = value;
        }
    }

    private float _timeLeft;
    public float TimeLeft
    {
        get => _timeLeft;

        set
        {
            if (_timeLeft == value) return;

            _timeLeft = value;
            timerText.SetText($"{Mathf.RoundToInt(_timeLeft)}");
        }
    }


    public static GameManager Instance { get; private set; }
    private int _playerScore;
    public int PlayerScore
    {
        get => _playerScore;

        set
        {
            if (_playerScore == value) return;

            _playerScore = value;
            playerScoreText.SetText($"{_playerScore}");
        }
    }
    private int _computerScore;
    public int ComputerScore
    {
        get => _computerScore;

        set
        {
            if (_computerScore == value) return;

            _computerScore = value;
            computerScoreText.SetText($"{_computerScore}");
        }
    }

    public static event Action onGameOver;

    public static event Action onGameReset;

    void Awake()
    {
        Instance = this;

        _playerScore = 0;
        _computerScore = 0;

        onGameOver += PauseGame;

        _input.RetryEvent += ResetGame;

        gameMenu.SetActive(false);
    }

    private void Start()
    {
        _timerOn = true;
        _timeLeft = maxTimeSeconds;
    }

    private void Update()
    {
        if (_timerOn)
        {
            if (_timeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Timer at zero");
                _timeLeft = 0;
                _timerOn = false;
                onGameOver?.Invoke();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        if (_playerScore > _computerScore)
        {
            winsText.SetText("Player Wins");

            // winsText.color = new Color(255, 159, 67, 1);
            // retryText.color = new Color(243, 104, 224, 1);
        }
        else
        {
            winsText.SetText("Computer Wins");

            // winsText.color = new Color(243, 104, 224, 1);
            // retryText.color = new Color(255, 159, 67, 1);
        }
        gameMenu.SetActive(true);
    }

    private void ResetGame()
    {
        onGameReset?.Invoke();

        _playerScore = 0;
        _computerScore = 0;
        _timeLeft = maxTimeSeconds;
        _timerOn = true;
        Time.timeScale = 1;
        gameMenu.SetActive(false);

    }
}
