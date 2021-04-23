using UnityEngine;
using UnityEngine.SceneManagement;

public class RPGGameManager : MonoBehaviour
{
    public static RPGGameManager instanciaCompartilhada = null; // instância compartilhada do Singleton

    public RPGCameraManager cameraManager; // referência da componente RPGCameraManager

    public PontoSpawn playerPontoSpawn; // referência do Ponto de Spawn do Player

    public bool isGameOver; // indica se o player foi derrotado e a fase deve serfinalizada

    private void Awake()
    {
        if (instanciaCompartilhada != null && instanciaCompartilhada != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instanciaCompartilhada = this;
        }
    }

    private void Start()
    {
        SetupScene();
    }

    private void Update()
    {
        if (isGameOver)
            LoadGameOver();
    }

    /// <summary>
    /// Aplica as configurações iniciais do jogo
    /// </summary>
    public void SetupScene()
    {
        isGameOver = false;
        SpawnPlayer();
    }

    /// <summary>
    /// Realiza o spawn do Player e direciona a câmera virtual a ele
    /// </summary>
    public void SpawnPlayer()
    {
        if (playerPontoSpawn != null)
        {
            GameObject player = playerPontoSpawn.SpawnO();
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }

    /// <summary>
    /// Carrega a cena de Game Over
    /// </summary>
    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
