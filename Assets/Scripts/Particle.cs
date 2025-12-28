using UnityEngine;

public class Particle : MonoBehaviour
{
    void Awake(){
        Invoke("die", 0.3f);
    }

    void die(){
        Destroy(gameObject);
    }
}
