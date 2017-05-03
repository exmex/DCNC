using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Shared.Util
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class TdfReader
    {
        public readonly SBitmap Bitmap;
        public int[] DataTable;
        public readonly SHeader Header;
        public byte[] ResTable;
        public readonly SVersion Version;

        public TdfReader()
        {
            Bitmap = new SBitmap();
            Version = new SVersion();
            Header = new SHeader
            {
                Date = new SDate()
            };
        }

        public bool Load(string fileName)
        {
            if (File.Exists(fileName) && new FileInfo(fileName).Extension == ".tdf")
            {
                using (var reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    Bitmap.BfType = reader.ReadInt16();
                    Bitmap.BfSize = reader.ReadUInt32();
                    Bitmap.BfReserved1 = reader.ReadInt16();
                    Bitmap.BfReserved2 = reader.ReadInt16();
                    Bitmap.BfOffBits = reader.ReadUInt32();
                    if (Bitmap.BfType == 19778)
                        reader.ReadBytes((int) Bitmap.BfSize - 14);

                    Version.Major = reader.ReadUInt16();
                    Version.Minor = reader.ReadUInt16();
                    if (Version.Major != 1 && Version.Minor != 4)
                    {
                        Log.Error($"Invalid file version. Expected 1.4 got {Version.Major}.{Version.Minor}");
                        return false;
                    }

                    Header.Date.Year = reader.ReadUInt16();
                    Header.Date.Month = reader.ReadChar();
                    Header.Date.Day = reader.ReadChar();

                    Header.Flag = reader.ReadUInt32();
                    Header.Offset = reader.ReadUInt32();
                    Header.Col = reader.ReadUInt32();
                    Header.Row = reader.ReadUInt32();

                    DataTable = new int[Header.Col * Header.Row];
                    for (var i = 0; i < Header.Col * Header.Row; i++)
                        DataTable[i] = reader.ReadInt32();

                    ResTable = new byte[Header.Offset - (Header.Col * 4 * Header.Row + 24)];
                    for (long i = 0; i < Header.Offset - (Header.Col * 4 * Header.Row + 24); i++)
                        ResTable[i] = reader.ReadByte();

                    Debug.WriteLine("Loaded TDF Version: {0:D}.{1:D} ({2:D}/{3:D}/{4:D})", (int) Version.Major,
                        Version.Minor, (short) Header.Date.Month, (short) Header.Date.Day, Header.Date.Year);
                    Debug.WriteLine("File contains {0} Rows and {1} Columns", Header.Row, Header.Col);

                    Debug.WriteLine("DataTable size: {0:D}, ResTable size: {1:D}", DataTable.Length, ResTable.Length);

                    return true;
                }
            }
            Log.Error($"File {fileName} either does not exist or is not a valid TDF file.");
            return false;
        }

        public class SBitmap
        {
            public uint BfOffBits;
            public short BfReserved1;
            public short BfReserved2;
            public uint BfSize;
            public short BfType;
        }

        public class SVersion
        {
            public ushort Major;
            public ushort Minor;
        }

        public class SDate
        {
            public char Day;
            public char Month;
            public ushort Year;
        }

        public class SHeader
        {
            public uint Col;
            public SDate Date;
            public uint Flag;
            public uint Offset;
            public uint Row;
        }
    }
}