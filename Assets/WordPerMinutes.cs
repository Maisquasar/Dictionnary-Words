using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WordPerMinutes : MonoBehaviour
{
    [SerializeField] InputField Input;
    [SerializeField] GameObject Graph;
    Text text;
    Graphic _graphic;

    int CharacterCount;
    int WordPerMinute;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        _graphic = Graph.GetComponent<Graphic>();
    }

    public void NewMinute()
    {
        WordPerMinute = CharacterCount / 5;
        text.text = $"WPM : {WordPerMinute}";
        CharacterCount = 0;
        _graphic.CreateNewPoint(WordPerMinute);
    }

    public void NewCharacter()
    {
        CharacterCount += 1;
    }
}
