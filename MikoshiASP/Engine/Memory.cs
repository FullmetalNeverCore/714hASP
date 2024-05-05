using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
namespace MikoshiASP.Engine
{
    public interface Memory
    {

        string? open_json(string path);

        void save_json(string data, string path, byte b = 0);

    }
}

