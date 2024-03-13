using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;


public class handleValue : MonoBehaviour
{
    
    // conexion con Firebase
    private FirebaseApp _app;
    // Singleton de la Base de Datos
    private FirebaseDatabase _db;
    // referencia a la 'coleccion' Jugadores
    private DatabaseReference _refJugadores;
    // referencia a un Logro en concreto
    private DatabaseReference _refStatMuertes;
  
  
    
  
    
    // Start is called before the first frame update
    void Start()
    {
             
       
        
        // realizamos la conexion a Firebase
        _app = Conexion();
        
        // obtenemos el Singleton de la base de datos
        _db = FirebaseDatabase.DefaultInstance;
        
        // Obtenemos la referencia a TODA la base de datos
        // DatabaseReference reference = db.RootReference;
        
        // Definimos la referencia a Clientes
        _refJugadores = _db.GetReference("Jugadores");
        
        // Definimos la referencia a AA02
        _refStatMuertes = _db.GetReference("Jugadores/Stats/muertes");
        
        // Recogemos todos los valores de Clientes
        _refJugadores.Child("Stats").GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    // Handle the error...
                }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    // mostramos los datos
                    RecorreResultado(snapshot);
                    //Debug.Log(snapshot.value);
                }
            });
        
        // Añadimos el evento cambia un valor
         _refStatMuertes.ValueChanged += HandleValueChanged;

        // Añadimos un nodo
        AltaDevice();
    }
    
    // realizamos la conexion a Firebase
    // devolvemos una instancia de esta aplicacion
    FirebaseApp Conexion()
    {
        FirebaseApp firebaseApp = null;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                firebaseApp = FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                firebaseApp = null;
            }
        });
            
        return firebaseApp;
    }
    
  
    void HandleValueChanged(object sender, ValueChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Mostramos lo resultados
        MuestroJugador(args.Snapshot);
        // escalo objeto
       int muertes = int.Parse(args.Snapshot.Value.ToString());
        Debug.Log("muertesD "+muertes);
    }

    // recorro un snapshot de un nivel
    void RecorreResultado(DataSnapshot snapshot)
    {
        foreach(var resultado in snapshot.Children) // Clientes
        {
            Debug.LogFormat("Key = {0}", resultado.Key);  // "Key = AAxx"
            foreach(var levels in resultado.Children)
            {
                Debug.LogFormat("(key){0}:(value){1}", levels.Key, levels.Value);
            }
        }
    }
    
    // muestro un jugador
    void MuestroJugador(DataSnapshot jugador)
    {
        foreach (var resultado in jugador.Children) // jugador
        {
            Debug.LogFormat("{0}:{1}", resultado.Key, resultado.Value);
        }
    }


    // doy de alta un nodo con un identificador unico
    void AltaDevice()
    {
        _refJugadores.Child(SystemInfo.deviceUniqueIdentifier).Child("nombre").SetValueAsync("Mi dispositivo");
    }
    
    // Update is called once per frame
    void Update()
    {
      
    }
}