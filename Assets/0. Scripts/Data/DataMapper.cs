using System.Collections.Generic;

public class DataMapper
{
    public CharacterData MappingCharacterData(List<string> rowList)
    {
        TypeParser parser = new TypeParser();

        CharacterData characterData =
            new CharacterData(parser.ParseToInt(rowList[0]),
                              rowList[1],
                              rowList[2],
                              rowList[3],
                              rowList[4],
                              rowList[5]);

        return characterData;
    }

    public MessageData MappingMessageData(List<string> rowList)
    {
        TypeParser parser = new TypeParser();

        /*MessageData messageData =
            new MessageData(parser.ParseToInt(rowList[0]),
                rowList[1],
                rowList[2],
                rowList[3],
                rowList[4],
                rowList[5],
                rowList[6]);
        return messageData;*/
        return null;
    }

    public ItemData MappingItemData(List<string> rowList)
    {
        TypeParser parser = new TypeParser();

        ItemData itemData =
            new ItemData(rowList[0],
                rowList[1],
                rowList[2],
                parser.ParseToItemType(rowList[3]),

                rowList[4],
                parser.ParseToFloat(rowList[5]),
                parser.ParseToInt(rowList[6]),
                parser.ParseToBool(rowList[7]),
                
                rowList[8],
                parser.ParseToFloat(rowList[9]),
                parser.ParseToInt(rowList[10]),
                parser.ParseToBool(rowList[11]),

                rowList[12]);

        return itemData;
    }

    public ScheduleData MappingScheduleData(List<string> rowList)
    {
        TypeParser parser = new TypeParser();

        return new ScheduleData(rowList[0], rowList[1], rowList[2], rowList[3],
            rowList[4], parser.ParseToInt(rowList[5]));
    }

    public ScheduleResultData MappingScheduleStatReward(List<string> rowList)
    {
        TypeParser parser = new TypeParser();

        return new ScheduleResultData(rowList[2], parser.ParseToInt(rowList[3]), parser.ParseToInt(rowList[4]),
            rowList[5], parser.ParseToInt(rowList[6]),
            rowList[7], parser.ParseToInt(rowList[8]));
    }

    public ParttimeLevelData MappingParttimeLevelData(List<string> rowList)
    {
        TypeParser parser = new TypeParser();

        return new ParttimeLevelData(rowList[0], parser.ParseToInt(rowList[1]),
            parser.ParseToInt(rowList[4]), parser.ParseToInt(rowList[3]), parser.ParseToInt(rowList[5]), parser.ParseToInt(rowList[6]),
            parser.ParseToFloat(rowList[7]),
            rowList[8], parser.ParseToFloat(rowList[9]),
            rowList[10], parser.ParseToFloat(rowList[11]),
            rowList[12], parser.ParseToFloat(rowList[13]));
    }

    public ConfigData MappingConfigData(List<string> rowList)
    {
        TypeParser parser = new TypeParser();

        return new ConfigData(parser.ParseToFloat(rowList[0]),
                              parser.ParseToFloat(rowList[1]),
                              parser.ParseToFloat(rowList[2]),
                              parser.ParseToFloat(rowList[3]),
                              parser.ParseToFloat(rowList[4]),
                              parser.ParseToFloat(rowList[5]),
                              parser.ParseToFloat(rowList[6]),
                              parser.ParseToFloat(rowList[7]),
                              parser.ParseToFloat(rowList[8]),
                              parser.ParseToFloat(rowList[9]),
                              parser.ParseToFloat(rowList[10]),
                              parser.ParseToFloat(rowList[11]),
                              parser.ParseToFloat(rowList[12]),
                              parser.ParseToFloat(rowList[13]),
                              parser.ParseToFloat(rowList[14]));
    }
}