using System;
using System.Collections.Generic;
using TableData;
using UnityEngine;
using TableSO.Scripts;

/// <summary>
/// Custom Data Class - Made by TableSO CustomTableGenerator
/// Key Type: string
/// </summary>

namespace TableData
{
    [System.Serializable]
    public class otakuevent : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }
        
        public MemberType memberFilter;
        
        public OtakuEventInitialData initData;
        
        public List<OtakuEventSelectionPair> selectionData = new();
        public void AddSelection(otakuevent_select data) =>
            selectionData.Add(new OtakuEventSelectionPair(data.select_no, data.key_select,
                                              data.rsc_select, data.rsc_result_success, data.rsc_result_fail));
        
        public List<OtakuEventResult> resultData = new();
        public void AddResult(otakuevent_result data) =>
            resultData.Add(new OtakuEventResult(data));

        public otakuevent(otakuevent_type data)
        {
            this.ID = data.ID;
            this.memberFilter = data.Filter;
            initData = new OtakuEventInitialData(data.rsc_start, data.key_start);
        }

    }
}

[Serializable]
public class OtakuEventInitialData
{
    public string imageID;
    public string stringID;

    public OtakuEventInitialData (string imageID, string stringID)
    {
        this.imageID = imageID;
        this.stringID = stringID;
    }
}


[Serializable]
public class OtakuEventSelectionPair
{
    public int num;
    public string selectionStringID;
    public string resultStringID;
    public string successImageID;
    public string failImageID;
    
    public OtakuEventSelectionPair(int num, string selectionStringID, string resultStringID, string successImageID, string failImageID)
    {
        this.num = num; 
        this.selectionStringID = selectionStringID;
        this.resultStringID = resultStringID;
        this.successImageID = successImageID;
        this.failImageID = failImageID;
    }
}

[Serializable]
public class OtakuEventResult
{
    public ResultType resultType;
    public int weight;
    public int mental;
    public int fanship;

    public OtakuEventResult(otakuevent_result data)
    {
        string type = data.result_type.Replace(" ", "");    // 휴먼에러 방지
        if (type == "success") resultType = ResultType.Success;
        else if (type == "perfect") resultType = ResultType.Perfect;
        else if (type == "worst")  resultType = ResultType.Worst;
        else if (type == "fail")  resultType = ResultType.Fail;

        weight = data.weight;
        mental = data.mental;
        fanship = data.fanship;
    }
}