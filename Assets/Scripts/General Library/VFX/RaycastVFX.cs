using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.VFX
{
    public class RaycastVFX : MonoBehaviour
    {
        public UnityEngine.Camera _camera;
        public Transform start;
        public GameObject prefab;

        void Awake()
        {
            if (_camera == null) _camera = GetComponentInParent<UnityEngine.Camera>();
        }

        public void CreateRaycastObject(RaycastHit hit)
        {
            Vector3 end = transform.position + transform.forward * 1000f;

            if (hit.collider != null)
                end = hit.point;

            CreatePrefabBetweenPoints(start.position, end, 0.03f);
        }

        void CreatePrefabBetweenPoints(Vector3 start, Vector3 end, float width)
        {
            if (prefab == null) return;

            var offset = end - start;
            var scale = new Vector3(width, offset.magnitude / 2.0f, width);
            var position = start + (offset / 2.0f);

            var cylinder = Instantiate(prefab, position, Quaternion.identity);
            cylinder.transform.up = offset;
            cylinder.transform.localScale = scale;
        }
    }
}