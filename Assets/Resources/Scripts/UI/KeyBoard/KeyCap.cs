using UnityEngine;
using UnityEngine.UI;

public class KeyCap : MonoBehaviour
{
    public Text keyText;
    private bool IsText;
    private Material a;
    private Material b;
    private void Awake()
    {
        this.a = this.GetComponent<MeshRenderer>().materials[0];
        this.b = this.GetComponent<MeshRenderer>().materials[1];
        this.GetComponent<MeshRenderer>().material = a;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!this.IsText)
        {
            this.IsText = true;
            foreach(var data in KeyBoard.instance.listKeyCap)
            {
                if(!data.GetComponentInChildren<KeyCap>().IsText)
                    data.GetComponentInChildren<BoxCollider>().enabled = false;
            }
            this.GetComponent<MeshRenderer>().material = b;
            this.inputText(other.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        this.GetComponent<MeshRenderer>().material = a;
        foreach (var data in KeyBoard.instance.listKeyCap)
        {
            data.GetComponentInChildren<BoxCollider>().enabled = true;
        }
    }

    private void inputText(string other)
    {
        if(other == "inputStick")
        {
            switch (this.transform.parent.name)
            {
                case "key":
                    {
                        KeyBoard.instance.Addstring(this.keyText.text);
                        this.Invoke("IsTextChange", 0.2f);
                        break;
                    }
                case "del":
                    {
                        KeyBoard.instance.Delstring();
                        this.Invoke("IsTextChange", 0.2f);
                        break;
                    }
            }
        }
    }
    private void IsTextChange()
    {
        this.IsText = false;
    }
}
