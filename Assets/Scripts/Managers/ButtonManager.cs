using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    /// <summary>
    /// Chama a cena da primeira fase do jogo
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("RPG-Fase1");
    }

    /// <summary>
    /// Chama a cena de Créditos
    /// </summary>
    public void ShowCredits()
    {
        SceneManager.LoadScene("Creditos");
    }

    /// <summary>
    /// Chama a cena do menu principal
    /// </summary>
    public void ShowMainMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
