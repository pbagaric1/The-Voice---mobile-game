﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private List<IPlayerRespawnListener> _listeners;

    public void Awake()
    {
        _listeners = new List<IPlayerRespawnListener>();
    }

    public void PlayerHitCheckpoint()
    {
        StartCoroutine(PlayerHitCheckpointco());
    }

    private IEnumerator PlayerHitCheckpointco()
    {
        FloatingText.Show("Checkpoint!", "CheckpointText", new CenteredTextPosition(0.4f));
        yield return new WaitForSeconds(.5f);
        //FloatingText.Show(string.Format("+{0} time bonus!", bonus), "CheckpointText", new CenteredTextPosition(.5f));
    }

    public void PlayerLeftCheckpoint()
    {

    }

    public void AssignObjectToCheckpoint(IPlayerRespawnListener listener)
    {
       // _listeners.Add(listener);
    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);

        foreach (var listener in _listeners)
            listener.OnPlayerRespawnInThisCheckpoint(this, player);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)
            return;

        if (LevelManager.Instance.currentCheckpoint == gameObject)
            return;
        PlayerHitCheckpoint();
        LevelManager.Instance.currentCheckpoint = gameObject;
    }

}

