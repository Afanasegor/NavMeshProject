using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Toggle autoAim;
    [SerializeField] private Controller player;

    private void Update()
    {
        if (autoAim.isOn)
        {
            player.autoAim = true;
        }
        else
        {
            player.autoAim = false;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
