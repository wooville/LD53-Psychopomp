using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    // [SerializeField] private GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        // _input.DeliverEvent += HandlePause;
        // _input.ResumeEvent += HandleResume;
        // _input.SkipIntroEvent += HandleSkipIntro;
    }

    // private void HandlePause() {
    //     Time.timeScale = 0;
    //     pauseMenu.SetActive(true);
    // }

    // private void HandleResume() {
    //     Time.timeScale = 1;
    //     pauseMenu.SetActive(false);
    // }
}
