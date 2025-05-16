using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyBoard : Singleton<KeyBoard>
{
    public Text inputKeyText;
    public List<GameObject> listKeyCap;

    public void KeyBoardInit()
    {
        this.inputKeyText.text = "";
    } 
    private void Start()
    {
        this.inputKeyText.text = "";
    }
    public void Addstring(string key)
    {
        if (this.inputKeyText.text.Length > 4) return;
        this.inputKeyText.text = this.inputKeyText.text + key;
    }
    public void Delstring()
    {
        if (this.inputKeyText.text.Length < 1) return;

        this.inputKeyText.text = this.inputKeyText.text.Substring(0, inputKeyText.text.Length - 1);
    }
}
