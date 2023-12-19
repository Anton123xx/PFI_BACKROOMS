using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinSceneScript : MonoBehaviour
{
    [SerializeField]
    public TMP_Text txt;
    [SerializeField]
    public Button clickBtn;
    private void Awake()
    {
        Partage.winSceneScript = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        faire();
    }
    public void faire()
    {
    //https://stackoverflow.com/questions/8038994/display-float-as-string-with-at-least-1-decimal-place
        float temps = Partage.stopTimer();
        txt.text = "Vous avez trouvé la sortie... en " + temps.ToString("F1");
        clickBtn.onClick.AddListener(() => { SceneManager.LoadScene("Procedural"); });
    }
    public void clicked()
    {
        SceneManager.LoadScene("Procedural");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
