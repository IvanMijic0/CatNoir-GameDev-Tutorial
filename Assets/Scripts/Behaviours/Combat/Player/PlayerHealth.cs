using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
using Behaviours.Movement;

namespace Behaviours.Combat.Player
{
    public class PlayerHealth : MonoBehaviour
    {
       
        public int hitPoints = 3;
        [SerializeField] private float delay = 5f;
        [SerializeField] private Animator anim;
        [SerializeField] private PlatformMovement playerMove;
        [SerializeField] private PlayerProjectileFire proj;

        public void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            playerMove = GameObject.Find("Player").GetComponent<PlatformMovement>();
            proj = GameObject.Find("Player").GetComponent<PlayerProjectileFire>();
        }

        private void Update()
        {
            if (hitPoints == 0)
            {
                playerMove.enabled = false;
                proj.enabled = false;
                anim.Play("Defeated");
                StartCoroutine(LoadLevel(delay));
            }
        }

        IEnumerator LoadLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(2);
        }
    }
}
