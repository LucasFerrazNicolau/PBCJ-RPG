using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Arma : MonoBehaviour
{
    public GameObject municaoPrefab; // armazena o prefab da Munição
    static List<GameObject> municaoPiscina; // Pool de Munição
    public int tamanhoPiscina; // Tamanho do Pool
    public float velocidadeArma; // Velocidade da Munição

    bool atirando; // verifica se está atirando para a animação
    [HideInInspector]
    public Animator animator; // armazena componente do Animator

    Camera cameraLocal; // referência para a câmera principal

    float slopePositivo; // slope positivo em relação ao Player
    float slopeNegativo; // slope negativo em relação ao Player

    enum Quadrante
    {
        Leste,
        Sul,
        Oeste,
        Norte
    }

    private void Awake()
    {
        if (municaoPiscina == null)
        {
            municaoPiscina = new List<GameObject>();
        }
        for (int i = 0; i < tamanhoPiscina; i++)
        {
            GameObject municaoO = Instantiate(municaoPrefab);
            municaoO.SetActive(false);
            municaoPiscina.Add(municaoO);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        atirando = false;
        cameraLocal = Camera.main;
        Vector2 abaixoEsquerda = cameraLocal.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 acimaDireita = cameraLocal.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 abaixoDireita = cameraLocal.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        Vector2 acimaEsquerda = cameraLocal.ScreenToWorldPoint(new Vector2(0, Screen.height));
        slopePositivo = PegarSlope(abaixoEsquerda, acimaDireita);
        slopeNegativo = PegarSlope(acimaEsquerda, abaixoDireita);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            atirando = true;
            DispararMunicao();
        }
        UpdateEstado();
    }

    private void OnDestroy()
    {
        municaoPiscina = null;
    }

    /// <summary>
    /// Realiza o spawn da munição puxando uma munição livre do pool e a ativando
    /// </summary>
    /// <param name="posicao">Posição onde a munição será criada</param>
    /// <returns>Referência para a munição criada</returns>
    public GameObject SpawnMunicao(Vector3 posicao)
    {
        foreach (GameObject municao in municaoPiscina)
        {
            if (municao.activeSelf == false)
            {
                municao.SetActive(true);
                municao.transform.position = posicao;
                return municao;
            }
        }
        return null;
    }

    /// <summary>
    /// Cria uma munição e à atira em uma direção
    /// </summary>
    void DispararMunicao()
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject municao = SpawnMunicao(transform.position);
        if (municao != null)
        {
            Arco arcoScript = municao.GetComponent<Arco>();
            float duracaoTrajetoria = 1.0f / velocidadeArma;
            StartCoroutine(arcoScript.ArcoTrajetoria(posicaoMouse, duracaoTrajetoria));
        }
    }

    /// <summary>
    /// Obtém o slope entre dois pontos
    /// </summary>
    /// <param name="ponto1">Primeiro ponto</param>
    /// <param name="ponto2">Segundo ponto</param>
    /// <returns>Slope relativo aos pontos</returns>
    float PegarSlope(Vector2 ponto1, Vector2 ponto2)
    {
        return (ponto2.y - ponto1.y) / (ponto2.x - ponto1.x);
    }

    /// <summary>
    /// Verifica se uma posição está acima do slope positivo
    /// </summary>
    /// <param name="posicaoEntrada">Posição de referência</param>
    /// <returns>Se está acima do slope positivo</returns>
    bool AcimaSlopePositivo(Vector2 posicaoEntrada)
    {
        Vector2 posicaoPlayer = gameObject.transform.position;
        cameraLocal = Camera.main;
        Vector2 posicaoMouse = cameraLocal.ScreenToWorldPoint(posicaoEntrada);
        float interseccaoY = posicaoPlayer.y - (slopePositivo * posicaoPlayer.x);
        float entradaInterseccao = posicaoMouse.y - (slopePositivo * posicaoMouse.x);
        return entradaInterseccao > interseccaoY;
    }

    /// <summary>
    /// Verifica se uma posição está acima do slope negativo
    /// </summary>
    /// <param name="posicaoEntrada">Posição de referência</param>
    /// <returns>Se está acima do slope negativo</returns>
    bool AcimaSlopeNegativo(Vector2 posicaoEntrada)
    {
        Vector2 posicaoPlayer = gameObject.transform.position;
        cameraLocal = Camera.main;
        Vector2 posicaoMouse = cameraLocal.ScreenToWorldPoint(posicaoEntrada);
        float interseccaoY = posicaoPlayer.y - (slopeNegativo * posicaoPlayer.x);
        float entradaInterseccao = posicaoMouse.y - (slopeNegativo * posicaoMouse.x);
        return entradaInterseccao > interseccaoY;
    }

    /// <summary>
    /// Obtém o quadrante o qual foi selecionado com o mouse em relação ao Player
    /// </summary>
    /// <returns>Quadrante clicado</returns>
    Quadrante PegarQuadrante()
    {
        Vector2 posicaoMouse = Input.mousePosition;
        Vector2 posicaoPlayer = transform.position;
        bool acimaSlopePositivo = AcimaSlopePositivo(posicaoMouse);
        bool acimaSlopeNegativo = AcimaSlopeNegativo(posicaoMouse);
        if (!acimaSlopePositivo && acimaSlopeNegativo)
        {
            return Quadrante.Leste;
        }
        else if (!acimaSlopePositivo && !acimaSlopeNegativo)
        {
            return Quadrante.Sul;
        }
        else if (acimaSlopePositivo && !acimaSlopeNegativo)
        {
            return Quadrante.Oeste;
        }
        else
        {
            return Quadrante.Norte;
        }
    }

    /// <summary>
    /// Atualiza os parâmetros do Animator de acordo com o estado atual
    /// </summary>
    void UpdateEstado()
    {
        if (atirando)
        {
            Vector2 vetorQuadrante;
            Quadrante quadranteEnum = PegarQuadrante();
            switch (quadranteEnum)
            {
                case Quadrante.Leste:
                    vetorQuadrante = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrante.Sul:
                    vetorQuadrante = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrante.Oeste:
                    vetorQuadrante = new Vector2(-1.0f, 0.0f);
                    break;
                case Quadrante.Norte:
                    vetorQuadrante = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    vetorQuadrante = new Vector2(0.0f, 0.0f);
                    break;
            }
            animator.SetBool("Atirando", true);
            animator.SetFloat("atiraX", vetorQuadrante.x);
            animator.SetFloat("atiraY", vetorQuadrante.y);
            atirando = false;
        }
        else
        {
            animator.SetBool("Atirando", false);
        }
    }
}
