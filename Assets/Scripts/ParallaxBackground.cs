using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

    [SerializeField] private float parallaxStrength = 1f;

    private float currentOffset;
    private float spriteLength;
    private float factor = 100f;

    // Start is called before the first frame update
    void Start()
    {
        currentOffset = transform.position.x;
        var renderer = GetComponentInChildren<SpriteRenderer>();
        spriteLength = renderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        var position = Camera.main.transform.position;

        var distance = position.x * parallaxStrength;

        var newPosition = new Vector3(currentOffset + distance, transform.position.y, transform.position.z);

        transform.position = newPosition;

        var parallaxOffset = position.x * (1 - parallaxStrength);
        if (parallaxOffset > currentOffset + (spriteLength / 2))
        {
            currentOffset += spriteLength;
        }
        else if (parallaxOffset < currentOffset - (spriteLength / 2))
        {
            currentOffset -= spriteLength;
        }
    }
}
