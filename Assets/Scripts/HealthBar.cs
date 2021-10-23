using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float health = 3;
    private float fullhealth;
    private GameObject hb;

    private void Start()
    {
        fullhealth = health;
        hb = transform.GetChild(0).transform.GetChild(1).gameObject;
    }

    public void Damage(int value)
    {
        health -= value;
        if(health > 0)
        {
        hb.transform.localScale = new Vector3(health / fullhealth, .1f, 1);
        }
        if(health <= 0)
        {
            GameManager.enemies.Remove(gameObject);
            GameManager.UpdateGoGrid(gameObject.transform.position, null);
            Destroy(gameObject);
        }
    }
}
