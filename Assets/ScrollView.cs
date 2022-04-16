using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScrollView : MonoBehaviour
{
    GameManager manager;
    [SerializeField] Button DefaultButton;
    [SerializeField] InputField Inputfield;
    List<Button> btns;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InputSearch()
    {
        SetButton(Inputfield.text);
    }

    public void SetButton(string search)
    {
        foreach (var word in WordsDatas.wordsDatas)
        {
            if (word.Substring(0, search.Length) == search)
            {
                btns.Add(Instantiate(DefaultButton, gameObject.transform));
                btns.Last().GetComponent<Text>().text = word;
            }
        }
    }
}
