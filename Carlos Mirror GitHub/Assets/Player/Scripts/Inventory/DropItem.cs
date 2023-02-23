using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;
using System.Runtime.CompilerServices;

public class DropItem : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Button dropButton;

    private void Update()
    {
        //dropButton.interactable = transform.parent.GetChild(0).childCount > 0;
    }
}