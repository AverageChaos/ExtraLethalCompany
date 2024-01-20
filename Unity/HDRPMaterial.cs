using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace ExtraLethalCompany.Unity
{
    /*public static class HDRPMaterial
    {
        public static Shader HDRP_Lit = Shader.Find("HDRP/Lit");

        public static Material CreateMaterial(string asset, Shader shader)
        {
            Material material = new(shader);

            Texture texture = ExtraLethalCompany.Assets.LoadAsset<Texture>($"{asset}_Texture");
            material.SetTexture($"{asset}_Texture", texture);
            material.mainTexture = texture;

            material.EnableLocalKeyword(new(shader, "_DISABLE_SSR_TRANSPARENT"));
            material.EnableLocalKeyword(new(shader, "_NORMALMAP_TANGENT_SPACE"));

            material.name = $"{asset}_Material";

            HDMaterial.ValidateMaterial(material);

            return material;
        }
    }*/
}
