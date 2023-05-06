using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SoulSelectorUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text name;

    private TMP_Text price;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(ModifyPlayerSoul);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(string soulName)
    {
        this.name.text = soulName;
    }

    public void ModifyPlayerSoul()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().ModifySoul(name.text, 1);
    }
}
