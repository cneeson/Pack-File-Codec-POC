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
        private const String INPUT_FILE_PATH = @"c:\users\craig\documents\visual studio 2017\Projects\PackFileEditorStandalone\PackFileEditorStandalone\Resources\unit_template.pack";

        private static PackFile currentPackFile;        // Full pack file full of tables 

        static void Main(string[] args)
        {
            Console.Out.WriteLine("--- BASIC AS FUCK PACK FILE EDITOR ---");
            Console.Out.WriteLine("Loading file from path: " + INPUT_FILE_PATH);

            // Load pack file from system into memory
            var codec = new PackFileCodec();
            currentPackFile = codec.Open(INPUT_FILE_PATH);



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


            // NEXT STEP, WORK OUT WAY TO GET SCHEMA.XMLs IN BECAUSE THAT IS WHY THERE IS CURRENTLY
            // NO DBFile BEING PRODUCED
            DBFile test = PackedFileDbCodec.Decode(currentPackedFile);

            return new DataTable();
        }
    }
}
