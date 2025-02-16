using System;
using UnityEngine;

namespace Utilities
{
    public static class AliveChecker 
    {
        public static bool IsValid(this MonoBehaviour comp) {
            try {
                if (comp.gameObject == null) return false;
            } catch(Exception) {
                return false;
            }
            return true;
        }
    }
}
