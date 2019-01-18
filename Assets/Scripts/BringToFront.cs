using UnityEngine;
using System.Collections;

public class BringToFront : MonoBehaviour
{
    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }
}