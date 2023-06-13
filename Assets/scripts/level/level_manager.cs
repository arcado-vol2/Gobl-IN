using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class level_manager : MonoBehaviour
{
    public int enemy_count = 0;
    public unit_manager unit_Manager;
    public Replay_manager replay_Manager;

    public GameObject end_part_ui;
    public GameObject end_level_ui;
    public GameObject level_ui;
    public TMP_Text timer_text;
    public float timer_time;

    public goblin_button[] gob_buttons = new goblin_button[3];
    void Awake()
    {
        replay_Manager = FindObjectOfType<Replay_manager>();
        Time.timeScale = 1f;
        enemy_count = FindObjectsOfType<controller_AI>().Length;
        end_part_ui.SetActive(false);
        end_level_ui.SetActive(false);
        level_ui.SetActive(true);

    }

    void Start()
    {
        for (int i = unit_Manager.units.Count; i < 3; i++)
        {
            gob_buttons[i].Disable();
        }
        timer_text.text = timer_time.ToString("F2");
    }
    void Update()
    {
        if (timer_time <= 0)
        {
            EndPart();
        }
        else
        {
            timer_time -= Time.deltaTime;
            timer_text.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer_time / 60), Mathf.FloorToInt(timer_time % 60));
        }

    }

    public void KillEnemy()
    {
        enemy_count--;
        if (enemy_count == 0)
        {
            EndLevel();
        }
    }

    public void SelectUnit(int in_id)
    {
        replay_Manager.unit_id = in_id;
        unit_Manager.RestartLevel();
    }
    public void ReturnToMenu()
    {
        Destroy(replay_Manager.gameObject);
        SceneManager.LoadScene("main_menu");
    }

    public void RestartLevel()
    {
        //replay_Manager.Restart();
        FindObjectOfType<Replay_manager>().Restart();
        unit_Manager.RestartLevel();
    }

    public void EndLevel()
    {
        Time.timeScale = 0;
        unit_Manager.DisableControll();
        end_part_ui.SetActive(false);
        end_level_ui.SetActive(true);
        level_ui.SetActive(false);
    }
    public void EndPart()
    {
        Time.timeScale = 0;
        unit_Manager.DisableControll();
        end_part_ui.SetActive(true);
        end_level_ui.SetActive(false);
        level_ui.SetActive(false);
    }
}
