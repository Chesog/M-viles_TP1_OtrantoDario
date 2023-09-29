using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DificultyObserver : MonoBehaviour
{
    [SerializeField]private ButtonsSelection _selection;
    // Start is called before the first frame update
    void Start()
    {
        _selection.OnDifficultySelection += OnDificultySelection;
    }

    private void OnDificultySelection(int value)
    {
        GameManager.Instancia.SetDifficulty(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
