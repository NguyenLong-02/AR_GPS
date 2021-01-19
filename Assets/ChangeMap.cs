using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChangeMap : MonoBehaviour
{
    Button changeMap;
    void Start()
    {
        changeMap = GetComponent<Button>();
        changeMap.onClick.AddListener(() =>
       {
           SceneManager.LoadScene("Flat Map - Loading Screen");
       });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
