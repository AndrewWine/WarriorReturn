using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public Action doOnAttack;
    public Action<float> doOnJump;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            doOnAttack?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            doOnAttack?.Invoke();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            doOnJump?.Invoke(10.0f);
        }
    }
}
