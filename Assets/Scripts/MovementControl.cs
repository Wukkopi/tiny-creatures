using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    [SerializeField] private float moveStrength;
    [SerializeField] private float jumpStrength;
    [SerializeField] private List<Rigidbody2D> characters;
    [SerializeField] private float moveAnimationThreshold;
    private List<ContactPoint2D> contactPoints = new List<ContactPoint2D>();
    private GameInitializer gameInitializer;
    private CameraFollowTarget cameraFollowTarget;
 
    private bool jump;
    private float move;

    void Awake()
    {
        gameInitializer = GetComponent<GameInitializer>();
        cameraFollowTarget = Camera.main.GetComponent<CameraFollowTarget>();
    }

    void FixedUpdate()
    {
        foreach(var character in characters)
        {
            if (jump)
            {
                if (character.GetContacts(contactPoints) > 0)
                {
                    foreach (var cp in contactPoints)
                    {
                        if (cp.collider.gameObject.layer == LayerMask.NameToLayer("Level"))
                        {
                            if (cp.normal.y < 0) continue;
                            var direction = (Vector2.up * 2) + cp.normal;
                            character.AddForce(direction.normalized * jumpStrength, ForceMode2D.Impulse);
                            break;
                        }
                    }
                }
            }
            character.AddForce(Vector2.right * move * moveStrength, ForceMode2D.Force);
        }
        jump = false;
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxisRaw("Move");
        jump = jump || Input.GetAxisRaw("Jump") > 0.5f;

        // Debug.Log($"move: {move}, jump: {jump}");

        if (tryGetValidCharacter(out var character))
        {
            characters.Clear();
            characters.Add(character);
            cameraFollowTarget.SetTarget(characters[0].gameObject);
        }
        else if (shouldDeselect() || characters.Count == 0)
        {
            characters.Clear();
            characters.AddRange(gameInitializer.AllCharacters);
            if (!cameraFollowTarget.HasTarget)
            cameraFollowTarget.SetTarget(characters[0].gameObject);
        }

        foreach(var cha in gameInitializer.AllCharacters)
        {
            var animator = cha.GetComponent<Animator>();
            if (Mathf.Abs(cha.velocity.x) > moveAnimationThreshold)
                animator.Play("CreatureRun");
            else
                animator.Play("CreatureIdle");
        }
    }

    private bool shouldDeselect()
    {
        if(Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                var worldPoint = Camera.current.ScreenToWorldPoint(touch.position);
                if (Physics2D.Raycast(worldPoint, Vector2.zero))
                {
                    continue;
                }
                return true;
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return !Physics2D.Raycast(worldPoint, Vector2.zero);
        }
        return false;
    }

    private bool tryGetCharacterAtPoint(Vector3 point, out Rigidbody2D character)
    {
        var worldPoint = Camera.main.ScreenToWorldPoint(point); //ScreenPointToRay(point);
       
        character = null;
        var hit = Physics2D.Raycast(worldPoint, Vector2.zero); //, float.MaxValue, LayerMask.NameToLayer("Character"));
        if (hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            character = hit.transform.GetComponent<Rigidbody2D>();
            return true;
        }
        return false;
    }

    private bool tryGetValidCharacter(out Rigidbody2D character)
    {
        character = null;
        if(Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                if (tryGetCharacterAtPoint(touch.position, out character))
                {
                    return true;
                }
            }
        }
        else if(Input.GetMouseButtonDown(0) && tryGetCharacterAtPoint(Input.mousePosition, out character))
        {
            return true;
        }
        return false;
    }
}
