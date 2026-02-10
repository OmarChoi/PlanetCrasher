using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class WebGetSutdentCSVTest : MonoBehaviour
{
    private async void Start()
    {
        string result = await GetWebText("https://raw.githubusercontent.com/mongilteacher/skku2_script_study/refs/heads/main/students.csv");
        Debug.Log(result);

        List<Person> persons = new List<Person>();
    }

    private async UniTask<string> GetWebText(string url)
    {
        var txt = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return txt;
    }
}

public class Person
{
    public string Name { get; private set; }
    public int Age { get; private set; }
}