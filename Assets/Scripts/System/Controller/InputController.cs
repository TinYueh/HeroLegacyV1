using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Todo: �ɤW�ƹ��I���� Particle
    //[SerializeField]
    //private GameObject _clickParticle = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //_clickParticle.SetActive(true);
            //_clickParticle.transform.position = new Vector3(posMouse.x, posMouse.y, 0f);
        }
    }
}
