using UnityEngine;

public class PontoSpawn : MonoBehaviour
{
    public GameObject prefabParaSpawn; // referência do prefab utilizado no Ponto de Spawn

    public float intervaloRepeticao; // intervalo em segundos de quanto tempo irá levar para spawnar novo objeto

    public bool isPlayerSpawn; // indica se é um ponto de Spawn de Player

    void Start()
    {
        if (intervaloRepeticao > 0)
        {
            InvokeRepeating("SpawnO", 0.0f, intervaloRepeticao);
        }
    }

    /// <summary>
    /// Realiza o spawn do objeto, isto é, cria ele no jogo
    /// </summary>
    /// <returns>Referência para o objeto criado</returns>
    public GameObject SpawnO()
    {
        if (isPlayerSpawn)
        {
            GameObject player = GameObject.Find("PlayerO(Clone)");
            if (player != null)
            {
                player.transform.position = transform.position;
                return player;
            }
        }

        if (prefabParaSpawn != null)
        {
            return Instantiate(prefabParaSpawn, transform.position, Quaternion.identity);
        }

        return null;
    }
}
