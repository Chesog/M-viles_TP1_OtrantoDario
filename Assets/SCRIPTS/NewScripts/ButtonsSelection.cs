using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsSelection : MonoBehaviour
{
    public event Action<int> OnDifficultySelection;

    public void SetEasy()
    {
        OnDifficultySelection.Invoke(0); 
    }
    public void SetNormal()
    {
        OnDifficultySelection.Invoke(1); 
    }
    public void SetHard()
    {
        OnDifficultySelection.Invoke(2); 
    }
}
