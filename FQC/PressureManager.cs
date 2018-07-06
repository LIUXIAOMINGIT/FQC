using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ComunicationProtocol.Misc;

namespace FQC
{
    /// <summary>
    /// 注射器管理类
    /// 主要用于管理注射器器品牌各种语言名称及对应的产品ID
    /// </summary>
    public class PressureManager
    {
        private static PressureManager m_Manager = null;
        private Hashtable m_HashProductPressure = new Hashtable();//（Key：泵类型  value:PressureConfig）

        private PressureManager()
        {
        }

        public static PressureManager Instance()
        {
            if (m_Manager == null)
                m_Manager = new PressureManager();
            return m_Manager;
        }

        /// <summary>
        /// 按照不同语言添加不同的品牌名称
        /// </summary>
        /// <param name="lang">语言</param>
        /// <param name="brand">品牌</param>
        /// <param name="name">此语言下的品牌名称</param>
        public void Add(ProductID pid, OcclusionLevel level, float min, float max)
        {
            if (!m_HashProductPressure.ContainsKey(pid))
            {
                PressureConfig pcfg = new PressureConfig();
                pcfg.Add(level, min, max);
                m_HashProductPressure.Add(pid, pcfg);
            }
            else
            {
                PressureConfig pcfg = m_HashProductPressure[pid] as PressureConfig;
                Tuple<OcclusionLevel, float, float> pressureParameter = pcfg.Find(level);
                if (pressureParameter == null)
                {
                    pcfg.Add(level, min, max);
                }
            }
        }

        public void Clear()
        {
            m_HashProductPressure.Clear();
        }

        public PressureConfig Get(ProductID pid)
        {
            if (m_HashProductPressure.ContainsKey(pid))
                return m_HashProductPressure[pid] as PressureConfig;
            return null;
        }
    }


    public class PressureConfig
    {
        private List<Tuple<OcclusionLevel, float, float>> _pressureParameters = null;

        public List<Tuple<OcclusionLevel, float, float>> PressureParameters
        {
            get{return _pressureParameters;}
            set{_pressureParameters = value;}
        }

        public PressureConfig()
        {
            _pressureParameters = new List<Tuple<OcclusionLevel, float, float>>();
        }

        public void Add(OcclusionLevel level, float min, float max)
        {
            _pressureParameters.Add(new Tuple<OcclusionLevel, float, float>(level, min, max));
        }

        public Tuple<OcclusionLevel, float, float> Find(OcclusionLevel level)
        {
            return _pressureParameters.Find((x) => { return x.Item1 == level; });
        }
    }

}
