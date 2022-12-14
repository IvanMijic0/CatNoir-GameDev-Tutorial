using System.Collections;
using Audio;
using UnityEngine;

namespace Behaviours.Combat.Player
{
    public class PlayerProjectileFire: MonoBehaviour
    {
        private static readonly int Attack = Animator.StringToHash("attack");
        
        [Header("Projectile")]
        [SerializeField] private ProjectileBehaviour projectilePrefab;
        [SerializeField] private Transform launchOffset;

        [Header("Attacking")]
        [SerializeField] private float attackTime = 2f;

        private bool _isAttacking;

        public void FireProjectile(Animator anim, bool isAttacking, AudioManager audioManager)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (audioManager.audioSource.enabled)
                {
                    StartCoroutine(AttackSprite(attackTime, anim));
                    Instantiate(projectilePrefab, launchOffset.position, transform.rotation);
                    audioManager.PlaySound(1);
                }
            }
            
        }

        private IEnumerator AttackSprite(float timeOfAttack, Animator anim)
        {
            anim.SetBool(Attack, true);
            _isAttacking = true;
            yield return new WaitForSeconds(timeOfAttack);
            anim.SetBool(Attack, false);
            _isAttacking = false;
        }

        public bool GetIsAttacking()
        {
            return _isAttacking;
        }
        
          
            


          
    }
}