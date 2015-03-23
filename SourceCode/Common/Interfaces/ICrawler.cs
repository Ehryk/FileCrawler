using System;
using Common.Events;
using System.IO;

namespace Common.Interfaces
{
    public interface ICrawler
    {
        string Path();
        string Directory();

        bool Complete();
        bool Running();

        bool AttachOutput(IOutput output);
    }
}
