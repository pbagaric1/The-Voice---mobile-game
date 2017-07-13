﻿using UnityEngine;

public class GameHud : MonoBehaviour
{
    public GUISkin Skin;

    public void OnGUI()
    {
        GUI.skin = Skin;

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            GUILayout.BeginVertical(Skin.GetStyle("GameHud"));
            {
                GUILayout.Label(string.Format("Points: {0}", GameManager.Instance.Points), Skin.GetStyle("PointsText"));

                var time = LevelManager.Instance.RunningTime;
                GUILayout.Label(string.Format(
                    "{0:00}:{1:00}",
                    time.Minutes + (time.Hours * 60),
                    time.Seconds), Skin.GetStyle("TimeText"));

                //GUILayout.Label(string.Format("Microphones: {0} / {1}", GameManager.Instance.CollectedMicrophones, LevelManager.Instance._numberOfMicrophones), Skin.GetStyle("MicrophonesText"));
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            GUILayout.BeginVertical(Skin.GetStyle("GameHud"));
            {
                GUILayout.Label(string.Format("Microphones: {0} / {1}", GameManager.Instance.CollectedMicrophones, LevelManager.Instance._numberOfMicrophones), Skin.GetStyle("MicrophonesText"));
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }
}

