using UnityEngine;

namespace Utilities
{
   public static class WorldToScreen
   {
      public static Vector2 Convert(Camera camera, Vector3 worldCoordinate)
      {
         Vector2 onScreen = camera.WorldToScreenPoint(worldCoordinate);
         return new Vector2(onScreen.x, Screen.height - onScreen.y);
      }
   }
}
