using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    Text timer;
    [SerializeField] int _hours;
    [SerializeField] int _minutes;
    [SerializeField] float _seconds;

    Vector3 startPos;
    Vector3 Goto;

    [SerializeField] GameObject HUD;

    public int Hours { get { return _hours; } }
    public int Minutes { get { return _minutes; } }
    public int Seconds { get { return (int)_seconds; } }

    [SerializeField] UnityEvent NewMinute;

    private void Start()
    {
        timer = GetComponent<Text>();
        timer.text = string.Format($"{_hours:D2}:{_minutes:D2}:{(int)_seconds:D2}");
        startPos = transform.position;
        Goto = new Vector3(-7f, 4.3f, 90.0f);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Home))
        {
            if (HUD.activeSelf)
            {
                HUD.SetActive(false);
                transform.position = Goto;
            }
            else
            {
                HUD.SetActive(true);
                transform.position = startPos;
            }
        }
        timer.text = string.Format($"{_hours:D2}:{_minutes:D2}:{(int)_seconds:D2}");
        if (lastInputTime <= Time.time || _pause)
            return;
        SetTime();
    }

    #region Add function

    public void AddHour(int time)
    {
        if (_hours + time >= 0)
            _hours += time;
    }

    public void AddMinutes(int time)
    {
        if (_minutes + time <= 60 && _minutes + time >= 0)
            _minutes += time;
    }

    public void AddSeconds(int time)
    {
        if (_seconds + time <= 60 && _seconds + time >= 0)
            _seconds += time;
    }
    #endregion

    public void SetLastTime()
    {
        int[] tmp = { _hours, _minutes, (int)_seconds };
        WordsDatas.WriteLastTimeFile(tmp, Application.streamingAssetsPath + "/Datas/LastTime.txt");
    }

    public void GetLastTime()
    {
        int[] tmp = WordsDatas.GetLastTime(Application.streamingAssetsPath + "/Datas/LastTime.txt");
        _hours = tmp[0];
        _minutes = tmp[1];
        _seconds = tmp[2];
    }

    bool _pause;
    public void PauseTime()
    {
        _pause = !_pause;
    }

    float lastInputTime;
    public void LastInput()
    {
        lastInputTime = Time.time + 3;
    }

    void SetTime()
    {
        _seconds += (float)Time.deltaTime;
        if (_seconds >= 60)
        {
            _seconds = 0;
            _minutes += 1;
            NewMinute.Invoke();
        }
        if (_minutes >= 60)
        {
            _hours += 1;
            _minutes = 0;
        }
    }
}
