using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject transition;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        transition.SetActive(false);

    }

    public void PlayAnim()
    {
        transition.SetActive(true);
        anim.Play("transition", 0);
    }

    public void EndAnim()
    {
        transition.SetActive(false);
    }

    public void EnableNewUI()
    {
    }

}
