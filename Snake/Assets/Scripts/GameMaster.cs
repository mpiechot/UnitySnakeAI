using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    private List<Player> players;
    private static GameMaster master;

    public GameObject topWall;
    public GameObject leftWall;

    public int width { get; set; }
    public int height { get; set; }

    public Apple apple;

    void Awake()
    {
        master = this;
        width = (int)topWall.transform.localScale.x;
        height = (int)leftWall.transform.localScale.y;
        players = new List<Player>();
    }
    void Start()
    {
        CreateApple();
        CreatePlayers();
    }
    // Update is called once per frame
    void Update()
    {
        foreach(Player player in players)
        {
            player.UpdatePlayer();
        }
    }
    public void handleCollision(Player player, Collider col)
    {
        if(col.tag == "Apple")
        {
            player.Grow(1);
            Destroy(col.gameObject);
            CreateApple();
        }
        if(col.tag == "Wall")
        {
            Respawn(player);
        }
        if(col.tag == "Snake")
        {
            if(player.length == 1 || col.gameObject.GetComponent<BodyPart>() == null)
            {
                return;
            }
            Respawn(player);
        }
    }
    public List<Player> getPlayers()
    {
        return players;
    }
    public static GameMaster GetInstance()
    {
        return master;
    }

    private void CreateApple()
    {
        int x = (int)Random.Range(-(width / 2), width / 2);
        int y = (int)Random.Range(-(height / 2), height / 2);
        apple = Instantiate(apple, new Vector3(x, y), Quaternion.identity).GetComponent<Apple>();
        apple.name = "Apple";
    }
    private void CreatePlayers()
    {
        byte id = 100;
        foreach(GameObject player in playerPrefabs)
        {
            int x = (int)Random.Range(-(width / 2), width / 2);
            int y = (int)Random.Range(-(height / 2), height / 2);
            Player playerInst = Instantiate(player, new Vector3(x, y), Quaternion.identity).GetComponent<Player>();
            playerInst.name = player.name;
            playerInst.id = id++;
            players.Add(playerInst);
        }
    }
    private void Respawn(Player player)
    {
        player.Die();
        int x = (int)Random.Range(-(width / 2), width / 2);
        int y = (int)Random.Range(-(height / 2), height / 2);
        player.transform.position = new Vector3(x,y);
    }
}
