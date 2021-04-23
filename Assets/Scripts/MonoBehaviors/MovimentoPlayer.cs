using UnityEngine;

public class MovimentoPlayer : MonoBehaviour
{
    public float velocidadeMovimento = 3.0f; // momento (impulso) a ser dado  ao player
    Vector2 movimento = new Vector2(); // movimento detectado pelo teclado

    Animator animator; // componente Animator do player
    //string estadoAnimacao = "EstadoAnimacao"; // nome do parâmetro do Controlador de Animação que determina a animação // Desnecessário com Blend Tree

    Rigidbody2D rb2D; // componente RigidBody2D do player

    //enum EstadosCaractere
    //{
    //    andaLeste = 1,
    //    andaOeste = 2,
    //    andaNorte = 3,
    //    andaSul = 4,
    //    idle = 5
    //} // Desnecessário com Blend Tree

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateEstado();
    }

    private void FixedUpdate()
    {
        MoverCaractere();
    }

    /// <summary>
    /// Realiza leitura de inputs para fazer o movimento físico do caractere
    /// </summary>
    private void MoverCaractere()
    {
        movimento.x = Input.GetAxis("Horizontal");
        movimento.y = Input.GetAxis("Vertical");
        movimento.Normalize();
        rb2D.velocity = movimento * velocidadeMovimento;
    }

    /// <summary>
    /// Atualiza o parâmetro que define a animação
    /// </summary>
    /*private void UpdateEstado()
    {
        if (movimento.x > 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaLeste);
        }
        else if (movimento.x < 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaOeste);
        }
        else if (movimento.y > 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaNorte);
        }
        else if (movimento.y < 0)
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.andaSul);
        }
        else
        {
            animator.SetInteger(estadoAnimacao, (int)EstadosCaractere.idle);
        }
    }*/
    void UpdateEstado()
    {
        if (Mathf.Approximately(movimento.x, 0) && Mathf.Approximately(movimento.y, 0))
        {
            animator.SetBool("Caminhando", false);
        }
        else
        {
            animator.SetBool("Caminhando", true);
        }
        animator.SetFloat("dirX", movimento.x);
        animator.SetFloat("dirY", movimento.y);
    }
}
