using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite s1, s2, s3;
    public bool isBarrel = false;

    public void UpdateSprite(bool windup, bool attack, char attackAxis)
    {
        if (isBarrel)
        {
            if(attackAxis == 'x')
            {
                sr.sprite = s2;
            }
            else if(attackAxis == 'y')
            {
                sr.sprite = s3;
            }
            else
            {
                sr.sprite = s1;
            }
        }
        else if (!windup && !attack)
        {
            sr.sprite = s1;
        }
        else if (windup)
        {
            sr.sprite = s2;
        }
        else if (attack)
        {
            sr.sprite = s3;
        }
    }
}
