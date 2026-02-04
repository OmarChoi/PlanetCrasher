using System;
using Firebase.Firestore;

[FirestoreData]
public class DogSaveData
{
    [FirestoreDocumentId]
    public string Id { get; set; }
    
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public int Age { get; set; }

    public DogSaveData() { }
    
    public DogSaveData(string name, int age)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("name cannot be null or empty");
        if (age <= 0) throw new ArgumentException("age cannot less or equal to 0");
        Name = name;
        Age = age;
    }
}
