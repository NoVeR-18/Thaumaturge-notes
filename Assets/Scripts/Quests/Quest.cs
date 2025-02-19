using System;
using UnityEngine;


namespace QuestPack
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Quest")]
    public class Quest : ScriptableObject
    {
        public Action PointsChanged;
        public QuestType QuestType;
        public QuestAwardType QuestAwardType;
        public string QuestInfo;
        public int Award;
        public int RemainToAwards;
        public int PointsToAwards;
        public bool isActive = true;


        public void AddPoints(int count)
        {
            RemainToAwards += count;
            if (RemainToAwards >= PointsToAwards)
            {
                isActive = false;
            }
            PointsChanged.Invoke();
        }
    }
}
