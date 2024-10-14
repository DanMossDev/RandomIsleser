using System;
using System.Collections.Generic;

namespace RandomIsleser
{
    [Serializable]
    public class QuestData
    {
        public int ID;
        
        public bool IsStarted = false;
        public bool IsComplete = false;

        public int ObjectiveIndex = 0;

        public List<ObjectiveData> Objectives;
    }
}
