using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    private List<Player> players;
    private static GameMaster master;
    private Dictionary<byte, TextMeshProUGUI> playerIDToScoreUI;
    private Dictionary<byte, int> playerIDToScore;

    public GameObject topWall;
    public GameObject leftWall;

    public int width { get; set; }
    public int height { get; set; }
    public byte playersTurn { get; set; }
    public float UpdatePeriod = 0.1f;

    public Apple apple;
    public GameObject UI;
    public GameObject UIRow;

    void Awake()
    {
        master = this;
        width = (int)topWall.transform.localScale.x;
        height = (int)leftWall.transform.localScale.y;
        players = new List<Player>();
        playerIDToScoreUI = new Dictionary<byte, TextMeshProUGUI>();
        playerIDToScore = new Dictionary<byte, int>();
    }
    void Start()
    {
        CreateApple();
        CreatePlayers();
    }
    // Update is called once per frame
    void Update()
    {
        foreach (Player player in players)
        {
            playersTurn = player.id;
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
        int x = (int)Random.Range(1, width-1);
        int y = (int)Random.Range(1, height-1);
        apple = Instantiate(apple, new Vector3(x, y), Quaternion.identity).GetComponent<Apple>();
        apple.name = "Apple";
    }
    private void CreatePlayers()
    {
        byte id = 100;
        float offset = 100;
        foreach(GameObject player in playerPrefabs)
        {
            int x = (int)Random.Range(1, width-1);
            int y = (int)Random.Range(1, height-1);
            Player playerInst = Instantiate(player, new Vector3(x, y), Quaternion.identity).GetComponent<Player>();
            playerInst.name = player.name;
            playerInst.id = id++;
            playerInst.gameObject.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
            players.Add(playerInst);

            //Create UI Elements
            GameObject ui_row = Instantiate(UIRow, UI.transform);
           // ui_row.transform.position = new Vector3(-909, 549 - offset, 0);
            ui_row.transform.position = new Vector3(60, 1249 - offset, 0);
            offset += 100;

            TextMeshProUGUI[] values = ui_row.GetComponentsInChildren<TextMeshProUGUI>();
            if(values.Length != 2)
            {
                Debug.LogError("Wtf? There should be only 2 TMP UI Elements");
            }
            values[0].text = playerInst.id+"";
            values[0].color = playerInst.gameObject.GetComponent<Renderer>().material.color;
            values[1].text = 0+"";
            playerIDToScoreUI.Add(playerInst.id, values[1]);
            playerIDToScore.Add(playerInst.id, 0);
        }
    }
    private void Respawn(Player player)
    {
        player.Die();
        int x = (int)Random.Range(0, width);
        int y = (int)Random.Range(0, height);
        player.transform.position = new Vector3(x,y);

        foreach(Player other_player in players)
        {
            if(other_player.id != player.id)
            {
                playerIDToScore[other_player.id] += 1;
                playerIDToScoreUI[other_player.id].text = playerIDToScore[other_player.id]+"";
            }
        }

        
    }
}
