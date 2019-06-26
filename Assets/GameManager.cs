using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public GameObject Player;
    public GameObject PlayerCam;

    public List<EffectVariables> effects;

    private int invisMask = ~(1 << 9);

    // Start is called before the first frame update
    void Start() {
        Instance = this;

        for (int i = 0; i < effects.Count; i++) {
            effects[i].materials = new Material[effects[i].materialObjects.Length];
            for (int j = 0; j < effects[i].materialObjects.Length; j++) {
                effects[i].materials[j] = effects[i].materialObjects[j].GetComponent<Renderer>().material;
                effects[i].currentlySeen = false;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < effects.Count; i++) {
            effects[i].currentlySeen = false;
        }

        SendRayCast(PlayerCam.transform.position, PlayerCam.transform.forward);
    }

    public void SendRayCast(Vector3 pos, Vector3 dir) {
        RaycastHit hit;
        Debug.DrawRay(pos, dir * 1000f);
        if (Physics.Raycast(pos, dir, out hit, 1000.0f, invisMask)) {
            if (hit.transform.CompareTag("Mirror")) {
                Vector3 reflection = Vector3.Reflect(PlayerCam.transform.forward.normalized, hit.normal);
                SendRayCast(hit.point, reflection);
            }
            else {
                Debug.Log(hit.transform.tag);
                for (int i = 0; i < effects.Count; i++) {
                    if (effects[i].tag == hit.transform.tag && effects[i].timer < effects[i].waitTime + effects[i].timeEffectChange) {
                        effects[i].timer += Time.deltaTime;
                        if (effects[i].timer > effects[i].waitTime + effects[i].timeEffectChange)
                            effects[i].timer = effects[i].waitTime + effects[i].timeEffectChange;
                    }
                    else if(effects[i].tag != hit.transform.tag && effects[i].timer > 0f) {
                        effects[i].timer -= Time.deltaTime * effects[i].disappearRate;
                        if (effects[i].timer < 0f)
                            effects[i].timer = 0f;
                    }

                    foreach(Material mat in effects[i].materials) {
                        mat.SetFloat(effects[i].effectReference, ((effects[i].timer > effects[i].waitTime ? (effects[i].timer - effects[i].waitTime) : 0f) / effects[i].timeEffectChange) * effects[i].increase);
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class EffectVariables {
    public GameObject[] materialObjects;
    [System.NonSerialized] public Material[] materials;
    [System.NonSerialized] public bool currentlySeen;
    [System.NonSerialized] public float timer;
    public string effectReference;
    public float waitTime;
    public float timeEffectChange;
    public float increase;
    public float disappearRate;
    public string tag;
}
