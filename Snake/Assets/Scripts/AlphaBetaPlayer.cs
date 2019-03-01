using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaBetaPlayer : MonoBehaviour,Actor
{
    private Direction dir;

    Direction Actor.NextMove()
    {
        SetDirection();
        return dir;
    }
    private void SetDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0 && dir != Direction.LEFT)
        {
            dir = Direction.RIGHT;
        }
        else if (horizontal < 0 && dir != Direction.RIGHT)
        {
            dir = Direction.LEFT;
        }
        else if (vertical > 0 && dir != Direction.DOWN)
        {
            dir = Direction.UP;
        }
        else if (vertical < 0 && dir != Direction.UP)
        {
            dir = Direction.DOWN;
        }
    }
}
