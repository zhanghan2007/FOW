using UnityEngine;

/// <summary>
/// 说明：FOW表现层渲染脚本
/// 
/// @by wsh 2017-05-20
/// </summary>
namespace FOW
{
    public class FOWRender : MonoBehaviour
    {
        // 这里设置战争迷雾颜色
        public Color unexploredColor = new Color(0f, 0f, 0f, 255f / 255f);
        public Color exploredColor = new Color(0f, 0f, 0f, 128f / 255f);

        private MeshRenderer render;
        private MaterialPropertyBlock propertyBlock;

        void Start()
        {
            propertyBlock = new MaterialPropertyBlock();
            render = GetComponentInChildren<MeshRenderer>();
        }

        public void Activate(bool active)
        {
            gameObject.SetActive(active);
        }

        public bool IsActive
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        void OnWillRenderObject()
        {
            if (FOWSystem.I.texture != null)
            {
                render.GetPropertyBlock(propertyBlock);

                propertyBlock.SetTexture("_MainTex", FOWSystem.I.texture);
                propertyBlock.SetFloat("_BlendFactor", FOWSystem.I.blendFactor);
                if (FOWSystem.I.enableFog)
                {
                    propertyBlock.SetColor("_Unexplored", unexploredColor);
                }
                else
                {
                    propertyBlock.SetColor("_Unexplored", exploredColor);
                }
                propertyBlock.SetColor("_Explored", exploredColor);

                render.SetPropertyBlock(propertyBlock);
            }
        }
    }
}