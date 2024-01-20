using GameNetcodeStuff;
using System.Collections;
using UnityEngine;

namespace ExtraLethalCompany.Extra.Creatures
{
    public class ExtraBlob : MonoBehaviour
    {
        public Coroutine EatPlayerBodyCoroutine;
        private BlobAI Blob;

        void Awake() => Blob = gameObject.GetComponent<BlobAI>();

        void OnCollisionEnter(Collision collision)
        {
            if (!Blob.IsHost || !Blob.ventAnimationFinished || Blob.isEnemyDead)
                return;

            GrabbableObject obj = collision.collider.gameObject.GetComponent<GrabbableObject>();
            if (obj != null)
            {
                DestroyImmediate(obj);
                gameObject.transform.localScale *= 1.05f;
            }
        }


        public void AskEatPlayerBody(int playerKilled) => EatPlayerBodyCoroutine ??= StartCoroutine(EatPlayerBody(playerKilled));

        private IEnumerator EatPlayerBody(int playerKilled)
        {
            yield return null;
            PlayerControllerB playerScript = StartOfRound.Instance.allPlayerScripts[playerKilled];
            float startTime = Time.realtimeSinceStartup;
            yield return new WaitUntil(() => playerScript.deadBody != null || Time.realtimeSinceStartup - startTime > 2f);
            if (playerScript.deadBody == null)
            {
                Debug.Log("Blob: Player body was not spawned or found within 2 seconds.");
                yield break;
            }
            yield return new WaitForSeconds(2f);
            gameObject.transform.localScale *= 1.1f;
            yield break;
        }
    }
}
