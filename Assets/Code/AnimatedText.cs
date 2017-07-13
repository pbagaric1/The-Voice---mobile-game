using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


    public class AnimatedText : MonoBehaviour
    {
        public Text textArea;
        public Button buttonNext;
        public Button buttonSkip;
        public string[] strings;
        public float speed = 0.1f;
        private bool buttonSkipDisabled = false;

        int stringIndex = 0;
        int characterIndex = 0;

        public string levelName;

        void Start()
        {
            buttonNext.gameObject.SetActive(false);
            StartCoroutine(DisplayTimer());
        }
        IEnumerator DisplayTimer ()
        {
            yield return new WaitForSeconds(1f);
           while (1 == 1)
            {
                yield return new WaitForSeconds(speed);
                if (characterIndex > strings[stringIndex].Length)
                {
                    continue;
                }
                textArea.text = strings[stringIndex].Substring(0, characterIndex);
                characterIndex++;
            }
            
        }

        public void Update()
        {
            if (stringIndex < strings.Length && !buttonSkipDisabled)
            {
                if (characterIndex < strings[stringIndex].Length)
                {
                    buttonSkip.GetComponentInChildren<Text>().text = "Skip";
                }
                else if (stringIndex < strings.Length)
                {
                    buttonSkip.GetComponentInChildren<Text>().text = "Next";
                }
                //else
                //    buttonSkip.gameObject.SetActive(false);
            }
        }

        public void LoadLevel()
        {
            Application.LoadLevel(levelName);
        }

        public void SkipDialog()
        {
            if (stringIndex < strings.Length)
            {
                if (characterIndex < strings[stringIndex].Length)
                {
                    characterIndex = strings[stringIndex].Length;
                }
                else if (stringIndex < strings.Length - 1)
                {
                    stringIndex++;
                    characterIndex = 0;
                }
                else
                {
                    buttonSkipDisabled = true;
                    buttonNext.gameObject.SetActive(true);
                    buttonSkip.gameObject.SetActive(false);
                }
            }
        }
    }

