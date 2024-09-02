using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 说明：战争迷雾表现逻辑，实现所有FOW逻辑表现接口以及与FOWSystem的对接
/// 
/// @by wsh 2017-05-19
/// </summary>

namespace FOW
{
    public class FOWLogic : Singleton<FOWLogic>
    {
        private readonly string FOWRenderPath = "FOWRender";
        // 视野体
        private List<IFOWRevealer> m_revealers = new();
        // 渲染器
        private List<FOWRender> m_renders = new();

        public void Init()
        {
            m_revealers.Clear();
            m_renders.Clear();

            var go = GameObject.Find("WarFog");
            go.AddComponent<FOWSystem>();
            CreateRender(go.transform);
        }

        public void Dispose()
        {
            foreach (var revealer in m_revealers)
            {
                if (revealer != null)
                {
                    revealer.Release();
                }
            }
            m_revealers.Clear();

            foreach (var render in m_renders)
            {
                if (render != null)
                {
                    render.enabled = false;
                    // 需要修改：真实项目走自己的删除实体逻辑
                    Object.Destroy(render.gameObject);
                    
                }
            }
            m_renders.Clear();

            Object.Destroy(FOWSystem.I.gameObject);
        }

        public void AddCharactor(int charaID)
        {
            if (!FOWCharactorRevealer.Contains(charaID))
            {
                if (FOWCharactorRevealer.CheckIsValid(charaID))
                {
                    FOWCharactorRevealer revealer = FOWCharactorRevealer.Get();
                    revealer.InitInfo(charaID);
                    FOWSystem.AddRevealer(revealer);
                    m_revealers.Add(revealer);
                }
            }
        }

        public void CreateRender(Transform parent)
        {
            if (parent == null)
            {
                return;
            }

            // 需要修改：真实项目走自己的实例化预制体逻辑，设置自己的
            var prefabs = Resources.Load("Prefabs/FOWRender");
            if (prefabs != null)
            {
                var mesh = Object.Instantiate(prefabs) as GameObject;
                if (mesh != null)
                {
                    mesh.transform.parent = parent;
                    var render = mesh.gameObject.AddComponent<FOWRender>();
                    float fCenterX = 0f;
                    float fCenterZ = 0f;
                    // 注意：除以50，是因为用以渲染雾的面片，尺寸为长宽50的片
                    float scale = FOWSystem.I.worldSize / 50;
                    var transform = render.transform;
                    transform.position = new Vector3(fCenterX, 0f, -fCenterZ);
                    // 需要修改：旋转雾面片的角度，使之和地图在同一个显示面，比如一般3d游戏地面是平铺的，2d游戏地图是竖着的
                    transform.eulerAngles = new Vector3(-90, 180f, 0f);
                    transform.localScale = new Vector3(scale, scale, 1f);
                    m_renders.Add(render);
                }
            }
        }
        
        private void ActivateRender(FOWRender render, bool active)
        {
            if (render != null)
            {
                render.Activate(active);
            }
        }

        public void Update(int deltaMS)
        {
            // 说明：每个游戏帧更新，这里不做时间限制，实测对游戏帧率优化微乎其微
            UpdateRenders();
            UpdateRevealers(deltaMS);
        }

        protected void UpdateRenders()
        {
            foreach (var t in m_renders)
            {
                ActivateRender(t, FOWSystem.I.enableRender);
            }
        }

        protected void UpdateRevealers(int deltaMS)
        {
            for (int i = m_revealers.Count - 1; i >= 0 ; i--)
            {
                IFOWRevealer revealer = m_revealers[i];
                revealer.Update(deltaMS);
                if (!revealer.IsValid())
                {
                    m_revealers.RemoveAt(i);
                    FOWSystem.RemoveRevealer(revealer);
                    revealer.Release();
                }
            }
        }

        public void AddTempRevealer(Vector3 position, float radius, int leftMS)
        {
            if (leftMS <= 0)
            {
                return;
            }
            
            FOWTempRevealer tmpRevealer = FOWTempRevealer.Get();
            tmpRevealer.InitInfo(position, radius, leftMS);
            FOWSystem.AddRevealer(tmpRevealer);
            m_revealers.Add(tmpRevealer);
        }
    }
}
