using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameObject[,] gameobjectGrid;
    public static int[,] obstacleGrid;
    public static List<GameObject> enemies;
    public static List<GameObject> obstacles;
    public GameObject obstacle;
    public GameObject[] enemyType = new GameObject[3];
    public int numOfEnemies = 3;
    public int numOfObjects = 3;

    public GameObject playerDamageText;
    private int playerHitCount = 0;

    public GameObject playerPrefab;
    private GameObject player;

    public static int hInput;
    public static int vInput;
    public static bool b1Input;

    public static List<GameObject> currentAttacks;

    private bool turnEnded = true;

    private void Start()
    {
        Begin();
    }
    public void Restart()
    {
        GetComponent<GenerateRoom>().ClearTiles();
        MoveAll();
        playerHitCount = 0;
        playerDamageText.GetComponent<TextDisplay>().DisplayTxt("Hits taken: " + playerHitCount);
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        foreach (GameObject obs in obstacles)
        {
            Destroy(obs);
        }
        foreach (GameObject damageZone in currentAttacks)
        {
            Destroy(damageZone);
        }
        Destroy(player);
        Begin();
    }
    void Begin()
    {

        enemies = new List<GameObject>();
        obstacles = new List<GameObject>();
        player = Instantiate(playerPrefab, new Vector2(50, 50), Quaternion.identity);
        gameobjectGrid = new GameObject[100, 100];
        obstacleGrid = new int[100, 100];
        currentAttacks = new List<GameObject>();

        gameObject.GetComponent<GenerateRoom>().GenerateRoomAt(new Vector3Int(50, 50, 0));

        int roomWidth = GetComponent<GenerateRoom>().width;
        int roomHeight = GetComponent<GenerateRoom>().height;

        UpdateGoGrid(player.transform.position, player);

        for (int i = 0; i < numOfObjects; i++)
        {
            Vector2 pos = new Vector2(Random.Range(50 - roomWidth, 51 + roomWidth), Random.Range(50 - roomHeight, 51 + roomHeight));
            if (VectorToGoGrid(pos) != null)
            {
                i--;
            }
            else
            {
                GameObject obs = Instantiate(obstacle, pos, transform.rotation);
                UpdateGoGrid(pos, obs);
                obstacles.Add(obs);
            }
        }

        for (int i = 0; i < numOfEnemies; i++)
        {
            Vector2 pos = new Vector2(Random.Range(50 - roomWidth, 51 + roomWidth ), Random.Range(50 - roomHeight, 51 + roomHeight));
            if (VectorToGoGrid(pos) != null)
            {
                i--;
            }
            else
            {
                GameObject enemy = Instantiate(enemyType[Random.Range(0, 3)], pos, transform.rotation);
                UpdateGoGrid(pos, enemy);
                enemies.Add(enemy);
            }
        }
       PlanMoveAll();

    }
    IEnumerator ProgressTurn(float sec)
    {
        turnEnded = false;

        yield return new WaitForSeconds(.02f);

        UpdateGoGrid(player.transform.position, null);
        player.GetComponent<MovePlayer>().DecideMove(hInput, vInput, b1Input);
        UpdateGoGrid(player.transform.position, player);

        MoveAll();
        
        if (currentAttacks.Count > 0)
        {
            yield return new WaitForSeconds(.2f);
            ApplyDamage();
        }

        PlanMoveAll();
        yield return new WaitForSeconds(sec / 1000f);
        turnEnded = true;
    }

    // Update is called once per frame
    void Update()
    {
        hInput = (int)Input.GetAxisRaw("Horizontal");
        vInput = (int)Input.GetAxisRaw("Vertical");
        b1Input = Input.GetKey(KeyCode.Space);
        player.GetComponent<MovePlayer>().UpdateSprite(b1Input, turnEnded);
        if ((hInput != 0 || vInput != 0 || Input.GetKey(KeyCode.P)) && turnEnded == true)
        {
            StartCoroutine(ProgressTurn(150));
        }
    }
    void PlanMoveAll()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<MoveEnemy>().PlanMovement(player.transform.position);
            UpdateGoGrid(enemy.GetComponent<MoveEnemy>().movePosition, enemy);
        }
    }

    void MoveAll()
    {
        foreach (GameObject enemy in enemies)
        {
            UpdateGoGrid(enemy.transform.position, null);
            enemy.GetComponent<MoveEnemy>().Move();
            UpdateGoGrid(enemy.transform.position, enemy);
        }
    }

    public static void UpdateObGrid(Vector3 pos, int num)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        obstacleGrid[x, y] = num;
    }

    public static void UpdateGoGrid(Vector3 pos, GameObject go)
    {
        int x = (int) pos.x;
        int y = (int) pos.y;
        gameobjectGrid[x, y] = go;
    }

    public static int VectorToObGrid(Vector2 vec)
    {
        int x = (int)vec.x;
        int y = (int)vec.y;
        return obstacleGrid[x, y];
    }

    public static GameObject VectorToGoGrid(Vector2 vec)
    {
        int x = (int)vec.x;
        int y = (int)vec.y;
        return gameobjectGrid[x, y];
    }

    void ApplyDamage()
    {
        foreach(GameObject damageZone in currentAttacks)
        {
            GameObject target = (VectorToGoGrid(damageZone.transform.position));

            if (target != null)
            {
                if (target.tag == "Enemy" && damageZone.tag == "DamageZone")
                {
                    target.GetComponent<HealthBar>().Damage(1);
                }
                else if (target.tag == "Obstacle")
                {
                    target.GetComponent<ObjectDamage>().Damage(1);
                }
                else if (target.tag == "Player")
                {
                    playerHitCount += 1;
                    playerDamageText.GetComponent<TextDisplay>().DisplayTxt("Hits taken: " + playerHitCount);
                    Debug.Log("Hit");
                }

            }
            Destroy(damageZone);
        }

        currentAttacks.Clear();
    }
}
