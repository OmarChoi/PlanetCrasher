using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class WebGetSutdentCSVTest : MonoBehaviour
{
    private List<Person> _persons = new List<Person>();
    private async void Start()
    {
        string result = await GetWebText("https://raw.githubusercontent.com/mongilteacher/skku2_script_study/refs/heads/main/students.csv");
        result = result.TrimStart('\uFEFF');

        var config = new CsvConfiguration(CultureInfo.CurrentCulture);
        var stringReader = new StringReader(result);
        var csv = new CsvReader(stringReader, config);

        _persons = csv.GetRecords<Person>().ToList();

        foreach (Person p in _persons)
        {
            Debug.Log(p);
        }
    }
    
    private async UniTask<string> GetWebText(string url)
    {
        var txt = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return txt;
    }
}

public class Person
{
    [Name("id")]
    public int Id { get; set;  }
    
    [Name("name")]
    public string Name { get; set;}
    
    
    [Name("age")]
    public int Age { get; set; }

    public Person()
    {
        
    }
    
    public Person(int id, string name, int age)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Age = age;
    }

    public override string ToString()
    {
        return $"Person(Id={Id}, Name={Name}, Age={Age})";
    }
}