using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameState : MonoBehaviour
{
    [Header("Times")]
    [SerializeField] float eventInterval = 10f;
    float eventTimer = 0f;
    [SerializeField] float dayLength = 60f;
    float dayTime = 0f;
    int dayIndex = 0;
    string[] dayNames = { "Night", "Morning", "Day", "Evening" };

    [Header("Power")]
    [SerializeField] AnimationCurve powerDemand;
    float powerOffset = 0f;
    float powerIn;
    float powerOut;
    bool powerWarn;
    float gasPrice = 1f;
    float carbonTax = 0f;

    [Header("Events")]
    [SerializeField] List<GameEvent> events;
    int eventIndex = -1;

    [Header("Mood")]
    [SerializeField] int moodSteps = 20;
    int mood;
    int moodDelta;
    float price;
    string[] moodNames = { "<<<", "<<", "<", "", ">" };

    [Header("UI")]
    [SerializeField] Image eventClock;
    [SerializeField] TextMeshProUGUI eventDescription;
    [SerializeField] Image dayClock;
    [SerializeField] TextMeshProUGUI dayDescription;
    [SerializeField] Image powerInput;
    [SerializeField] Image powerOutput;
    [SerializeField] GameObject powerWarning;
    [SerializeField] Image priceMeter;
    [SerializeField] Image moodMeter;
    [SerializeField] TextMeshProUGUI moodTrend;


    public static GameState Instance { get; protected set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }


    void Start()
    {
        mood = moodSteps;
        eventTimer = 0f;
        dayTime = 0f;
        dayIndex = -1;
        eventIndex = -1;
        eventDescription.text = "";
        powerOffset = 0f;
        gasPrice = 1f;
        carbonTax = 0f;
        CalculateStats();
    }

    void Update()
    {
        dayTime += Time.deltaTime;
        if (dayTime > dayLength)
        {
            dayTime -= dayLength;
        }
        eventTimer += Time.deltaTime;
        if (eventTimer > eventInterval)
        {
            eventTimer -= eventInterval;
            NextEvent();
            AdjustMood();
        }
        CalculateStats();
    }

    void CalculateStats()
    {
        powerOut = Mathf.Min(powerDemand.Evaluate(dayTime / dayLength) + powerOffset, 1.0f); ;
        powerOutput.fillAmount = powerOut;
        powerIn = powerOut; // TODO calculate power in
        powerInput.fillAmount = powerIn;
        powerWarn = powerIn < powerOut - 0.01f;
        if (powerWarn != powerWarning.activeSelf)
            powerWarning.SetActive(powerWarn);
        price = 0.5f; // TODO calculate price
        priceMeter.fillAmount = price;
        int mood = 0;
        if (powerWarn)
            mood -= 2;
        if (price < 0.25f)
            mood++;
        else if (price > 0.6f)
            mood--;
        if (mood != moodDelta)
        {
            moodDelta = mood;
            moodTrend.text = moodNames[mood + 3];
        }
        float dayFrac = dayTime / dayLength;
        dayClock.fillAmount = dayFrac;
        int dayId = Mathf.FloorToInt(dayFrac * (float)dayNames.Length);
        if (dayId != dayIndex)
        {
            dayDescription.text = dayNames[dayId];
            dayIndex = dayId;
        }
        eventClock.fillAmount = eventTimer / eventInterval;
    }

    void NextEvent()
    {
        eventIndex++;
        if (eventIndex < events.Count)
        {
            eventDescription.text = events[eventIndex].description;
            // TODO EVent
        }
        else
        {
            // TODO select a random event
            eventDescription.text = string.Format("Event {}", eventIndex);
        }
    }

    void AdjustMood()
    {
        mood += moodDelta;
        if (mood <= 0)
        {
            //TODO game over
            Debug.LogWarning("Game Over!");
            moodMeter.fillAmount = 0f;
        }
        else
        {
            moodMeter.fillAmount = (float)mood / (float)moodSteps;
        }
    }
}
