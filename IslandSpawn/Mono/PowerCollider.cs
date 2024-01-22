using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class PowerCollider : MonoBehaviour
    {
        public static bool playerInRange;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player")
                playerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.name == "Player")
                playerInRange = false;
        }
    }
}