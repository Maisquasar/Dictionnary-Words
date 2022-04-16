using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Globalization;
using System.Text;

public class GameManager : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] Text CurrentWordText;
    [SerializeField] Text CurrentLetterText;
    [SerializeField] Text WordCountText;
    [Header("Inputs")]
    [SerializeField] InputField InputField;
    [SerializeField] InputField GoToInput;

    private string currentWord;
    private int currentId = -1;

    List<Text> OtherWordsTexts = new List<Text>();
    List<Text> OtherLettersTexts = new List<Text>();
    // Start is called before the first frame update
    void Start()
    {
        AddLetter(-1f);
        AddLetter(-2f);

        AddText(new Vector2(-5f, 1f), 0.75f);
        AddText(new Vector2(-2.5f, 0.5f));
        AddText(new Vector2(2.5f, 0.5f));
        AddText(new Vector2(5f, 1f), 0.75f);

        StartCoroutine(WaitForStart());
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitUntil(() => WordsDatas.isInitialized);
        NewWord();
    }

    private void Update()
    {
        WordCountText.text = $"{currentId}/{WordsDatas.wordsDatas.Count}";
    }

    public void NewWord()
    {
        SetText(currentId + 1);
    }

    public void GotoUrl()
    {
        string Url = $"https://www.google.com/search?q={currentWord}";
        Application.OpenURL(Url);
    }


    public void Check()
    {
        double coef = DiceCoefficient(InputField.text, currentWord);
        if (coef == 1)
        {
            StartCoroutine(ChangeColor(true, InputField));
            NewWord();
            InputField.text = "";
        }
        if (InputField.text.Length > 0)
        {
            if (InputField.text.Last() == ' ' && coef > 0.6)
                StartCoroutine(ChangeColor(false, InputField));
        }
    }

    public static double DiceCoefficient(string strA, string strB)
    {
        strA = RemoveDiacritics(strA.ToLower());
        strB = RemoveDiacritics(strB.ToLower());
        HashSet<string> setA = new HashSet<string>();
        HashSet<string> setB = new HashSet<string>();

        for (int i = 0; i < strA.Length - 1; ++i)
            setA.Add(strA.Substring(i, 2));

        for (int i = 0; i < strB.Length - 1; ++i)
            setB.Add(strB.Substring(i, 2));

        HashSet<string> intersection = new HashSet<string>(setA);
        intersection.IntersectWith(setB);

        return (2.0 * intersection.Count) / (setA.Count + setB.Count);
    }


    static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC);
    }


    public void GoTo()
    {
        string search = GoToInput.text;
        var tmp = WordsDatas.GetIdByWord(search);
        if (tmp != -1)
        {
            StartCoroutine(ChangeColor(true, GoToInput));
            SetText(tmp);
        }
        else
        {
            StartCoroutine(ChangeColor(false, GoToInput));
        }
    }

    bool endOfCoroutine = true;
    IEnumerator ChangeColor(bool flag, InputField input)
    {
        if (!endOfCoroutine)
            yield break;
        endOfCoroutine = false;
        ColorBlock defaultColor = input.colors;
        ColorBlock tmp = input.colors;
        if (flag)
        {
            tmp.selectedColor = Color.green;
            input.colors = tmp;
        }
        else
        {
            tmp.selectedColor = Color.red;
            input.colors = tmp;
        }
        yield return new WaitForSeconds(0.5f);
        input.colors = defaultColor;
        endOfCoroutine = true;
    }

    public void GetLastWord()
    {
        currentWord = WordsDatas.GetLastWord(Application.streamingAssetsPath + "/Datas/LastWord.txt");
        int tmp = WordsDatas.GetIdByWord(currentWord);
        if (tmp != -1)
            SetText(tmp);
    }

    public void SaveLastWord()
    {
        WordsDatas.WriteLastWordFile(currentWord, Application.streamingAssetsPath + "/Datas/LastWord.txt");
    }

    #region Other

    void AddText(Vector2 offset, float alpha = 0.5f, TextAnchor anchor = TextAnchor.MiddleCenter)
    {
        OtherWordsTexts.Add(Instantiate(CurrentWordText, CurrentWordText.transform.parent));
        OtherWordsTexts.Last().transform.parent = CurrentWordText.gameObject.transform.parent;
        OtherWordsTexts.Last().transform.position = (Vector2)CurrentWordText.gameObject.transform.position + offset;
        OtherWordsTexts.Last().transform.position = new Vector3(OtherWordsTexts.Last().transform.position.x, OtherWordsTexts.Last().transform.position.y, 0); 
        OtherWordsTexts.Last().color = CurrentWordText.color - new Color(0, 0, 0, alpha);
    }

    void AddLetter(float y)
    {
        OtherLettersTexts.Add(Instantiate(CurrentLetterText, CurrentLetterText.transform));
        OtherLettersTexts.Last().transform.parent = CurrentLetterText.gameObject.transform.parent;
        OtherLettersTexts.Last().transform.position = (Vector2)CurrentLetterText.gameObject.transform.position + Vector2.up * y;
    }

    void SetText(int id)
    {
        currentId = id;
        currentWord = WordsDatas.wordsDatas[currentId];
        CurrentWordText.text = currentWord;
        for (int i = 0; i < OtherWordsTexts.Count; i++)
        {
            SetOtherTexts(i, -2 + i);
        }
        for (int i = 0; i < OtherLettersTexts.Count; i++)
        {
            SetOtherLetters(i);
        }
    }

    void SetOtherTexts(int id, int position)
    {
        if (position < 0)
        {
            if (currentId > (position * -1) - 1)
                OtherWordsTexts[id].text = WordsDatas.wordsDatas[currentId + position];
            else
                OtherWordsTexts[id].text = "";
        }
        else
        {
            if (currentId < WordsDatas.wordsDatas.Count + position - 2)
                OtherWordsTexts[id].text = WordsDatas.wordsDatas[currentId + position + 1];
            else
                OtherWordsTexts[id].text = "";
        }
    }

    void SetOtherLetters(int id)
    {
        char v = (char)(WordsDatas.wordsDatas[currentId].ToCharArray()[0]);
        CurrentLetterText.text = $"{v}";
        if (WordsDatas.wordsDatas[currentId].ToCharArray()[0] + id + 1 > 90)
        {
            foreach (var other in OtherLettersTexts)
                other.text = "";
        }
        else
        {
            v = (char)(WordsDatas.wordsDatas[currentId].ToCharArray()[0] + id + 1);
            OtherLettersTexts[id].text = $"{v}";
        }
    }
    #endregion
}

