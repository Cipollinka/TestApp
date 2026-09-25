using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float velocity = 10f;

    public bool isFirstTap = false, isFinishGame = false;
    public GameObject hideOnFirstTap;
    [SerializeField] private int carHP = 100;

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private TMP_Text hpText, targetText, gameStatusText;
    [SerializeField] private Image hpBar;
    [SerializeField] private GameObject gameOverPanel;
    public int countKilledEnemies = 0;
    int targetKilledEnemies = 5;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        isFirstTap = false;
        hpBar.transform.parent.gameObject.SetActive(false);
        targetKilledEnemies = PlayerPrefs.GetInt("TargetKilledEnemies", 5);
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (isFirstTap)
            {
                for (int i = 0; i < Random.Range(1,4); i++)
                {
                    Instantiate(enemyPrefab, new Vector3(Random.Range(-5f, 5f), 0, Random.Range(20, 75)), Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !isFirstTap)
        {
            isFirstTap = true;
            hideOnFirstTap.SetActive(false);
            StartCoroutine(SpawnEnemies());
        }
        targetText.text = $"{countKilledEnemies}/{targetKilledEnemies}";
        if (countKilledEnemies >= targetKilledEnemies)
        {
            StartCoroutine(WinGame());
        }
    }

    IEnumerator WinGame()
    {
        gameOverPanel.SetActive(true);
        isFinishGame = true;
        gameStatusText.text = "You Win!";
        gameStatusText.gameObject.SetActive(true);
        PlayerPrefs.SetInt("TargetKilledEnemies", targetKilledEnemies + 5);
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    
    public void TakeDamage(int damage)
    {
        if (isFinishGame)
        {
            return;
        }
        carHP -= damage;
        if (carHP <= 0)
        {
            StartCoroutine(EndGame());
        }
        hpBar.transform.parent.gameObject.SetActive(true);
        hpText.text = $"{carHP}/100";
        hpBar.fillAmount = (float)carHP / 100f;
    }

    IEnumerator EndGame()
    {
        gameOverPanel.SetActive(true);
        isFinishGame = true;
        gameStatusText.text = "Game Over";
        gameStatusText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
