using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    struct PlayerHead
    {
        public int x, y;
        public byte id;

        public PlayerHead(int x, int y, byte id)
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }
    }

    class GameState
    {
        private byte[,] gameField;
        private List<PlayerHead> heads;
        private int playersTurn;
        private GameMaster gm;

        public GameState()
        {
            heads = new List<PlayerHead>();
            gm = GameMaster.GetInstance();
           
            gameField = new byte[gm.width,gm.height];

            playersTurn = 0;

            //Set Walls
            for(int i =0; i < gm.width; i++)
            {
                for(int j=0;j < gm.height; j++)
                {
                    if(i==0 || j == 0)
                    {
                        gameField[i, j] = 1;
                    }
                }
            }

            //Place Apple
            gameField[(int)gm.apple.transform.position.x, (int)gm.apple.transform.position.y] = 2;

            //Place Players
            foreach(Player player in gm.getPlayers())
            {
                int x = (int)player.transform.position.x;
                int y = (int)player.transform.position.y;
                heads.Add(new PlayerHead(x, y, player.id));
                gameField[x, y] = player.id;
                foreach(BodyPart part in player.getParts())
                {
                    x = (int)part.transform.position.x;
                    y = (int)part.transform.position.y;
                    gameField[x, y] = player.id;
                }
            }
        }

        public GameState MoveSnake(byte id, Direction dir)
        {


            return null;
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
