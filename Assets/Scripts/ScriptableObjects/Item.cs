using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject
{
    public string nomeObjeto; // Nome do item
    public Sprite sprite; // Sprite do item utilizada no inventário
    public int quantidade; // Quantidade a qual o item coletável corresponde
    public bool empilhavel; // Se o item acumula no inventário

    public enum TipoItem
    {
        MOEDA,
        HEALTH,
        CHAVE,
        EQUIPAMENTO
    }

    public TipoItem tipoItem; // Tipo do item definido pela Enum
}
