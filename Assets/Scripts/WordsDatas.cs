using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;

public class WordsDatas : MonoBehaviour
{
    public static bool isInitialized = false;
    public static List<string> wordsDatas = new List<string>();
    [SerializeField] TextReader file;

    private void Start()
    {
        Debug.Log(Application.streamingAssetsPath);
        readTextFile(Application.streamingAssetsPath + "/Datas/WordList2.txt");
        isInitialized = true;
    }

    public static void WriteLastTimeFile(int[] LastTime, string file_path)
    {
        StreamWriter inp_stm = File.CreateText(file_path);
        foreach (var line in LastTime)
            inp_stm.WriteLine(line);
        inp_stm.Close();
    }

    public static int[] GetLastTime(string file_path)
    {
        int[] tmp = new int[3];
        StreamReader inp_stm = new StreamReader(file_path, Encoding.GetEncoding("iso-8859-1"));
        if (inp_stm == null)
            Debug.LogError($"File {file_path} not found !");
        for (int i = 0; i < 3; i++)
            tmp[i] = int.Parse(inp_stm.ReadLine());
        inp_stm.Close();
        return tmp;
    }

    public static void WriteLastWordFile(string LastWord, string file_path)
    {
        StreamWriter inp_stm = File.CreateText(file_path);
        inp_stm.WriteLine(LastWord);
        inp_stm.Close();
    }

    public static string GetLastWord(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path, Encoding.GetEncoding("iso-8859-1"));
        if (inp_stm == null)
            Debug.LogError($"File {file_path} not found !");
        string word = inp_stm.ReadLine();
        inp_stm.Close();
        return word;
    }

    void readTextFile(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path, Encoding.GetEncoding("iso-8859-1"));
        if (inp_stm == null)
            Debug.LogError($"File {file_path} not found !");
        while (!inp_stm.EndOfStream)
        {
            string word = inp_stm.ReadLine();
            word += " ";
            word = char.ToUpper(word[0]) + word.Substring(1).ToLower();
            wordsDatas.Add(word);
        }
        Debug.Log($"All {wordsDatas.Count} words Loaded !");
        inp_stm.Close();
        
    }

    public static int GetIdByWord(string Word)
    {
        if (Word.Last() != ' ')
            Word += " ";
        for (int i = 0; i < wordsDatas.Count; i++)
        {
            if (string.Equals(wordsDatas[i], Word, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        Debug.LogError($"Word {Word} not found");
        return -1;
    }

    public static string GetRandomWordBySize(int Size)
    {
        var tmp = new List<string>();
        for (int i = 0; i < wordsDatas.Count; i++)
        {
            if (wordsDatas[i].Length == Size)
                tmp.Add(wordsDatas[i]);
        }
        int index = UnityEngine.Random.Range(0, tmp.Count);
        if (tmp[index] != null)
            return tmp[index];
        Debug.LogError($"Word with Size {Size} not found");
        return null;
    }
}
