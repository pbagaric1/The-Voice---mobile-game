using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public string FirstLevel;

    //public void Update()
    //{
    //    if (!Input.GetMouseButtonDown(0))
    //        return;

    //    GameManager.Instance.Reset();
    //    Application.LoadLevel(FirstLevel);
    //}

    public void StartGame()
    {
        GameManager.Instance.Reset();
        Application.LoadLevel(FirstLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

