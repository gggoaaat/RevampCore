using System;

namespace Revamp.IO.Structs
{
    public interface ICommandResult
    {
        DateTime _EndTime { get; set; }
        string _Response { get; set; }
        CommandResultReturn _Return { get; set; }
        DateTime _StartTime { get; set; }
        bool _Successful { get; set; }
        string attemptedCommand { get; set; }
        long originalOrderNumber { get; set; }
    }
}