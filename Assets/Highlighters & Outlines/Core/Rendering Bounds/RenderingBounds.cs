using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
    static public class RenderingBounds
    {
        static public bool CloseEnughToRenderFullScreen(Camera camera, Vector3 center,float maxDistance, float minDistance)
        {
            Vector3 c = center;
        
            var centerScreenPos = camera.WorldToViewportPoint(c);

            if (centerScreenPos.z < maxDistance && centerScreenPos.z > minDistance)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calculates rendering bounds.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="extents"></param>
        /// <param name="center"></param>
        /// <param name="renderingBounds"> Vector4(MinX, MinY, MaxX, MaxY) in viewport space</param>
        /// <returns>Vector4(MinX, MinY, MaxX, MaxY) in viewport space</returns>
        static public Vector4 CalculateBounds(Camera camera, Vector3 extents, Vector3 center, Vector4 renderingBounds,float sizeIncrease)
        {
            Vector3 e = extents + Vector3.one * sizeIncrease;
            Vector3 c = center;

            Vector3[] worldCorners = new[] {
                            new Vector3( c.x + e.x, c.y + e.y, c.z + e.z ),
                            new Vector3( c.x + e.x, c.y + e.y, c.z - e.z ),
                            new Vector3( c.x + e.x, c.y - e.y, c.z + e.z ),
                            new Vector3( c.x + e.x, c.y - e.y, c.z - e.z ),
                            new Vector3( c.x - e.x, c.y + e.y, c.z + e.z ),
                            new Vector3( c.x - e.x, c.y + e.y, c.z - e.z ),
                            new Vector3( c.x - e.x, c.y - e.y, c.z + e.z ),
                            new Vector3( c.x - e.x, c.y - e.y, c.z - e.z ),
                        };


            for (int index = 0; index < 8; index++)
            {
                var viewPos = camera.WorldToViewportPoint(worldCorners[index]);

                if(viewPos.z < 0)
                {
                    continue;
                }

                renderingBounds.x = Mathf.Min(viewPos.x, renderingBounds.x);
                renderingBounds.y = Mathf.Min(viewPos.y, renderingBounds.y);

                renderingBounds.z = Mathf.Max(viewPos.x, renderingBounds.z);
                renderingBounds.w = Mathf.Max(viewPos.y, renderingBounds.w);
            }

            return renderingBounds;
        }
    }
}