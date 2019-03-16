using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    class GameState
    {
        private byte[,] gameField;
        private List<Snake> snakes;
        private byte playersTurn;
        private GameMaster gm;

        public GameState(byte[,] field, List<Snake> snakes, byte turn)
        {
            this.gameField = field;
            this.snakes = snakes;
            this.playersTurn = turn;
            gm = GameMaster.GetInstance();
        }

        public GameState()
        {
            snakes = new List<Snake>();
            gm = GameMaster.GetInstance();

            gameField = new byte[gm.width, gm.height];

            playersTurn = gm.playersTurn;

            //Set Walls
            PlaceWalls(gameField);

            //Place Apple
            PlaceApples(gameField);

            //Place Players
            PlaceInitPlayers(gameField);
        }

        private void PlaceInitPlayers(byte[,] field)
        {
            foreach (Player player in gm.getPlayers())
            {
                int x = (int)player.transform.position.x;
                int y = (int)player.transform.position.y;
                if(x < 0 || x >= gm.width)
                {
                    return;
                }
                if(y < 0 || y >= gm.height)
                {
                    return;
                }

                Koordinate[] koords = new Koordinate[player.getParts().Count + 1];
                koords[0] = new Koordinate(x, y);
                field[x, y] = player.id;
                int i = 1;
                foreach (BodyPart part in player.getParts())
                {
                    x = (int)part.transform.position.x;
                    y = (int)part.transform.position.y;
                    koords[i++] = new Koordinate(x, y);
                    field[x, y] = player.id;
                }
                snakes.Add(new Snake(player.id, koords));
            }
        }
        private void PlacePlayers(byte[,] field, List<Snake> snakes)
        {
            foreach (Snake snake in snakes)
            {             
                foreach (Koordinate koord in snake.getPositions())
                {
                    field[koord.X, koord.Y] = snake.getID();
                }
            }
        }
        private void PlaceApples(byte[,] field)
        {
            field[(int)gm.apple.transform.position.x, (int)gm.apple.transform.position.y] = 2;
        }

        private void PlaceWalls(byte[,] field)
        {
            for (int i = 0; i < gm.width; i++)
            {
                for (int j = 0; j < gm.height; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        field[i, j] = 1;
                    }
                }
            }
        }

        public GameState MoveSnake(byte id, Direction dir)
        {
            List<Snake> newSnakes = getSnakesCopyWithMove(id, dir);
            byte[,] newField = new byte[gm.width, gm.height];
            PlaceWalls(newField);
            PlaceApples(newField);
            PlacePlayers(newField, newSnakes);
            return new GameState(newField,newSnakes,(byte)((playersTurn + 1) % snakes.Count));
        }

        private List<Snake> getSnakesCopyWithMove(byte id, Direction dir)
        {
            List<Snake> movedSnakes = new List<Snake>();
            foreach (Snake s in snakes)
            {
                Snake copy = new Snake(s);
                if (s.getID() == id)
                {
                    copy.Move(dir);
                }
                movedSnakes.Add(copy);
            }
            return movedSnakes;
        }
        
        public List<Snake> getSnakes()
        {
            return snakes;
        }
        public byte[,] getField()
        {
            return gameField;
        }
        //TODO: To be implemented!
        public void PrintState()
        {
            for(int x = 0; x < gm.width; x++)
            {
                for(int y = 0; y < gm.height; y++)
                {

                }
            }
        }
    }
}
