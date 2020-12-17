// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth
{
    public class ModelController : MonoBehaviour
    {
        [SerializeField]
        private float slowTime = 0.1f;

        private Animator animator;
        private List<BaseCloth> clothList = new List<BaseCloth>();

        private bool slow;

        void Start()
        {
            animator = GetComponent<Animator>();
            clothList.AddRange(GetComponentsInChildren<BaseCloth>());
        }

        void Update()
        {
        }

        public void OnNextButton()
        {
            animator.SetTrigger("Next");
        }

        public void OnBackButton()
        {
            animator.SetTrigger("Back");
        }

        public void OnSlowButton()
        {
            slow = !slow;

            float timeScale = slow ? slowTime : 1.0f;

            animator.speed = timeScale;
            foreach (var cloth in clothList)
            {
                cloth.SetTimeScale(timeScale);
            }
        }

        public void OnActiveButton()
        {
            foreach (var cloth in clothList)
            {
                cloth.gameObject.SetActive(!cloth.gameObject.activeSelf);
            }
        }
    }
}
