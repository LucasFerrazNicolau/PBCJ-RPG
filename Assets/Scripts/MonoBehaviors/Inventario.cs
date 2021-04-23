using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public GameObject slotPrefab; // objeto que recebe o prefab Slot
    public const int numSlots = 5; // número fixo de slots
    Image[] itemImagens = new Image[numSlots]; // array de imagens
    Item[] items = new Item[numSlots]; // array de itens
    GameObject[] slots = new GameObject[numSlots]; // array de slots

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        CriarSlot();
    }

    /// <summary>
    /// Cria os slots iniciais do inventário
    /// </summary>
    public void CriarSlot()
    {
        if (slotPrefab != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                GameObject novoSlot = Instantiate(slotPrefab);
                novoSlot.name = "ItemSlot_" + i;
                novoSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = novoSlot;
                itemImagens[i] = novoSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    /// <summary>
    /// Inclui novo item ao inventário
    /// </summary>
    /// <param name="itemToAdd">Novo item a ser adicionado</param>
    /// <returns>Retorna se o item foi incluído com sucesso</returns>
    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].tipoItem == itemToAdd.tipoItem && itemToAdd.empilhavel)
            {
                items[i].quantidade++;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantidadeTexto = slotScript.qtdTexto;
                quantidadeTexto.enabled = true;
                quantidadeTexto.text = items[i].quantidade.ToString();
                return true;
            }

            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantidade = 1;
                itemImagens[i].sprite = itemToAdd.sprite;
                itemImagens[i].enabled = true;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Remove item do inventário, caso exista no inventário
    /// </summary>
    /// <param name="itemToRemoveName">Nome do item a ser removido</param>
    /// <returns>Retorna se o item foi removido do inventário</returns>
    public bool RemoveItem(string itemToRemoveName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].nomeObjeto == itemToRemoveName && (!items[i].empilhavel || items[i].quantidade > 0))
            {
                if (items[i].empilhavel)
                {
                    items[i].quantidade--;
                    Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                    Text quantidadeTexto = slotScript.qtdTexto;
                    quantidadeTexto.enabled = true;
                    quantidadeTexto.text = items[i].quantidade.ToString();
                }
                else
                {
                    Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                    Text quantidadeTexto = slotScript.qtdTexto;
                    quantidadeTexto.enabled = false;
                    quantidadeTexto.text = "";
                    itemImagens[i].sprite = null;
                    itemImagens[i].enabled = false;
                    items[i] = null;
                }

                return true;
            }
        }

        return false;
    }
}
