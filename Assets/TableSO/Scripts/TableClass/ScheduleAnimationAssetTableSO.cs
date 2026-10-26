using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableSO.Scripts;

namespace Table
{
    public class ScheduleAnimationAssetTableSO : TableSO.Scripts.AssetTableSO<TableData.ScheduleAnimationAsset>
    {
        public override TableType tableType => TableType.Asset;

        [SerializeField] private string assetFolderPath = "Assets/TableSO/Asset/ScheduleAnimation";
        public override string label { get => "ScheduleAnimationAssetTableSO"; }
        public override Type assetType { get => typeof(Sprite); }
        
        private Dictionary<string, List<(int, Sprite)>> animationDict = new Dictionary<string, List<(int, Sprite)>>();
        public List<ScheduleAnimationData> animations = new List<ScheduleAnimationData>();

        public override async Task UpdateData()
        {
            await base.UpdateData();
            
            ParseToAnimationList();
            
            ReleaseData();
        }

        private void ParseToAnimationList()
        {
            animationDict.Clear();
            animations.Clear();
            
            foreach (var data in dataList)
            {
                (string name, int index) = AnimationParser.Parse(data.ID);

                if (!animationDict.ContainsKey(name))
                    animationDict.Add(name, new List<(int, Sprite)>());

                animationDict[name].Add((index, data.Asset));
            }

            foreach (var key in animationDict.Keys.ToList())
            {
                animationDict[key].Sort((a, b) => a.Item1.CompareTo(b.Item1));
            }

            foreach (var (key, value) in animationDict)
                animations.Add(new ScheduleAnimationData(key, value.Select(item => item.Item2).ToList()));
        }

        public List<Sprite> GetAnimationClip(string key)
        {
            string targetKey = key.Replace("\"", "");
            
            Debug.Log($"입력 애니메이션 키 : {key}");
            foreach (var animation in animations)
            {
                if (targetKey.Equals(animation.Key.Replace("\"", "")))return animation.Clips;
            }
            
            // Return Default
            return animations[0].Clips;
        }
    }

    public class AnimationParser
    {
        public static (string name, int index) Parse(string input)
        {
            int lastUnderscore = input.LastIndexOf('_');
            if (lastUnderscore == -1)
                throw new ArgumentException("입력 문자열에 '_'가 없습니다.");

            string namePart = input.Substring(0, lastUnderscore);
            string numberPart = input.Substring(lastUnderscore + 1);

            if (!int.TryParse(numberPart, out int index))
                throw new ArgumentException("마지막 부분이 정수가 아닙니다.");

            return (namePart, index);
        }
    }
}
