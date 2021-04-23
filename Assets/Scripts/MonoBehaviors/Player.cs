using System.Collections;
using UnityEngine;

public class Player : Caractere
{
    public Inventario inventarioPrefab; // referência ao objeto prefab criado do Inventário
    Inventario inventario; // referência ao Inventário

    public HealthBar healthBarPrefab; // referência ao objeto prefab criado da HealthBar
    HealthBar healthBar; // referência à HealthBar

    public PontosDano pontosDano; // novo tipo que tem o valor da "saúde" do objeto script

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        inventario = Instantiate(inventarioPrefab);

        pontosDano.valor = InicioPontosDano;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.caractere = this;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coletavel"))
        {
            Item danoObjeto = collision.gameObject.GetComponent<Consumable>().item;
            if (danoObjeto != null)
            {
                bool deveDesaparecer = false;
                //print("Acertou: " + danoObjeto.nomeObjeto);

                switch (danoObjeto.tipoItem)
                {
                    case Item.TipoItem.MOEDA:
                        //deveDesaparecer = true;
                        deveDesaparecer = inventario.AddItem(danoObjeto);
                        break;
                    case Item.TipoItem.HEALTH:
                        deveDesaparecer = AjustarPontosDano(danoObjeto.quantidade);
                        break;
                    case Item.TipoItem.CHAVE:
                        deveDesaparecer = inventario.AddItem(danoObjeto);
                        break;
                    case Item.TipoItem.EQUIPAMENTO:
                        deveDesaparecer = inventario.AddItem(danoObjeto);
                        break;
                    default:
                        break;
                }

                Objetivo objetivo = collision.gameObject.GetComponent<Objetivo>();
                if (objetivo != null)
                {
                    string nextScene = objetivo.nextScene;
                    objetivo.IrParaProximaCena();
                    if (nextScene == "Win")
                        KillCaractereNotGameOver();
                }

                if (deveDesaparecer)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Porta"))
        {
            if (inventario.RemoveItem("Chave"))
            {
                collision.gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            Item premio = collision.gameObject.GetComponent<NPC>().premio;
            inventario.RemoveItem(premio.nomeObjeto);
            inventario.AddItem(premio);
        }
    }

    public override IEnumerator DanoCaractere(int dano, float intervalo)
    {
        while (true)
        {
            StartCoroutine(FlickerCaractere());

            pontosDano.valor -= dano;

            if (pontosDano.valor <= float.Epsilon)
            {
                KillCaractere();
                break;
            }

            if (intervalo > float.Epsilon)
            {
                yield return new WaitForSeconds(intervalo);
            }
            else
            {
                break;
            }
        }
    }

    public override void KillCaractere()
    {
        base.KillCaractere();
        Destroy(healthBar.gameObject);
        Destroy(inventario.gameObject);

        RPGGameManager gameManager = GameObject.Find("RPGGameManager").GetComponent<RPGGameManager>();
        gameManager.isGameOver = true;
    }

    /// <summary>
    /// Implementa o KillCaractere sem dispara evento de Game Over
    /// </summary>
    public void KillCaractereNotGameOver()
    {
        base.KillCaractere();
        Destroy(healthBar.gameObject);
        Destroy(inventario.gameObject);
    }

    public override void ResetCaractere()
    {
        inventario = Instantiate(inventarioPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.caractere = this;
        pontosDano.valor = InicioPontosDano;
    }

    /// <summary>
    /// Método que recupera os pontos de dano (saúde) do player
    /// </summary>
    /// <param name="quantidade">Quantidade de pontos de vida recebida</param>
    /// <returns>Retorna se a quantidade de vida foi aumentada, isto é, se já não estava no máximo</returns>
    public bool AjustarPontosDano(int quantidade)
    {
        /*if (pontosDano.valor < MaxPontosDano)
        {
            pontosDano.valor += quantidade;
            //print("Ajustando Pontos de Dano por: " + quantidade + ". Novo valor: " + pontosDano.valor);
            return true;
        }
        else
        {
            return false;
        }*/

        // Correção a vida ser coletada mesmo com vida cheia
        if (pontosDano.valor < MaxPontosDano)
            pontosDano.valor += quantidade;
        return true;
    }
}
