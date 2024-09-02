using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 说明：角色视野
/// 
/// @by wsh 2017-05-20
/// </summary>
namespace FOW
{
    public class FOWCharactorRevealer : FOWRevealer
    {
        protected static HashSet<int> m_allChara = new();
        protected int m_charaID;

        public new static FOWCharactorRevealer Get()
        {
            return ClassObjPool<FOWCharactorRevealer>.Get();
        }

        public static bool Contains(int charaID)
        {
            return m_allChara.Contains(charaID);
        }

        public override void OnRelease()
        {
            m_allChara.Remove(m_charaID);

            base.OnRelease();
        }

        public void InitInfo(int charaID)
        {
            m_charaID = charaID;
            m_allChara.Add(m_charaID);

            Update(0);
        }

        public override void Update(int deltaMS)
        {
            if (!CheckIsValid(m_charaID, out var position, out var radius))
            {
                m_isValid = false;
            }
            else
            {
                m_position = new Vector3(position.x, 0, position.z);
                m_radius = radius;
                m_isValid = true;
            }
        }

        public static bool CheckIsValid(int entity)
        {
            return CheckIsValid(entity, out _, out _);
        }

        public static bool CheckIsValid(int entity, out Vector3 position, out float radius)
        {
            position = Vector3.zero;
            radius = 0;
            // 需要修改：真实项目走自己的角色管理逻辑，这里只是简单模拟一下
            var player = Player.GetPlayer(entity);
            if (player == null)
            {
                return false;
            }
            position = player.transform.position;
            radius = 5;
            return true;
        }
    }
}