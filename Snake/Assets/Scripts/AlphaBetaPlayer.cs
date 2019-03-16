using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaBetaPlayer : MonoBehaviour,Actor
{
    private Direction dir;
    private const int DEPTH = 8;
    private GameState actualState;

    //MaxPlayer Vars
    private byte myPlayerID;
    private int mySnakeAppleDepth;
    private bool mySnakeApple;

    //MinPlayerVars
    private byte otherPlayerID;
    private int enemySnakeAppleDepth;
    private bool enemySnakeApple;

    Direction Actor.NextMove()
    {
        //Erstelle den aktuellen GameState
        actualState = new GameState();
        GameMaster gm = GameMaster.GetInstance();
        myPlayerID = gm.playersTurn;
        foreach(Player p in gm.getPlayers())
        {
            if(p.id != myPlayerID)
            {
                otherPlayerID = p.id;
                break;
            }
        }
        initAlphaBetaVars();
        //Führe AlphaBetaPruning mit diesem aus
        int score = max(DEPTH, int.MinValue, int.MaxValue);

        return dir;
    }
    private void initAlphaBetaVars()
    {
        mySnakeApple = false;
        enemySnakeApple = false;
    }


    private int max(int depth, int alpha, int beta)
    {
        List<Direction> possibleDirections = getPossibleDirections(myPlayerID);
        snakeAtApplePos(depth, true, getMySnake());
        if (depth == 0 || possibleDirections.Count == 0 || mySnakeApple)
        {
            return evaluate(depth,possibleDirections);
        }
        int max = alpha;
        foreach(Direction dir in possibleDirections)
        {
            GameState oldState = actualState;
            actualState = actualState.MoveSnake(myPlayerID, dir);
            int score = min(depth - 1, max, beta);
            actualState = oldState;
            if(score > max)
            {
                max = score;
                if(max >= beta)
                {
                    break;
                }
                if(depth == DEPTH)
                {
                    this.dir = dir;
                }
            }
            if(depth == DEPTH)
            {
                //Debug.Log(myPlayerID + ": " + dir + " -> " + score);
            }
        }
        return max;
    }
    private int min(int depth, int alpha, int beta)
    {
        List<Direction> possibleDirections = getPossibleDirections(otherPlayerID);
        snakeAtApplePos(depth,false,getEnemySnake());

        if (depth == 0 || possibleDirections.Count == 0 || enemySnakeApple)
        {
            return evaluate(depth,possibleDirections);
        }
        int min = beta;
        foreach (Direction dir in possibleDirections)
        {
            GameState oldState = actualState;
            actualState = actualState.MoveSnake(otherPlayerID, dir);
            int score = max(depth - 1, alpha, min);
            actualState = oldState;
            if (score < min)
            {
                min = score;
                if (min <= alpha)
                {
                    break;
                }
            }
        }
        return min;
    }
    private int evaluate(int actualDepth, List<Direction> possibleDirs)
    {
        int score = 2 * actualDepth;
        if(possibleDirs.Count == 0)
        {
            score -= 100000;
        }
        GameMaster gm = GameMaster.GetInstance();
        int xApple = (int)gm.apple.transform.position.x;
        int yApple = (int)gm.apple.transform.position.y;


        Snake s = getMySnake();
        int xSnake = s.getPositions()[0].X;
        int ySnake = s.getPositions()[0].Y;

        Snake enemy = getEnemySnake();
        if(getPossibleDirections(enemy.getID()).Count == 0)
        {
            score += 100;
        }
       // score += (s.getPositions().Count - enemy.getPositions().Count) * 100;

        if(mySnakeApple)
        {
            score += mySnakeAppleDepth * 100;
        }
        if (enemySnakeApple)
        {
            score -= enemySnakeAppleDepth * 100;
        }
        score += 100 - 2 * getManhattanDistance(xApple, xSnake, yApple, ySnake);

        return score;
    }
    private int getManhattanDistance(int x1, int x2, int y1, int y2)
    {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }
    private void snakeAtApplePos(int depth, bool mySnake, Snake s)
    {
        GameMaster gm = GameMaster.GetInstance();
        int xApple = (int)gm.apple.transform.position.x;
        int yApple = (int)gm.apple.transform.position.y;

        int xSnake = s.getPositions()[0].X;
        int ySnake = s.getPositions()[0].Y;

        if( xApple == xSnake && yApple == ySnake)
        {
            if (mySnake)
            {
                mySnakeApple = true;
                mySnakeAppleDepth = depth;
            }
            else
            {
                enemySnakeApple = true;
                enemySnakeAppleDepth = depth;
            }
        }
    }
    private Snake getMySnake()
    {
        foreach(Snake s in actualState.getSnakes())
        {
            if(s.getID() == myPlayerID)
            {
                return s;
            }
        }
        return null;
    }
    private Snake getEnemySnake()
    {
        foreach (Snake s in actualState.getSnakes())
        {
            if (s.getID() != myPlayerID)
            {
                return s;
            }
        }
        return null;
    }
    private List<Direction> getPossibleDirections(byte playerID)
    {
        List<Direction> possibleDirs = new List<Direction>();
        foreach(Snake s in actualState.getSnakes())
        {
            if(s.getID() == playerID)
            {
                foreach(Direction dir in System.Enum.GetValues(typeof(Direction)))
                {
                    if (canMoveInDirection(s, dir))
                    {
                        possibleDirs.Add(dir);
                    }
                }
            }
        }
        return possibleDirs;
    }
    private bool canMoveInDirection(Snake snake, Direction dir)
    {
        int x = snake.getPositions()[0].X;
        int y = snake.getPositions()[0].Y;
        switch (dir)
        {
            case Direction.UP:
                y++;
                break;
            case Direction.DOWN:
                y--;
                break;
            case Direction.LEFT:
                x--;
                break;
            case Direction.RIGHT:
                x++;
                break;
        }
        if (isInsideField(x, y)) {
            byte fieldState = actualState.getField()[x, y];
            return fieldState == 0 || fieldState == 2;
        }
        else{
            return false;
        }
    }
    private bool isInsideField(int x, int y)
    {
        if(x >= 0 && x < actualState.getField().GetLength(0))
        {
            if(y >= 0 && y < actualState.getField().GetLength(1))
            {
                return true;
            }
        }
        return false;
    }
}
