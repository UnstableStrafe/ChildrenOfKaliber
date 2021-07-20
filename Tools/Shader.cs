using UnityEngine;

namespace Items
{
    class ShaderRGG : MonoBehaviour
    {
        public static ShaderRGG Instance
        {
            get
            {
                if (!m_instance) { m_instance = ETGModMainBehaviour.Instance.gameObject.AddComponent<ShaderRGG>(); }
                return m_instance;
            }
        }
        private static ShaderRGG m_instance;

        public static Material GlitchyRGG = new Material(Shader.Find("Brave/Internal/GlitchUnlit"));
    }
}
    