using TMPro;
using UnityEngine;
namespace QuestPack

{
    public enum QuestType
    {
        Deily,
        Weekly
    }
    public enum QuestAwardType
    {
        Time,
        Coin,
        Pepper
    }

    public class QuestInfoView : MonoBehaviour
    {
        [HideInInspector]
        public Quest quest;
        public TextMeshProUGUI QuestInfoText;
        public TextMeshProUGUI PointsCountText;
        public TextMeshProUGUI RemainToAwardsText;
        public void Initialize(Quest quest)
        {
            this.quest = quest;
            QuestInfoText.text = quest.QuestInfo;
            RemainToAwardsText.text = $"{quest.RemainToAwards} / {quest.PointsToAwards}";
            PointsCountText.text = $"{quest.Award} XP";
        }

        public void UpdateInfo()
        {
            //else
            RemainToAwardsText.text = $"{quest.RemainToAwards} / {quest.PointsToAwards}";
            //if (quest.RemainToAwards >= quest.PointsToAwards)
            //{
            //    return true;
            //}

            //return false;
        }
    }
}