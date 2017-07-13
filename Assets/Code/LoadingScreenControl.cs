using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

    public class LoadingScreenControl : MonoBehaviour
    {
        public GameObject loadingScreenObj;
        public Slider slider;

        AsyncOperation async;

        public void LoadScreenExample(string levelName)
        {
            StartCoroutine(LoadingScreen(levelName));
        }

        IEnumerator LoadingScreen(string lvlName)
        {
            loadingScreenObj.SetActive(true);
            async = Application.LoadLevelAsync(lvlName);
            async.allowSceneActivation = false;

            while(async.progress < 0.9f)
            {
                slider.value = async.progress;
                yield return null;
            }
            async.allowSceneActivation = true;
            yield return null;
            //while(async.isDone == false)
            //{
            //    slider.value = async.progress;
            //    if(async.progress == 0.9f)
            //    {
            //        slider.value = 1f;
            //        async.allowSceneActivation = true;
            //    }
            //    yield return null;
            //}
        }

    }

