using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player;
    public GameObject PlayerCam;

    public GameObject Floor;
    private Material floorMaterial;
    private float floorMaxStrength = 1.0f;
    private bool floorBeingSeen = false;

    public Dictionary<string, System.Action> TagToMethod = new Dictionary<string, System.Action>();

    private int invisMask = ~(1 << 9);

    private float timePlayerSeen = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        TagToMethod.Add("Player", PlayerSeen);
        floorMaterial = Floor.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        floorBeingSeen = false;
        SendRayCast(PlayerCam.transform.position, PlayerCam.transform.forward);
        PlayerSeenBehaviour(floorBeingSeen);
    }

    public void SendRayCast(Vector3 pos, Vector3 dir) {
        RaycastHit hit;
        Debug.DrawRay(pos, dir * 1000f);
        if(Physics.Raycast(pos, dir, out hit, 1000.0f, invisMask)){
            if (hit.transform.CompareTag("Mirror")) {
                Vector3 reflection = Vector3.Reflect(PlayerCam.transform.forward.normalized, hit.normal);
                SendRayCast(hit.point, reflection);
            }
            else {
                if (TagToMethod.ContainsKey(hit.transform.tag)) {
                    TagToMethod[hit.transform.tag].Invoke();
                }
            }
        }
    }

    public void PlayerSeen() {
        floorBeingSeen = true;
    }

    public void PlayerSeenBehaviour(bool seen) {
        if (seen) {
            if (timePlayerSeen < floorMaxStrength)
                timePlayerSeen += Time.deltaTime * 0.1f;
        }
        else {
            if(timePlayerSeen > 0f)
                timePlayerSeen -= Time.deltaTime * 1.0f;
            
        }

        floorMaterial.SetFloat("_RippleStrength", 1 - (floorMaxStrength - timePlayerSeen));
        floorMaterial.SetFloat("_RippleColorStrength", 1 - (floorMaxStrength - timePlayerSeen));
    }
}
