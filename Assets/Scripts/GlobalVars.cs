using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVars : MonoBehaviour
{
    public static float Timer => _Timer;
    internal static float _Timer;
    internal static float maxtime;
    public static bool UseFloor = true;
    public static bool isPlay = true;
    public Slider slider;

    public void ToggleFloor(bool tg) {
        UseFloor = tg;
    }
    public void Toggleplay(bool t)
    {
       isPlay = t;
    }
    public void SetTime(Single t)
    {
        _Timer = (float)t;
    }
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        
    }
    void Update()
    {
        slider.maxValue = maxtime * 2f;
        if (isPlay)
        {
            _Timer += Time.deltaTime;
            _Timer %= maxtime * 2f;
            slider.value = _Timer;
        }
    }
}
