using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackFileEditorStandalone
{
    class Program
    {
        private static readonly string FILES_CURRENT_DIR = Directory.GetCurrentDirectory() + "\\Files\\";
        private static readonly string PACK_FILE = Directory.GetCurrentDirectory() + @"\Resources\unit_template.pack";

        static UInt32 VERSION_MARKER = BitConverter.ToUInt32(new byte[] { 0xFC, 0xFD, 0xFE, 0xFF }, 0);
        static UInt32 GUID_MARKER = BitConverter.ToUInt32(new byte[] { 0xFD, 0xFE, 0xFC, 0xFF }, 0);

        private static PackFile currentPackFile;        // Full pack file full of tables 

        static void Main(string[] args)
        {
            Console.Out.WriteLine("--- BASIC AS FUCK PACK FILE EDITOR ---");
            Console.Out.WriteLine("Loading file from path: " + FILES_CURRENT_DIR);

            // Load pack file from system into memory
            var codec = new PackFileCodec();
            currentPackFile = codec.Open(PACK_FILE);

            DBTypeMap.InitializeAllTypeInfos(FILES_CURRENT_DIR);



            // Get datatable for buildings_units_allowed_tables ->> template_data__core
            DataTable landUnitsTable = GetDataTable("land_units_table");



            // Save the currently edited pack file back out to system
            SaveAsFile("custom_unit.pack");
        }

        private static void SaveAsFile(string filename)
        {
            string destDir = Directory.GetCurrentDirectory();

            if (!Directory.Exists(destDir + "\\generatedTemplates"))
            {
                Directory.CreateDirectory(destDir + "\\generatedTemplates");
            }
            string tempFile = destDir + "\\generatedTemplates\\" + filename;
            new PackFileCodec().WriteToFile(tempFile, currentPackFile);
        }

        private static DataTable GetDataTable(String tableKey)
        {
            PackedFile currentPackedFile = currentPackFile.Files[0];
            PackedFile replacementPackedFile = currentPackedFile;

            DBFile test = PackedFileDbCodec.Decode(currentPackedFile);

            test.Entries[0][0].Value = "dicks and or butts";

            Encode(new MemoryStream(replacementPackedFile.Data), test);

            //currentPackFile.Files[0] = currentPackedFile;

            //currentPackFile.Files[0] = null;

            currentPackFile.Files.Remove(currentPackedFile);
            currentPackFile.Files.Add(replacementPackedFile);

            return new DataTable();
        }

        public static void Encode(Stream stream, DBFile file)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            file.Header.EntryCount = (uint)file.Entries.Count;
            WriteHeader(writer, file.Header);
            file.Entries.ForEach(delegate (DBRow e) { WriteEntry(writer, e); });
            writer.Flush();
        }
        /*
         * Encode db file to memory and return it as a byte array.
         */
        public static byte[] Encode(DBFile file)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Encode(stream, file);
                return stream.ToArray();
            }
        }

        private static void WriteEntry(BinaryWriter writer, List<FieldInstance> fields)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                try
                {
                    FieldInstance field = fields[i];
                    field.Encode(writer);
                }
                catch (Exception x)
                {
                    Console.WriteLine(x);
                    throw x;
                }
            }
        }

        public static void WriteHeader(BinaryWriter writer, DBFileHeader header)
        {
            if (header.GUID != "")
            {
                writer.Write(GUID_MARKER);
                IOFunctions.WriteCAString(writer, header.GUID, Encoding.Unicode);
            }
            if (header.Version != 0)
            {
                writer.Write(VERSION_MARKER);
                writer.Write(header.Version);
            }
            writer.Write((byte)1);
            writer.Write(header.EntryCount);
        }
    }
}
