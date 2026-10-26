using System;

[Flags]
public enum ItemType
{
    None = 0,
    Study = 1 << 0,
    ParttimeJob = 1 << 1,
    Fanship = 1 << 2,
    Rest = 1 << 3,
    All = Study | ParttimeJob | Fanship | Rest
}