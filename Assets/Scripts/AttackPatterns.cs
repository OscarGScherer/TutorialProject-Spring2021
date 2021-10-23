using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{

    public static Vector2 RookAttackCheck(Vector2 origin, Vector2 target, GameObject effectTile)
    {
        int targetY = (int)target.y, targetX = (int)target.x;
        int originY = (int)origin.y, originX = (int)origin.x;
        Vector2 movePosition = origin;
        if (targetX == originX || targetY == originY)
        {
            int dirY = (originY > targetY) ? -1 : (originY < targetY) ? 1 : 0;
            int dirX = (originX > targetX) ? -1 : (originX < targetX) ? 1 : 0;

            Vector2 ppos = new Vector2(originX, originY);
            GameObject damageZone = Instantiate(effectTile, ppos, Quaternion.identity);
            GameManager.currentAttacks.Add(damageZone);
            Vector2 pos = new Vector2(originX + dirX, originY + dirY);

            for (int x = dirX, y = dirY; 
                GameManager.VectorToObGrid(pos) != 1 && 
                (GameManager.VectorToGoGrid(ppos) == null || GameManager.VectorToGoGrid(ppos).tag != "Obstacle");
                x += dirX, y += dirY)
            {
                damageZone = Instantiate(effectTile, pos, Quaternion.identity);
                GameManager.currentAttacks.Add(damageZone);
                movePosition = (GameManager.VectorToGoGrid(pos) == null) ? pos : movePosition;
                ppos = new Vector2(originX + x, originY + y);
                pos = new Vector2(originX + dirX + x, originY + dirY + y);
            }

            return movePosition;
        }

        return new Vector2(-100, -100);
    }

    public static bool RangedAttackCheck(Vector2 origin, Vector2 target, GameObject effectTile, bool canDiag = true)
    {
        int targetY = (int)target.y, targetX = (int)target.x;
        int originY = (int)origin.y, originX = (int)origin.x;
        float dist = Vector2.Distance(origin, target);
        if ((canDiag && (Mathf.Abs(targetY - originY) == Mathf.Abs(targetX - originX))) || targetX == originX || targetY == originY)
        {
            int dirY = (originY > targetY) ? -1 : (originY < targetY) ? 1 : 0;
            int dirX = (originX > targetX) ? -1 : (originX < targetX) ? 1 : 0;

            Vector2 pos = new Vector2(originX + dirX, originY + dirY);
            Vector2 ppos = new Vector2(originX, originY);

            for (int x = dirX, y = dirY; 
                GameManager.VectorToObGrid(pos) != 1 && 
                (GameManager.VectorToGoGrid(ppos) == null || GameManager.VectorToGoGrid(ppos).tag != "Obstacle");
                x += dirX, y += dirY)
            {
                GameObject damageZone = Instantiate(effectTile, pos, Quaternion.identity);
                GameManager.currentAttacks.Add(damageZone);
                pos = new Vector2(originX + dirX + x, originY + dirY + y);
                ppos = new Vector2(originX + x, originY + y);
            }

            return true;
        }

        return false;
    }
}
