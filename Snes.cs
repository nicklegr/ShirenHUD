using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;

namespace ShirenHUD
{
    class Snes
    {
        static int RamBase = 0x7e0000;
        static int RamSize = 0x20000;

        byte[] mRam;

        public Snes()
        {
            mRam = ReadSnesRam();
        }

        public byte U8(int addr)
        {
            Debug.Assert(RamBase <= addr && addr < RamBase + RamSize);
            return mRam[addr - RamBase];
        }

        public ushort U16(int addr)
        {
            Debug.Assert(RamBase <= addr && addr < RamBase + RamSize - 1);
            return ToU16(mRam.Skip(addr - RamBase).Take(2).ToArray());
        }

        public uint U32(int addr)
        {
            Debug.Assert(RamBase <= addr && addr < RamBase + RamSize - 1);
            return ToU32(mRam.Skip(addr - RamBase).Take(4).ToArray());
        }

        static ushort ToU16(byte[] data)
        {
            return (ushort)(data[1] << 8 | data[0]);
        }

        static uint ToU32(byte[] data)
        {
            return (uint)(data[3] << 24 | data[2] << 16 | data[1] << 8 | data[0]);
        }

        static byte[] ReadSnesRam()
        {
            var process = SearchProcess("snes9x.exe");
            IntPtr aPtr = OpenProcess(ProcessAccessFlags.PROCESS_VM_READ, false, process.Id);

            // Snes9X v1.53 (from MECC)
            // @$73815C,$2000,$306000
            // @$738154,$20000,$7E0000

            byte[] data = ReadMemory(4, new IntPtr(0x738154), aPtr, process);
            int baseAddress = (int)ToU32(data);

            /*
                $7E 	$0000-$1FFF 	LowRAM 	
	                    $2000-$7FFF 	HighRAM 	
	                    $8000-$FFFF 	Extended RAM 	
                $7F 	$0000-$FFFF 	Extended RAM
            */
            data = ReadMemory(0x20000, new IntPtr(baseAddress), aPtr, process);
            if (data == null)
                MessageBox.Show("Can't read memory");

            return data;

            /*
                        int BlockSize = 1024 * 128;
                        int offset = baseAddress;

                        while (true)
                        {
                            data = ReadMemory(BlockSize, new IntPtr(offset), aPtr, process);
                            if (data == null)
                                break;

                            for (int i = 0; i < BlockSize - 6; i++)
                            {
                                if (data[i] == 60 &&
                                    data[i + 1] == 59 &&
                                    data[i + 2] == 128 &&
                                    data[i + 3] == 65 &&
                                    data[i + 4] == 0 &&
                                    data[i + 5] == 12)
                                {
                                    int addr = offset + i;
                                    addr -= 0x25;
                                    MessageBox.Show(string.Format("found! {0,0:X8}", addr));

                                    // 33FAB31A
                                }

                                offset += BlockSize;
                            }
                        }
            */
        }

        static Process SearchProcess(String pTargetExePath)
        {
            Process[] aAllProcessArray = Process.GetProcesses();

            // 全プロセスに対して繰り返し。
            // フルパスを取得して判定する。
            foreach (Process aProcess in aAllProcessArray)
            {
                String aProcessFilePath = "";

                // プロセスの中にはフルパスを取得できないものがあるため、例外が発生しうる。
                try
                {
                    aProcessFilePath = aProcess.MainModule.FileName;
                }
                catch (Win32Exception)
                {
                    continue;
                }
                catch (InvalidOperationException)
                {
                    continue;
                }

                if (aProcessFilePath.EndsWith(pTargetExePath))
                {
                    return aProcess;
                }
            }
            MessageBox.Show("見つからないし…");
            return null;
        }

        /**
         * やや簡易リードメモリ。
         * @param pByte 読み取りたいバイト数
         * @param pOffset オフセット
         * @param pOpenedProcess OpenProcessの戻り値
         * @return 取得したバイト配列
         */
        static byte[] ReadMemory(int pByte, IntPtr pOffset, IntPtr pOpenedProcess, Process process)
        {
            byte[] aResultArray = new byte[pByte];

            int aGetByte;
            IntPtr aTarget;

            // エントリーポイントアドレスと引数のオフセットを加算したアドレスにあるデータを取得する。
            // エントリーポイントじゃなくてベースアドレスがいいなら
            // aMyProcess.MainModule.BaseAddress.ToInt32()を指定。
            aTarget = IntPtr.Add(pOffset, 0); // process.MainModule.EntryPointAddress.ToInt32()

            // HANDLE hProcess,             // プロセスのハンドル
            // LPCVOID lpBaseAddress,       // 読み取り開始アドレス
            // LPVOID lpBuffer,             // データを格納するバッファ
            // DWORD nSize,                 // 読み取りたいバイト数
            // LPDWORD lpNumberOfBytesRead  // 読み取ったバイト数
            // ReadProcessMemoryは失敗するとFALSEを返す。
            if (!ReadProcessMemory(pOpenedProcess, aTarget, aResultArray, new UIntPtr((uint)pByte), out aGetByte))
            {
                // MessageBox.Show("うまくいかないし…");
                return null;
            }

            return aResultArray;
        }

        // 出現するフラグを列挙
        enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            PROCESS_VM_READ = 0x10,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, out int lpNumberOfBytesRead);
    }
}
