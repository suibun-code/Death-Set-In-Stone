using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RespawnPlayer : Singleton<RespawnPlayer>
{
    public TextMeshProUGUI livesAmountText;

    public AudioSource music;

    public int lives = 6;

    public Vector3 playerSpawnPosition = new Vector3(0f, 0.5f, 0f);

    public GameObject player;
    public GameObject pf_deadPlayer;

    private void Start()
    {
        livesAmountText.text = lives.ToString();
    }

    public void Respawn()
    {
        GameObject deadPlayer = Instantiate(pf_deadPlayer, player.transform.position, player.transform.rotation, transform);
        deadPlayer.transform.localScale = player.transform.localScale;

        player.transform.position = playerSpawnPosition;

        lives--;
        music.pitch += 0.05f;

        livesAmountText.text = lives.ToString();

        player.transform.localScale = Vector3.one;

        if (lives < 0)
        {
            SceneManager.LoadScene("Loss");
        }
    }
}
