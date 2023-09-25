using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    private float giro;
    private Dictionary<string, float> axisValues = new Dictionary<string, float>();
    private Dictionary<string, bool> buttonValues = new Dictionary<string, bool>();
    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                instance = new InputManager();

            return instance;
        }
    }

    public void SetAxis(string inputName,float value)
    {
        if (!axisValues.ContainsKey(inputName))
            axisValues.Add(inputName,value);

        axisValues[inputName] = value;
    }

    public float GetOrAddAxis(string inputName)
    {
        if (!axisValues.ContainsKey(inputName))
            axisValues.Add(inputName,0f);
        return axisValues[inputName];
    }

    public float GetAxis(string inputName)
    {
#if UNITY_EDITOR
        return GetOrAddAxis(inputName) + Input.GetAxis(inputName);
#endif
#if UNITY_ANDROID || UNITY_IOS
        return GetOrAddAxis(inputName);
#endif
#if UNITY_STANDALONE
        Input.GetAxis(inputName);
#endif
    }
    
    public void SetButtons(string inputName,bool value)
    {
        if (!buttonValues.ContainsKey(inputName))
            buttonValues.Add(inputName,value);

        buttonValues[inputName] = value;
    }

    public bool GetOrAddButtons(string inputName)
    {
        if (!buttonValues.ContainsKey(inputName))
            buttonValues.Add(inputName,false);
        return buttonValues[inputName];
    }

    public bool GetButtons(string inputName)
    {
#if UNITY_EDITOR
        return GetOrAddButtons(inputName);
#endif
#if UNITY_ANDROID || UNITY_IOS
        return GetOrAddButtons(inputName);
#endif
#if UNITY_STANDALONE
        return GetButtons(inputName);
#endif
    }
}
