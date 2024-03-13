using System;
using System.Collections;
using System.Collections.Generic;
using EasyUI.Toast;
using Firebase.Database;
using UnityEngine;

public class AchievementsList : MonoBehaviour

{

    public FirebaseDatabase _db;
    public DatabaseReference _refJugadores;
    bool isQuerying = false;
 

    void Start()
    {
        _db = FirebaseDatabase.DefaultInstance;
        _refJugadores = _db.GetReference("Jugadores");
    }


    Dictionary<string, Tuple<int, string>> achievements = new Dictionary<string, Tuple<int, string>>
    {
        {"FirstLose", new Tuple<int, string>(1, "derrotas")},
        {"FirstWin", new Tuple<int, string>(1, "victorias")},
        {"Carnage", new Tuple<int, string>(30, "asesinatos")},
        {"ClumsyRecruit", new Tuple<int, string>(30, "muertes")},
        {"FirstDie", new Tuple<int, string>(1, "muertes")},
        {"FirstKill", new Tuple<int, string>(1, "asesinatos")},
        {"FirstFall", new Tuple<int, string>(1, "caidas")},
        {"RepeatMeThatLittleNumber", new Tuple<int, string>(33, "partidas")}
    };

    Dictionary<string, bool> doneQuerying = new Dictionary<string, bool>
    {
        {"FirstLose", false},
        {"FirstWin", false},
        {"Carnage", false},
        {"ClumsyRecruit", false},
        {"FirstDie", false},
        {"FirstKill", false},
        {"FirstFall", false},
        {"RepeatMeThatLittleNumber", false}
    };

    void Update()
    {
        foreach (var achievement in achievements.Keys)
        {
            if (!isQuerying && !doneQuerying[achievement])
            {
                isQuerying = true;
                StartCoroutine(QueryDatabaseStats(achievements[achievement].Item2, stat =>
                {
                    StartCoroutine(QueryDatabaseAchievementCompleted(achievement, completed =>
                    {
                        if (stat == achievements[achievement].Item1 && !completed)
                        {
                            UnlockAchievement(achievement);
                            doneQuerying[achievement] = true;
                            
                        }
                    }));
                }));
            }
            isQuerying = false;
        }
    }



    void QueryAndUpdateAchievements(string key)
    {
        _refJugadores.Child("Achievements").Child(key).Child("Completed").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                bool completed = bool.Parse(snapshot.Value.ToString());
                _refJugadores.Child("Achievements").Child(key).Child("Completed").SetValueAsync(true);
            }
        });
    }

    public void UnlockAchievement(string key)
    {
        QueryAndUpdateAchievements(key);
        if (PlayerPrefs.GetString("Language") == "ENGLISH" || PlayerPrefs.GetString("Language") == "Inglés")
        {
            StartCoroutine(QueryDatabaseAchievementName(key, "Name", name =>
            {
                Debug.Log("nombre" + name);
                Toast.Show(" <color=white> Achievement Unlocked:  " + name + "</color>", 2f,
                    new Color(0.3301887f, 0.3283197f, 0.3283197f, 1));
            }));
        }
        else
        {
            StartCoroutine(QueryDatabaseAchievementName(key, "Nombre", nombre =>
            {
                Toast.Show(" <color=white> Logro desbloqueado:  " + nombre + "</color>", 2f,
                    new Color(0.3301887f, 0.3283197f, 0.3283197f, 1));
            }));
        }
    }

    IEnumerator QueryDatabaseAchievementName(string achievement, string name, Action<string> callback)
    {
        string nombre = "";
        var task = _refJugadores.Child("Achievements").Child(achievement).Child(name).GetValueAsync();
        {

            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted)
            {
                Debug.LogError("Error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                nombre = (snapshot.Value.ToString());


            }

            callback(nombre);

        }
    }

    IEnumerator QueryDatabaseAchievementCompleted(string achievement, Action<bool> callback)
    {

        var task = _refJugadores.Child("Achievements").Child(achievement).Child("Completed").GetValueAsync();
        {
            bool completed = false;
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.IsFaulted)
            {
                Debug.LogError("Error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result; 
                completed = bool.Parse(snapshot.Value.ToString());


            }

            callback(completed);

        }


    }

    IEnumerator QueryDatabaseStats(string key, Action<int> callback)
    {
        int stat = 0;
        var task = _refJugadores.Child("Stats").Child(key).GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Error");
        }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
            stat = int.Parse(snapshot.Value.ToString());
        }

        callback(stat);
    }
}
        

        
        


    











