using UnityEngine;
using TableData;
using TableSO.Scripts;
using System.Linq;
using System.Collections.Generic;

namespace Table
{
    [CreateAssetMenu(fileName = "Idol_favorabilityTableSO", menuName = "TableSO/Idol_favorabilityTableSO")]
    public class Idol_favorabilityTableSO : TableSO.Scripts.CsvTableSO<string, TableData.Idol_favorability>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "Idol_favorability"; }

        private Dictionary<string, List<Idol_favorability>> _groupedByMember;

        public Dictionary<string, List<Idol_favorability>> GetMemberGroups()
        {
            if (_groupedByMember != null) return _groupedByMember;

            _groupedByMember = new Dictionary<string, List<Idol_favorability>>();

            if (dataList == null) return _groupedByMember;

            foreach (var data in dataList)
            {
                if (!_groupedByMember.ContainsKey(data.ID))
                {
                    _groupedByMember[data.ID] = new List<Idol_favorability>();
                }
                _groupedByMember[data.ID].Add(data);
            }

            foreach (var key in _groupedByMember.Keys.ToList())
            {
                _groupedByMember[key] = _groupedByMember[key]
                    .OrderBy(x => x.favorability_level)
                    .ToList();
            }

            return _groupedByMember;
        }

        public Idol_favorability GetLevelInfo(string memberID, int fanship)
        {
            var groups = GetMemberGroups();
            if (!groups.ContainsKey(memberID)) return null;

            var levels = groups[memberID];
            Idol_favorability currentLevel = null;

            // 요구 스텟(Nareq_favorabilityme)보다 현재 스텟이 높거나 같은 가장 높은 레벨을 찾음
            foreach (var level in levels)
            {
                if (fanship >= level.Nareq_favorabilityme)
                {
                    currentLevel = level;
                }
                else
                {
                    break;
                }

                if (currentLevel == null && levels.Count > 0)
                {
                    currentLevel = levels[0];
                }
            }
            return currentLevel;
        }
    }
}
