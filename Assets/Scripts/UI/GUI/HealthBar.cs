using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Behaviours.Combat.Player;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private int health;

    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    void Awake() {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    void Update(){
        health = playerHealth.hitPoints;
        if (health > numOfHearts){
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++){
            if(i < health){
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts){
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }
}
