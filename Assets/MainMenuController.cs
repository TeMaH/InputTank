using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button Play;
    public GameObject GameplayUI;
    public TankMovement TankPrefab;
    public Transform StartPivot;

    private TankMovement spawnedTank;
    
    void Start()
    {
        Play.onClick.AddListener(OnPlayClicked);
        Time.timeScale = 0.0f;
    }

    void OnPlayClicked()
    { 
        gameObject.SetActive(false);
        GameplayUI.SetActive(true);
        spawnedTank = Instantiate(TankPrefab, StartPivot.position, StartPivot.rotation);
        Time.timeScale = 1.0f;
    }

    public void OpenMainMenu()
    {
        Destroy(spawnedTank?.gameObject);
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        Play?.onClick.RemoveListener(OnPlayClicked);
    }
}
