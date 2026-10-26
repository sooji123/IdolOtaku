using System;
using System.Linq;
using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class calendarTableSO : TableSO.Scripts.CsvTableSO<string, TableData.calendar>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "calendar"; }

        private static readonly DateTime BaseDate = new DateTime(2026, 3, 2);

        /// <summary>
        /// 현재일 기준으로 가장 가까운 다음 EventType 이벤트까지 남은 일수를 반환.
        /// 해당 타입의 이벤트가 없거나 모두 지났으면 -1 반환.
        /// </summary>
        public int GetDaysUntilNextEvent(EventType eventType)
        {
            DateTime today = BaseDate.AddDays(UserDataManager.Instance.GetDateIndex());

            DateTime? nearest = dataList
                .Where(e => e.event_type == eventType)
                .Select(e => new DateTime(e.year + 2025, e.month, e.day))
                .Where(d => d >= today)//수정 => 당일인 경우 0
                .OrderBy(d => d)
                .Cast<DateTime?>()
                .FirstOrDefault();

            if (nearest == null) return -1;
            return (nearest.Value - today).Days;
        }
    }
}
