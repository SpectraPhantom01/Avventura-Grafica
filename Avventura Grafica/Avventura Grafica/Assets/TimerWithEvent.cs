using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerWithEvent : MonoBehaviour
{
    [Header("Setta il tempo del timer")]
    [SerializeField] int time;
    [Header("Deve partire dall'AWAKE?")]
    [SerializeField] bool playOnStart;

    public UnityEvent onEndTime;

    float timer;
    bool timerStarted = false;
    public int BeginningTime => time;
    private void Awake()
    {

        if (playOnStart)
            PlayTimer();
    }

    public void PlayTimer()
    {
        timer = time;
        timerStarted = true;
    }

    private void Update()
    {
        if (timerStarted)
        {
            timer -= 1 * Time.deltaTime;

            if (timer <= 0)
            {
                timerStarted = false;
                onEndTime.Invoke();
            }
        }
    }

    public void SetNewTime(int newTimer)
    {
        time = newTimer;
    }

    public void StopTimer()
    {
        timerStarted = false;
        timer = 0;
    }

    public int GetTimerValue()
    {

        return (int)timer;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
