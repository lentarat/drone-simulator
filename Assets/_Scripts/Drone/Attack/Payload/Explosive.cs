using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class Explosive : Payload
{
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
