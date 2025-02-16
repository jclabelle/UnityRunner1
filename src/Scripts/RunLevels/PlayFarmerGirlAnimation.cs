using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFarmerGirlAnimation : MonoBehaviour
{
    [field: SerializeField] private int AnimID { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Animator>() is Animator animator)
        {
            animator.SetInteger("animation", 16);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>() is Animator animator)
        {
            animator.SetInteger("animation", AnimID);
        }
    }
}
