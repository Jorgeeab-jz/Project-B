using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    // Nombre de la escena de juego (asegúrate de configurarlo en Build Settings)
    public string gameSceneName = "GameScene";

    // Referencia al panel de opciones (opcional)
    public GameObject optionsPanel;

    private void Start()
    {
        // Asegúrate de que el panel de opciones esté oculto al inicio
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    // Método para iniciar el juego
    public void StartGame()
    {
        SoundFXManager.instance.PlaySoundFXClip(audioClips[0], transform, 1);
        // Cambia a la escena de juego
        SceneManager.LoadScene(gameSceneName);
    }

    // Método para abrir/cerrar el panel de opciones
    public void ToggleOptionsPanel()
    {
        if (optionsPanel != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(audioClips[0], transform, 1);
            // Alterna la visibilidad del panel de opciones
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("Panel de opciones no asignado en el Inspector");
        }
    }

    // Método para salir del juego
    public void QuitGame()
    {
#if UNITY_EDITOR
        SoundFXManager.instance.PlaySoundFXClip(audioClips[0], transform, 1);
        // Si estás en el editor de Unity
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SoundFXManager.instance.PlaySoundFXClip(audioClips[0], transform, 1);
        // Si estás en una compilación construida
        Application.Quit();
#endif
    }
}
