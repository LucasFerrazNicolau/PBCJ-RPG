using System.Collections;
using UnityEngine;

public abstract class Caractere : MonoBehaviour
{
    //public int PontosDano; // versão anterior do valor de "dano"
    //public PontosDano pontosDano; // novo tipo que tem o valor de "dano" do objeto script
    //public int MaxPontosDano; // versão anterior do valor máximo de "dano"
    public float InicioPontosDano; // valor mínimo inicial de "saúde" do Player
    public float MaxPontosDano; // valor máximo permitido de "saúde" do Player

    /// <summary>
    /// Método que reinicia o objeto do caractere
    /// </summary>
    public abstract void ResetCaractere();


    /// <summary>
    /// Método utilizado para tornar o caractere vermelho por um breve momemnto
    /// </summary>
    /// <returns>IEnumerator relativo á duração do flick</returns>
    public virtual IEnumerator FlickerCaractere()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Método que aplica dano ao caractere
    /// </summary>
    /// <param name="dano">Quantidade de dano aplicada</param>
    /// <param name="intervalo">Intervalo de duração do dano pelo qual o player ficará imune</param>
    /// <returns>IEnumerator relativo á duração do dano</returns>
    public abstract IEnumerator DanoCaractere(int dano, float intervalo);

    /// <summary>
    /// Método que elimina o objeto do caractere
    /// </summary>
    public virtual void KillCaractere()
    {
        Destroy(gameObject);
    }
}
