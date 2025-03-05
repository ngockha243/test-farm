using System;
using System.Collections;
using UnityEngine;
using Utilities;

public class TimerController : MonoSingleton<TimerController>
{
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    private Action secondChanged;
    private void Start()
    {
        StartCoroutine(SecondChange());
    }

    private IEnumerator SecondChange()
    {
        while (true)
        {
            yield return waitForSeconds;
            secondChanged?.Invoke();
        }
    }

    public void AddAction(Action action)
    {
        secondChanged += action;
    }

    public void RemoveAction(Action action)
    {
        secondChanged -= action;
    }
}
