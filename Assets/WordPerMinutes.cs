using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WordPerMinutes : MonoBehaviour
{
    [SerializeField] InputField Input;
    Text text;

    int CharacterCount;
    float WordPerMinute;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NewMinute()
    {
        WordPerMinute = CharacterCount / 5;
        text.text = $"WPM : {WordPerMinute}";
        CharacterCount = 0;
    }

    public void NewCharacter()
    {
        CharacterCount += 1;
    }
}
