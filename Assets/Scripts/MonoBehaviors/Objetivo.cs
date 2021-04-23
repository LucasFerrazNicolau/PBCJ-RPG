using UnityEngine;
using UnityEngine.SceneManagement;

public class Objetivo : MonoBehaviour
{
    public string nextScene; // insica a cena para qual o objetivo irá enviar após ser coletado

    /// <summary>
    /// Carrega a próxima cena
    /// </summary>
    public void IrParaProximaCena()
    {
        SceneManager.LoadScene(nextScene);
    }
}
