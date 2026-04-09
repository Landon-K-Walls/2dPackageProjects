using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCUtil.PhysicsHelpers
{
    public class ClickCaster 
    {
        Camera _camera;
        int _layerMask;

        public delegate void CastSignature(WorldClick click);
        public event CastSignature OnCast;

        public ClickCaster(Camera camera, int layerMask)
        {
            _camera = camera;
            _layerMask = layerMask;
        }


        //broadcast click cast.
        //Add Cast() to Input events.
        public void Cast(Vector2 screenPosition, bool isDown, ClickType type)
        {
            //Camera must exist and be referenced
            if (_camera == null)
                return;

            RaycastHit hit;

            //Cast ray from camera to clicked point on world
            Physics.Raycast(
                    _camera.transform.position,
                    _camera.ViewportToWorldPoint(new Vector3(
                        screenPosition.x / Screen.width,
                        screenPosition.y / Screen.height,
                        3f)) - _camera.transform.position,
                    out hit,
                    Mathf.Infinity,
                    _layerMask
                    );

            //State of the click
            WorldClick click = new WorldClick(
                isDown,
                hit.point,
                screenPosition,
                type
                );

            Debug.DrawLine(_camera.transform.position, click.worldPoint, Color.red, 3f);

            //broadcast state of the click.
            if (OnCast != null)
                OnCast.Invoke(click);
        }

        public void Cast(Vector2 screenPosition, bool isDown, ClickType type, out WorldClick click)
        {
            RaycastHit hit;

            Physics.Raycast(
                    _camera.transform.position,
                    _camera.ViewportToWorldPoint(new Vector3(
                        screenPosition.x / Screen.width,
                        screenPosition.y / Screen.height,
                        0.1f)) - _camera.transform.position,
                    out hit,
                    Mathf.Infinity,
                    _layerMask
                    );

            click = new WorldClick(
                isDown,
                hit.point,
                screenPosition,
                type
                );

            if (OnCast != null)
                OnCast.Invoke(click);
        }

    }

    //stores the state of a click.
    public struct WorldClick
    {
        public bool isDown;
        public ClickType type;
        public Vector3 worldPoint;
        public Vector2 screenPoint;

        public WorldClick(bool isDown, Vector3 worldPoint, Vector2 screenPoint, ClickType type)
        {
            this.isDown = isDown;
            this.worldPoint = worldPoint;
            this.screenPoint = screenPoint;
            this.type = type;
        }

        public override string ToString()
        {
            return $"{isDown}, {{{worldPoint.x}, {worldPoint.y}, {worldPoint.z}}}";
        }
    }

    public enum ClickType
    {
        Left,
        Middle,
        Right
    }
}
