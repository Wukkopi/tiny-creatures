using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishManager : MonoBehaviour
{
    
    [SerializeField]private BoxCollider2D finishArea;
    private GameInitializer game;

    private List<Collider2D> colliders = new List<Collider2D>();
    private ContactFilter2D filter = new ContactFilter2D();
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
        if (finishers > 0)
           // Debug.Log($"FINISHERS: {finishers}");
        if (finishers == game.AllCharacters.Count)
        {
            Debug.LogWarning("FINISH");
        }    
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
