using System;
using System.Collections.Generic;

//Classes created to serialise data
namespace SerializableClasses
{
    [Serializable]
    public class Level
    {
        
        public List<PositionsVector3> Positions = new List<PositionsVector3>();
        //public List<Zone> Zones = new List<Zone>();
    }

   
    [Serializable]
    public class PositionsVector3
    {
        public string puzzleNumber;

        public float PositionX;
        public float PositionY;
        public float PositionZ;
    }

    [Serializable]
    public class GameUIInfo
    {
        //public int level { get; set; }
        //public int score { get; set; }

        public int level;
        public int score;
    }
}