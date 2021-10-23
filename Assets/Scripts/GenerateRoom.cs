using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GenerateRoom : MonoBehaviour
{
    public int width, height;
    private int w, h;
    public Tile ld_corner, lu_corner, rd_corner, ru_corner, l_wall, r_wall, wall;
    public Tile woodFloor;
    public Tilemap floor, elevation;

    public void ClearTiles()
    {
        floor.ClearAllTiles();
        elevation.ClearAllTiles();
    }

    public void GenerateRoomAt(Vector3Int pos)
    {
        w = 0;
        h = 0;
        int numOfRooms = Random.Range(3, 10);
        w += width + 1;
        h += height + 1;
        //width2 = (width2 < 2) ? Random.Range(3, 5): width2;
        //height2 = (height2 < 2)? Random.Range(3, 5): height2;
        Vector3Int ld_P = new Vector3Int(pos.x - w, pos.y - h, 0);
        Vector3Int ru_P = new Vector3Int(pos.x + w, pos.y + h, 0);

        SetCorners(ld_P, ru_P);
        BuildWalls(ld_P, ru_P);
        BuildFloor(ld_P);
    }

    void SetCorners(Vector3Int ld_pos, Vector3Int ru_pos)
    {
        if (elevation.GetTile(ld_pos) == null)
        {
            elevation.SetTile(ld_pos, ld_corner);
            GameManager.UpdateObGrid(ld_pos, 1);
        }

        if (elevation.GetTile(ru_pos) == null)
        {
            elevation.SetTile(ru_pos, ru_corner);
            GameManager.UpdateObGrid(ru_pos, 1);
        }

        Vector3Int lu_pos = new Vector3Int(ld_pos.x, ru_pos.y, 0);
        if (elevation.GetTile(lu_pos) == null)
        {
            elevation.SetTile(lu_pos, lu_corner);
            GameManager.UpdateObGrid(lu_pos, 1);
        }

        Vector3Int rd_pos = new Vector3Int(ru_pos.x, ld_pos.y, 0);
        if (elevation.GetTile(rd_pos) == null)
        {
            elevation.SetTile(rd_pos, rd_corner);
            GameManager.UpdateObGrid(rd_pos, 1);
        }
    }

    void BuildWalls(Vector3Int ld_pos, Vector3Int ru_pos)
    {
        int height = ru_pos.y - ld_pos.y;
        int width = ru_pos.x - ld_pos.x;

        for(int i = 1; i < height; i++)
        {
            Vector3Int leftWall = new Vector3Int(ld_pos.x, ld_pos.y + i, 0);
            Vector3Int rightWall = new Vector3Int(ld_pos.x + width, ld_pos.y + i, 0);
            GameManager.UpdateObGrid(leftWall, 1);
            GameManager.UpdateObGrid(rightWall, 1);
            elevation.SetTile(leftWall, l_wall);
            elevation.SetTile(rightWall, r_wall);
        }
        for (int i = 1; i < width; i++)
        {
            Vector3Int topWall = new Vector3Int(ld_pos.x + i, ld_pos.y, 0);
            Vector3Int bottomWall = new Vector3Int(ld_pos.x + i, ld_pos.y + height, 0);
            GameManager.UpdateObGrid(topWall, 1);
            GameManager.UpdateObGrid(bottomWall, 1);
            elevation.SetTile(topWall, wall);
            elevation.SetTile(bottomWall, wall);
        }

    }

    void BuildFloor(Vector3Int ld_pos)
    {
        for(int x = 1; x<w*2; x++)
        {
            for (int y = 1; y < h * 2; y++)
            {
                floor.SetTile(new Vector3Int(ld_pos.x + x, ld_pos.y + y, 0), woodFloor);
            }
        }
    }

}
