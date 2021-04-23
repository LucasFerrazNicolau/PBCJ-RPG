using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PontosDano pontosDano; // objeto de leitura de dados de quantos pontos tem o Player
    public Player caractere; // receberá o objeto do Player
    public Image medidorImagem; // recebe a barra de medição
    public Text pdTexto; // recebe os dados de PD
    float maxPontosDano; // armazena a quantidade limite de "saúde" do Player

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        maxPontosDano = caractere.MaxPontosDano;
    }

    void Update()
    {
        if (caractere != null)
        {
            medidorImagem.fillAmount = pontosDano.valor / maxPontosDano;
            pdTexto.text = "PD: " + (medidorImagem.fillAmount * 100);
        }
    }
}
