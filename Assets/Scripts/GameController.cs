using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController singleton;

    [SerializeField] private Toggle autoAim;
    [SerializeField] private PlayerController player;
    private GameMode gameMode;


    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1f;
    }

    private void Start()
    {
        gameMode = GameMode.walking;
    }    

    public void SetGameModeMethod(GameMode mode)
    {
        gameMode = GameMode.switchingMode;
        StartCoroutine(SetGameMode(mode));
    }

    IEnumerator SetGameMode(GameMode mode)
    {        
        yield return new WaitForSeconds(0.8f);
        player.ReadyForShoot();

        gameMode = mode;
    }

    public GameMode GetGameMode()
    {
        return gameMode;
    }

    public bool GetAutoAim()
    {
        return autoAim.isOn;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public enum GameMode : byte
    {
        walking,
        shooting,
        switchingMode // to select switching process
    }
}