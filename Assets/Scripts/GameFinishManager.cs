using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameFinishManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private BoxCollider2D finishArea;
    [SerializeField] private BoxCollider2D spawnArea;
    private GameInitializer game;

    private List<Collider2D> colliders = new List<Collider2D>();
    private ContactFilter2D filter = new ContactFilter2D();
    private float timer = 0f;
    private bool started = false;
    private bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<GameInitializer>();
    }

    void FixedUpdate()
    {
        finishArea.OverlapCollider(filter.NoFilter(), colliders);

        var finishers = 0;
        foreach(var collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Character"))
                finishers++;
        }

        spawnArea.OverlapCollider(filter.NoFilter(), colliders);

        var starters = 0;
        foreach(var collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Character"))
                starters++;
        }

        started = starters < game.AllCharacters.Count;
        finished = finishers == game.AllCharacters.Count;
    }


    // Update is called once per frame
    void Update()
    {
        if (started && !finished)
            timer += Time.deltaTime;
        else if (!finished)
            timer = 0;
        timerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss\.ff");
    }
}
