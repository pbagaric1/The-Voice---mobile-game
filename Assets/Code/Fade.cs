using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


    public class Fade : MonoBehaviour
    {
        public Image black;
        public Animator animator;


        public void Fading()
        {
            StartCoroutine(FadingCo());
        }

        private IEnumerator FadingCo()
        {
            animator.SetBool("Fade", true);
            yield return new WaitForSeconds(1f);
        }
    }

