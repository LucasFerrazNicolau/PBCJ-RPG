using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]

public class Perambular : MonoBehaviour
{
    public float velocidadePerseguicao; // velocidade do inimigo na área de spot
    public float velocidadePerambular; // velocidade do inimigo passeando
    float velocidadeCorrente; // velocidade do inimigo atribuída

    public float intervaloMudancaDirecao; // tempo para alterar direção
    public bool perseguePlayer; // indicador de perseguidor ou não

    Coroutine moverCoroutine; // corotina para movimentar o inimigo

    Rigidbody2D rb2D; // armazena o componente Rigidbody2D
    Animator animator; // armazena o componente Animator

    Transform alvoTransform = null; // armazena o componente Transform do alvo (Player)

    Vector3 posicaoFinal; // posição final (do algortimo Perambular)
    float anguloAtual = 0; // direção de ângulo utilizada no Perambular

    CircleCollider2D circleCollider; // armazena o componente de spot

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocidadeCorrente = velocidadePerambular;
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(RotinaPerambular());
        circleCollider = GetComponent<CircleCollider2D>();

        // Atribuição inicial para aproximar os pontos de destino do Perambular do caractere
        posicaoFinal = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2D.position, posicaoFinal, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && perseguePlayer)
        {
            velocidadeCorrente = velocidadePerseguicao;
            alvoTransform = collision.gameObject.transform;
            if (moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }
            moverCoroutine = StartCoroutine(Mover(rb2D, velocidadeCorrente));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Caminhando", false);
            velocidadeCorrente = velocidadePerambular;
            if (moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }
            alvoTransform = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    /// <summary>
    /// Método padrão do Perambular enquanto player não foi encontrado
    /// </summary>
    /// <returns>IEnumartor relativo ao intervalo de encontrar novo ponto de destino aleatório</returns>
    public IEnumerator RotinaPerambular()
    {
        while (true)
        {
            EscolherNovoPontoFinal();
            if (moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }
            moverCoroutine = StartCoroutine(Mover(rb2D, velocidadeCorrente));
            yield return new WaitForSeconds(intervaloMudancaDirecao);
        }
    }

    /// <summary>
    /// Seleciona um novo destino aleatório
    /// </summary>
    void EscolherNovoPontoFinal()
    {
        anguloAtual += Random.Range(0, 360);
        anguloAtual = Mathf.Repeat(anguloAtual, 360);
        posicaoFinal += Vector3ParaAngulo(anguloAtual);
    }

    /// <summary>
    /// Obtém uma direção com base no ângulo indicado
    /// </summary>
    /// <param name="anguloEntradaGraus">Ângulo em graus</param>
    /// <returns>Direção obtida do ângulo</returns>
    Vector3 Vector3ParaAngulo(float anguloEntradaGraus)
    {
        float anguloEntradaRadianos = anguloEntradaGraus * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(anguloEntradaRadianos), Mathf.Sin(anguloEntradaRadianos), 0);
    }

    /// <summary>
    /// Método que movimenta o RigidBody do objeto que está permbulando
    /// </summary>
    /// <param name="rbParaMover">RigidBody2D que deve ser movimentado</param>
    /// <param name="velocidade">Velocidade com que o objeto se movimenta</param>
    /// <returns>IEnumerator respectivo ao intervalo de tempo do FixedUpdate</returns>
    public IEnumerator Mover(Rigidbody2D rbParaMover, float velocidade)
    {
        float distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
        while (distanciaFaltante > float.Epsilon)
        {
            if (alvoTransform != null)
            {
                posicaoFinal = alvoTransform.position;
            }
            if (rbParaMover != null)
            {
                animator.SetBool("Caminhando", true);
                Vector3 novaPosicao = Vector3.MoveTowards(rbParaMover.position, posicaoFinal, velocidade * Time.deltaTime);
                rb2D.MovePosition(novaPosicao);
                distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("Caminhando", false);
    }
}
