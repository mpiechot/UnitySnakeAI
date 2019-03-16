using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    class Snake
    {
        private byte id;
        private List<Koordinate> positions = new List<Koordinate>();

        public Snake(byte id, params Koordinate[] koords)
        {
            this.id = id;
            //Snake Head should be at koords[0] -> List.add inserts at the end of the List, so the head will be at index 0
            foreach(Koordinate koord in koords)
            {
                Koordinate copyKoord = new Koordinate(koord.X, koord.Y);
                positions.Add(copyKoord);
            }
        }
        public Snake(Snake copy)
        {
            this.id = copy.getID();
            foreach(Koordinate koord in copy.getPositions())
            {
                Koordinate copyKoord = new Koordinate(koord.X, koord.Y);
                positions.Add(copyKoord);
            }
        }

        public byte getID()
        {
            return id;
        }
        public List<Koordinate> getPositions()
        {
            return positions;
        }

        //Snake Functions
        public void Grow()
        {
            Koordinate koord = new Koordinate(positions.Last<Koordinate>().X, positions.Last<Koordinate>().Y);
            positions.Add(koord);
        }
        public void Move(Direction dir)
        {
            Koordinate newPos = new Koordinate(positions.First<Koordinate>().X, positions.First<Koordinate>().Y);
            switch (dir)
            {
                case Direction.UP:
                    newPos.Y++;
                    break;
                case Direction.DOWN:
                    newPos.Y--;
                    break;
                case Direction.LEFT:
                    newPos.X--;
                    break;
                case Direction.RIGHT:
                    newPos.X++;
                    break;
            }
            positions.Insert(0, newPos);
            positions.RemoveAt(positions.Count - 1);
        }
    }
}
