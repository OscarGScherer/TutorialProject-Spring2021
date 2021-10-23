using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite s1, s2, s3;
    public GameObject damageZonePrefab;

    public void Move(int x, int y)
    {
        if (GameManager.VectorToGoGrid(transform.position + new Vector3(x, y)) == null 
            && GameManager.VectorToObGrid(transform.position + new Vector3(x, y)) != 1)
        {
            transform.position += new Vector3(x, y);
        }
    }
    public void UpdateSprite(bool b1, bool turnEnded)
    {
        if(turnEnded && b1)
        {
            sr.sprite = s2;
        }
        else if(turnEnded && !b1)
        {
            sr.sprite = s1;
        }

    }
    public void DecideMove(int x, int y, bool b1)
    {
        if(b1 && (x != 0 || y != 0))
        {
            GameObject damageZone = Instantiate(damageZonePrefab, transform.position + new Vector3(x, y), transform.rotation);
            GameManager.currentAttacks.Add(damageZone);
            sr.sprite = s3;
        }
        else
        {
            Move(x, y);
        }
    }

}
