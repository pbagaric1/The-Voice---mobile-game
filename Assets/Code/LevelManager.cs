using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set; }
    public CameraController Camera {get; private set;}
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }

    public Image black;
    public Animator animator;

    //public int CurrentTimeBonus
    //{
    //    get
    //    {
    //        var secondDifference = (int)(BonusCutoffSeconds - RunningTime.TotalSeconds);
    //        return Mathf.Max(0, secondDifference) * BonusSecondMultiplier;
    //    }
    //}

    private List<Checkpoint> _checkpoints;
    private int _currentCheckpointIndex;
    private DateTime _started;
    private int _savedPoints;

    public Checkpoint DebugSpawn;
    public int BonusCutoffSeconds;
    public int BonusSecondMultiplier;

    public List<PointStar> _microphones;
    public int _numberOfMicrophones;

    public GameObject currentCheckpoint;

public void Awake()
{
    _savedPoints = GameManager.Instance.Points;
    Instance = this;
}

public void Start()
{
    //_checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ThenByDescending(t => t.transform.position.y).ToList();
    //_currentCheckpointIndex = _checkpoints.Count > 0 ? 0 : -1;

    _microphones = FindObjectsOfType<PointStar>().ToList();
    _numberOfMicrophones = _microphones.Count;

    Player = FindObjectOfType<Player>();
    Camera = FindObjectOfType<CameraController>();
    currentCheckpoint = GameObject.Find("Level Start");

    _started = DateTime.UtcNow;

    //var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
    //foreach (var listener in listeners)
    //{
    //    for (var i = _checkpoints.Count - 1; i >= 0; i--)
    //    {
    //        var distance = ((MonoBehaviour)listener).transform.position.x - _checkpoints[i].transform.position.x;
    //        if (distance < 0)
    //            continue;

    //        _checkpoints[i].AssignObjectToCheckpoint(listener);
    //        break;
    //    }
    //}

#if UNITY_EDITOR
    if (DebugSpawn != null)
        DebugSpawn.SpawnPlayer(Player);
#endif
}

public void Update()
{
    //var isAtLAstCheckpoint = _currentCheckpointIndex + 1 >= _checkpoints.Count;
    //if (isAtLAstCheckpoint)
    //    return;

    //var distanceToNextCheckpoint = _checkpoints[_currentCheckpointIndex + 1].transform.position.x - Player.transform.position.x;
    //if (distanceToNextCheckpoint >= 0)
    //    return;

    //_checkpoints[_currentCheckpointIndex].PlayerLeftCheckpoint();
    //_currentCheckpointIndex++;
    //_checkpoints[_currentCheckpointIndex].PlayerHitCheckpoint();

    //GameManager.Instance.AddPoints(CurrentTimeBonus);
    //_savedPoints = GameManager.Instance.Points;
    //_started = DateTime.UtcNow;
}



    public void GotoNextLevel(string levelName)
{
    StartCoroutine(GotoNextLevelCo(levelName));
}

    private IEnumerator GotoNextLevelCo(string levelName)
    {
        if (GameManager.Instance.CollectedMicrophones != _numberOfMicrophones)
        {
            FloatingText.Show("You have to collect all the \nmicrophones to complete the level!", "CheckpointText", new CenteredTextPosition(.2f));
            yield return null;
            
        }

        else {

            Player.FinishLevel();
            //GameManager.Instance.AddPoints(CurrentTimeBonus);

            FloatingText.Show("Level Complete!", "CheckpointText", new CenteredTextPosition(.2f));
            yield return new WaitForSeconds(1);

            FloatingText.Show(string.Format("{0} points!", GameManager.Instance.Points), "CheckpointText", new CenteredTextPosition(.1f));
            yield return new WaitForSeconds(2f);

            animator.SetBool("Fade", true);
            yield return new WaitForSeconds(1f);

            if (string.IsNullOrEmpty(levelName))
                Application.LoadLevel("StartScreen");
            else
            {
                Application.LoadLevel(levelName);
                GameManager.Instance.ResetMicrophones();
            }
        }

        
    }
public void KillPlayer()
{
    StartCoroutine(KillPlayerCo());
}

private IEnumerator KillPlayerCo()
{
    Player.Kill();
    Camera.isFollowing = false;
    yield return new WaitForSeconds(2f);

    Camera.isFollowing = true;

    SpawnPlayer(Player);
    //if (_currentCheckpointIndex != -1)
    //    _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);

    //_started = DateTime.UtcNow;
    //GameManager.Instance.ResetPoints(_savedPoints);
}


public void SpawnPlayer(Player player)
{
    player.RespawnAt(currentCheckpoint.transform);

}

}

