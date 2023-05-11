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
    private int index;
    private bool isActivated;
    private Statue currStatue;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(ModifyPlayerSoul);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initailize(int index)
    {
        this.index = index;
    }
    public void UIUpdate(Statue statue)
    {
        this.currStatue = statue;
        this.isActivated = statue.SoulList[index].Item2;
        if (isActivated)
            this.name.text = statue.SoulList[index].Item1;
        else
            this.name.text = "Non Activated";
    }

    public void ModifyPlayerSoul()
    {
        if (isActivated)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().ModifySoul(name.text, 1);
            currStatue.SetSoulDisabled(index);
            currStatue.IsActivatedUI = false;
            UIManager.GetUIManager().HideStatueUi();
            currStatue = null;
        }
    }
}
