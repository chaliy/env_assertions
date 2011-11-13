using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace EnvAssertions
{
    public struct FileSize
    {
        // http://stackoverflow.com/questions/128618/c-file-size-format-provider/129110#129110
        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        public static readonly FileSize Empty = new FileSize(0);

        private readonly long size;

        public FileSize(long size)
        {
            this.size = size;
        }

        public long ToLong() 
        {
            return size;
        }        

        public override string ToString()
        {
            var buffer = new StringBuilder();
            StrFormatByteSize(size, buffer, 100);
            return buffer.ToString();
        }

        [DebuggerStepThrough]
        public static implicit operator FileSize(long value)
        {
            return new FileSize(value);
        }

        [DebuggerStepThrough]
        public static implicit operator FileSize(ulong value)
        {
            return new FileSize(Convert.ToInt64(value));
        }

        [DebuggerStepThrough]
        public static implicit operator long(FileSize value)
        {
            return value.ToLong();
        }
       
    }
}
