using QuestPack;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class QuestWindow : WindowBase
    {
        [Header("Title")]
        [SerializeField] Button _backButton;
        [SerializeField] Button _refreshButton;

        [Header("PointsContainer")]
        public int PointsToAward = 1000;
        private int _leftPointsToAward = 0;

        public TextMeshProUGUI PointsToGetAwardsText;
        public Image AwardsIcon;
        public Image AwardsSliderFill;
        public TextMeshProUGUI PointsLeftToGetAwardsText;

        [Header("Quests")]
        public Transform DailyQuestsContainer;
        public Transform WeeklyQuestsContainer;
        public List<Quest> QuestList = new List<Quest>();
        private List<QuestInfoView> questInfoViews;

        public static Action<int> UpdateMinutes;
        public static Action<int> CoinColected;
        public static Action<int> PepperColected;

        private void Start()
        {
            _leftPointsToAward = PlayerPrefs.GetInt("LeftPointsToAward");
            UpdatePointsContainer();
            InitializedQuests();

            _refreshButton.onClick.AddListener(ResetQuests);
            _backButton.onClick.AddListener(CloseTab);
        }
        private void InitializedQuests()
        {
            questInfoViews = new List<QuestInfoView>();
            foreach (Transform child in DailyQuestsContainer.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in WeeklyQuestsContainer.transform)
            {
                child.gameObject.SetActive(false);
            }
            PointsToGetAwardsText.text = $"Earn {PointsToAward} points to get Shiny Cube";
            AwardsSliderFill.fillAmount = Mathf.Clamp01(_leftPointsToAward / PointsToAward);
            PointsLeftToGetAwardsText.text = $"{_leftPointsToAward}/{PointsToAward} XP";
            var _questInfoViewPrefab = Resources.Load<QuestInfoView>("QuestInfo");
            foreach (Quest quest in QuestList)
            {
                if (quest.isActive)
                {
                    if (quest.QuestType == QuestType.Deily)
                    {
                        var _questInfoView = Instantiate(_questInfoViewPrefab.gameObject, DailyQuestsContainer).GetComponent<QuestInfoView>();
                        _questInfoView.Initialize(quest);
                        questInfoViews.Add(_questInfoView);
                    }
                    if (quest.QuestType == QuestType.Weekly)
                    {
                        var _questInfoView = Instantiate(_questInfoViewPrefab.gameObject, WeeklyQuestsContainer).GetComponent<QuestInfoView>();
                        _questInfoView.Initialize(quest);
                        questInfoViews.Add(_questInfoView);
                    }
                    if (quest.QuestAwardType == QuestAwardType.Time)
                    {
                        UpdateMinutes += quest.AddPoints;
                        quest.PointsChanged += UpdateQuestsConteiner;
                    }
                    if (quest.QuestAwardType == QuestAwardType.Coin)
                    {
                        CoinColected += quest.AddPoints;
                        quest.PointsChanged += UpdateQuestsConteiner;
                    }
                    if (quest.QuestAwardType == QuestAwardType.Pepper)
                    {
                        PepperColected += quest.AddPoints;
                        quest.PointsChanged += UpdateQuestsConteiner;
                    }
                }
            }
        }


        private void UpdatePointsContainer()
        {
            AwardsSliderFill.fillAmount = Mathf.Clamp01(_leftPointsToAward / PointsToAward);
            PointsLeftToGetAwardsText.text = $"{_leftPointsToAward}/{PointsToAward} XP";
        }

        private void UpdateQuestsConteiner()
        {
            for (int i = 0; i < questInfoViews.Count; i++)
            {
                var questInfoView = questInfoViews[i];
                questInfoView.UpdateInfo();
                if (questInfoView.quest.isActive == false)
                {
                    RemoveQuest(questInfoView);
                }
                UpdatePointsContainer();
            }
        }
        private void RemoveQuest(QuestInfoView questInfoView)
        {

            questInfoViews.Remove(questInfoView);
            UpdateMinutes -= questInfoView.quest.AddPoints;
            CoinColected -= questInfoView.quest.AddPoints;
            PepperColected -= questInfoView.quest.AddPoints;
            questInfoView.quest.PointsChanged -= UpdateQuestsConteiner;
            questInfoView.gameObject.SetActive(false);
            _leftPointsToAward += questInfoView.quest.Award;
            PlayerPrefs.SetInt("LeftPointsToAward", _leftPointsToAward);
            UpdatePointsContainer();

        }

        public void ResetQuests()
        {
            foreach (var quest in QuestList)
            {
                quest.RemainToAwards = 0;
                quest.isActive = true;
            }
            InitializedQuests();
        }
    }
}