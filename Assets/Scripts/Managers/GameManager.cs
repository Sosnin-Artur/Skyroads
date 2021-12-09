using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int lives = 1;    

    [SerializeField] private Text timeText;
    [SerializeField] private Text hightTimeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text hightScoreText;
    [SerializeField] private Text asteroidsCountText;
    [SerializeField] private Text hightAsteroidsCountText;

    [SerializeField] private GameObject hints;
    [SerializeField] private GameObject gameOverMenu;
    
    private float _speedModifier = 1.0f;

    private float _score = 0;
    private float _hightScore = 0;
    private int _time = 0;
    private int _hightTime = 0;
    private int _asteroidsCount = 0;
    private int _hightActeroidsCount = 0;    

    private bool _isGameStarted = false;

    private void Awake()
    {
        LoadData();
        hightScoreText.text =  "Hight Score: " + _hightScore.ToString();        
        hightTimeText.text =  "Best time : " + _hightTime.ToString();        
        hightAsteroidsCountText.text = "Best asteroids: " + _hightActeroidsCount.ToString();
    }    

    private void Start()
    {            
        Time.timeScale = 0;
        Cursor.visible = false;                
        hints.SetActive(true);
        gameOverMenu.SetActive(false);
        timeText.text = "";
    }
    
    private void Update()
    {        
        if (Input.anyKey && !_isGameStarted)
        {
            _isGameStarted = true;
            Time.timeScale = 1;            
            hints.SetActive(false);
            StartCoroutine(Timer());            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveData();
            Application.Quit();
        }
    }    

    private IEnumerator GameOver()
    {
        SaveData();
        Cursor.visible = true;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(2.0f);        
    }

    private IEnumerator Timer()
    {   
        while (true)
        {
            yield return new WaitForSeconds(1.0f);                
            _time++;
            AddScore(1 * _speedModifier);
            if (_time > _hightTime)
            {
                _hightTime = _time;                
                hightTimeText.text =  "Best time : " + _hightTime.ToString();     
            }
            timeText.text = _time.ToString();            
        }                             
    }

    private void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath 
            + "/SaveData.dat"); 
        
        SaveData data = new SaveData();
        data.HightTime = _hightTime;
        data.HightAsteroidsCount = _hightActeroidsCount;
        data.HightScore = _hightScore;        
        bf.Serialize(file, data);
        file.Close();
    }

    private void LoadData()
    {
        if (File.Exists(Application.persistentDataPath 
            + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = 
            File.Open(Application.persistentDataPath 
            + "/SaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            
            _hightTime = data.HightTime;
            _hightScore = data.HightScore;            
            _hightActeroidsCount = data.HightAsteroidsCount;
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

    public void AddScore(float score)
    {
        _score += score;        
        if (_score > _hightScore)
        {
            _hightScore = _score;
            hightScoreText.text =  "Hight Score: " + _hightScore.ToString();
        }
        scoreText.text = "Score: " + _score;
    }

    public void AddAsteroid(int value)
    {
        _asteroidsCount += value;        
        if (_asteroidsCount > _hightActeroidsCount)
        {
            _hightActeroidsCount = _asteroidsCount;
            hightAsteroidsCountText.text = "Best asteroids: " + _hightActeroidsCount.ToString();
        }
        asteroidsCountText.text = "Asteroids: " + _asteroidsCount.ToString();
    }
    
    public void AddLives(int value)
    {
        lives += value;        

        if (lives <= 0)
        {
            StartCoroutine(GameOver());
        }        
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }    

    public void Boost(float boost)
    {        
        _speedModifier = boost;        
    }
}
