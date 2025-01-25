using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private void Awake()
    {
        // Si ya existe una instancia, destruye este objeto
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Establece la instancia y evita la destrucción al cambiar de escena
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
