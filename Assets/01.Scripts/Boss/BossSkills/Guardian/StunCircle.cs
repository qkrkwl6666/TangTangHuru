using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunCircle : MonoBehaviour
{
    public ParticleSystem impact;

    private float duration = 2f;
    private float time = 0f;

    private bool attack = false;
    private float damage = 1f;
    private bool isFirst = false;
}
