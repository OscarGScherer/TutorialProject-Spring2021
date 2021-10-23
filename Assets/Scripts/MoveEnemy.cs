using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public GameObject movePointPrefab;
    public GameObject damageZonePrefab;
    public int movementType = 1;
    public Vector2 movePosition;
    public bool canDiag = true;
    public GameObject movePoint;
    
    private bool windup = false;
    private bool attack = false;
    private char attackAxis = 'n';

    void PawnMovement(Vector2 goal)
    {
        attack = false;

        Vector2 cPos = transform.position;
        List<Vector2> possibleMovements = new List<Vector2>();

        possibleMovements.Add(cPos + new Vector2(-1, 0));
        possibleMovements.Add(cPos + new Vector2(0, 1));
        possibleMovements.Add(cPos + new Vector2(1, 0));
        possibleMovements.Add(cPos + new Vector2(0, -1));

        possibleMovements.Add(cPos + new Vector2(-1, 1));
        possibleMovements.Add(cPos + new Vector2(1, 1));
        possibleMovements.Add(cPos + new Vector2(1, -1));
        possibleMovements.Add(cPos + new Vector2(-1, -1));

        Vector2 bestMovement = cPos;

        for (int i = 0; i < 4; i++)
        {
            if (possibleMovements[i] == goal)
            {
                GameObject damageZone = Instantiate(damageZonePrefab, possibleMovements[i], transform.rotation);
                GameManager.currentAttacks.Add(damageZone);
                windup = true;
                return;
            }
            if (Vector2.Distance(possibleMovements[i], goal) <= Vector2.Distance(bestMovement, goal))
            {
                if (GameManager.VectorToGoGrid(possibleMovements[i]) == null && GameManager.VectorToObGrid(possibleMovements[i]) != 1)
                {
                    bestMovement = possibleMovements[i];
                }
            }
        }

        if (cPos == bestMovement)
        {
            for (int i = 4; i < 8; i++)
            {
                if (Vector2.Distance(possibleMovements[i], goal) <= Vector2.Distance(bestMovement, goal))
                {
                    if (GameManager.VectorToGoGrid(possibleMovements[i]) == null && GameManager.VectorToObGrid(possibleMovements[i]) != 1)
                    {
                        bestMovement = possibleMovements[i];
                    }
                }

            }

        }

        movePosition = bestMovement;

        if (cPos != bestMovement)
        {
            movePoint = Instantiate(movePointPrefab, bestMovement, transform.rotation);
            GetComponent<DrawLine>().DrawToPoint(bestMovement);
        }
    }

    void RookMovement(Vector2 goal)
    {
       Vector2 cPos = transform.position;
       movePosition = AttackPatterns.RookAttackCheck(cPos, goal, damageZonePrefab);
       if (movePosition != new Vector2(-100, -100))
       {
            GetComponent<DrawLine>().DrawToPoint(movePosition);
            movePoint = Instantiate(movePointPrefab, movePosition, transform.rotation);
            windup = true;
            attackAxis = (movePosition.x == goal.x) ? 'y' : 'x';
       }
       else
       {
            attackAxis = 'n';
            PawnMovement(goal);
       }
        
    }

    void RangedMovement(Vector2 goal)
    {
        Vector2 cPos = transform.position;
        if (AttackPatterns.RangedAttackCheck(cPos, goal, damageZonePrefab, canDiag))
        {
            windup = true;
        }
        else
        {
            PawnMovement(goal);
        }
    }

    public void SpriteStatus()
    {
        GetComponent<SpriteManager>().UpdateSprite(windup, attack, attackAxis);
    }

    public void PlanMovement(Vector2 goal)
    {
        if(movementType == 1)
        {
            PawnMovement(goal);
        }
        else if(movementType == 2)
        {
            RangedMovement(goal);
        }
        else if(movementType == 3)
        {
            RookMovement(goal);
        }
        SpriteStatus();
    }

    public void Move()
    {
        if(windup && movementType == 3)
        {
            windup = false;
            SpriteStatus();
            transform.position = movePosition;
            Destroy(movePoint);
        }
        else if (windup)
        {
            windup = false;
            attack = true;
            SpriteStatus();
        }
        else
        {
            transform.position = movePosition;
            Destroy(movePoint);
        }
        GetComponent<DrawLine>().DeleteLine();
    }

}
