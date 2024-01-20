using GameNetcodeStuff;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace ExtraLethalCompany.Extra.Creatures
{
    public class ExtraSporeLizzard : MonoBehaviour
    {
        void Update()
        {
            if (StartOfRound.Instance == null)
                return;

            LocalVolumetricFog[] smokes = FindObjectsOfType<LocalVolumetricFog>().Where(g => g.name.Contains("PufferSmoke", System.StringComparison.OrdinalIgnoreCase)).ToArray();
            if (smokes.Length <= 0)
                return;

            PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[StartOfRound.Instance.thisClientPlayerId];
            Vector3 pos = player.gameObject.transform.position;
            for (int i = 0; i < smokes.Length; i++)
            {
                Vector3 smokePos = smokes[i].transform.position;
                if (Mathf.Abs(pos.x - smokePos.x) > smokes[i].transform.localScale.x &&
                    Mathf.Abs(pos.y - smokePos.y) > smokes[i].transform.localScale.y &&
                    Mathf.Abs(pos.z - smokePos.z) > smokes[i].transform.localScale.z)
                    continue;

                player.DamagePlayer((int)(15 * Time.deltaTime), causeOfDeath: CauseOfDeath.Suffocation);
            }
        }
    }
}
