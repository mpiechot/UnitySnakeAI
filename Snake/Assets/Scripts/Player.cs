using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    LEFT,
    RIGHT,
    DOWN,
    UP,
    STOP
}
public enum PlayerType
{
    PLAYER,
    AI
}
public class Player : MonoBehaviour
{
    //Control
    private Actor actor;
    private GameMaster gm;
    public byte id { get; set; }

    //Movement
    public Direction direction;
    
    private float delayTime = 0.0f;
    private Vector3 moveVector;

    //SnakeBody
    public GameObject bodyPartPrefab;
    private List<BodyPart> parts;
    public int length;
    private Color partColor;


    // Start is called before the first frame update
    void Start()
    {
        parts = new List<BodyPart>();
        gm = GameMaster.GetInstance();
        actor = GetComponent<Actor>();
        partColor = new Color(Random.value, Random.value, Random.value);
    }

    // Update is called once per frame
    public void UpdatePlayer()
    {
        direction = actor.NextMove();
        if(Time.time > delayTime)
        {
            delayTime += gm.UpdatePeriod;
            Move();
            UpdateBodyParts();
        }
    }
    public void Grow(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            BodyPart part = Instantiate(bodyPartPrefab, transform.position, Quaternion.identity).GetComponent<BodyPart>();
            if(parts.Count != 0)
            {
                part.transform.position = parts[parts.Count - 1].transform.position;
            }
            part.moveVector = Vector3.zero;
            parts.Add(part);
            part.gameObject.GetComponent<Renderer>().material.color = partColor;
        }
        length = parts.Count;
    }
    public void Die()
    {
        for (int i = parts.Count - 1; i >= 0; i--)
        {
            Destroy(parts[i].gameObject);
        }
        parts.Clear();
    }

    void OnTriggerEnter(Collider col)
    {
        gm.handleCollision(this, col);
    }
    public List<BodyPart> getParts()
    {
        return parts;
    }
    private void Move()
    {
        switch (direction)
        {
            case Direction.RIGHT:
                moveVector = new Vector3(1, 0, 0);
                break;
            case Direction.LEFT:
                moveVector = new Vector3(-1, 0, 0);
                break;
            case Direction.UP:
                moveVector = new Vector3(0, 1, 0);
                break;
            case Direction.DOWN:
                moveVector = new Vector3(0, -1, 0);
                break;
            case Direction.STOP:
                moveVector = Vector3.zero;
                break;
        }
        transform.Translate(moveVector);
    }
    private void UpdateBodyParts()
    {
        for(int i= parts.Count - 1; i >= 0; i--)
        {
            if(i == 0)
            {
                parts[i].UpdateBodyPart();
                parts[i].moveVector = moveVector;
            }
            else
            {
                parts[i].UpdateBodyPart();
                parts[i].moveVector = parts[i - 1].moveVector;
            }
        }
    }
}
