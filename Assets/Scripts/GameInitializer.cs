using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    [SerializeField] private GameObject characterObject;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private int characterCount;

    public List<Rigidbody2D> AllCharacters {get; private set;} = new List<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < characterCount; i++)
        {
            var gameObject = Instantiate(characterObject, spawnLocation.transform);
            AllCharacters.Add(gameObject.GetComponent<Rigidbody2D>());
        }
    }
}
