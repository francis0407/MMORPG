using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gamekit3D.Network;
using UnityEngine.UI;
class LookAtCameraText : MonoBehaviour
{
    public NetworkEntity networkEntity;
    public TMPro.TextMeshPro textMesh;
    private void Update()
    {
        textMesh.text = networkEntity.entityName;
        transform.LookAt(Camera.main.transform.position);
        Quaternion quaternion = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        //quaternion.y = quaternion.y + 180f;
        transform.rotation = quaternion;//Quaternion.Slerp(quaternion, transform.rotation, 10 * Time.deltaTime);
       // transform.rotation = new Quaternion(transform.rotation.x, 180f - transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }
}

