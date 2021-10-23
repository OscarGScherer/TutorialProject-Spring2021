using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{ 
    public int health = 3;
    public Sprite normal, damaged, breaking_point;
    private Sprite[] states;
    private float fullhealth;

    private void Start()
    {
        states = new Sprite[3];
        states[0] = breaking_point;
        states[1] = damaged;
        states[2] = normal;
        fullhealth = health;
    }

    public void Damage(int value)
    {
    health -= value;
    if (health > 0)
    {
        GetComponent<SpriteRenderer>().sprite = states[health - 1];
    }
    if (health <= 0)
    {
        GameManager.obstacles.Remove(gameObject);
        GameManager.UpdateGoGrid(gameObject.transform.position, null);
        Destroy(gameObject);
    }
    }
}