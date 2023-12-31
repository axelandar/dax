﻿using ADOTabular;
using DaxStudio.Interfaces;

namespace DaxStudio.UI.Interfaces
{
    public interface IDaxDocument
    {
        string Title { get; }
        IConnectionManager Connection { get; }
        void OutputError(string message);
        void OutputMessage(string message);
        void OutputWarning(string message);
        IDaxStudioHost Host { get; }
    }
}
