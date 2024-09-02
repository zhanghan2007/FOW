using System.Collections.Generic;
using FOW;
using UnityEngine;

// 需要修改：真实项目走自己的角色管理逻辑，这里只是简单模拟一下
public class Player : MonoBehaviour
{
    private static Dictionary<int, Player> players = new Dictionary<int, Player>();
    public static Player GetPlayer(int playerId)
    {
        return players.TryGetValue(playerId, out var p) ? p : null;
    }

    public float turnSmoothing = 15f;
    
    private Rigidbody rb;
    private int playerId;
    private Camera cam;
    private Vector3 lastPosition;
    
    void Start()
    {
        playerId = 1;
        rb = GetComponent<Rigidbody>();
        players.Add(playerId, this);
        cam = Camera.main;
        lastPosition = transform.position - cam.transform.position;
        
        FOWLogic.I.AddCharactor(playerId);
    }
    
    void FixedUpdate ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        MovementManagement(h, v);
        cam.transform.position = transform.position - lastPosition;
    }
    
    void MovementManagement (float horizontal, float vertical)
    {
        if(horizontal != 0f || vertical != 0f)
        {
            Rotating(horizontal, vertical);
        }
    }
    
    void Rotating (float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
        // rb.MoveRotation(newRotation);
        rb.Move(transform.position + targetDirection*Time.deltaTime*2, newRotation);
    }
}
