using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSVreader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSV_Controller : ControllerBase
    {
        private const string Path = @"C:\Users\wikto\OneDrive\Desktop\ExcelBDA.csv";

        [HttpGet("read-csv")]
        public IActionResult ReadCsv()
        {
            if (!System.IO.File.Exists(Path))
            {
                return NotFound("CSV file not found at the specified path.");
            }

            try
            {
                // Use Csvreader to read the file
                var csvReader = new Csvreader();
                var csvData = csvReader.Read(Path);

                return Ok(csvData);
            }
            catch (IOException ex)
            {
                return StatusCode(500, $"File access error: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public class Csvreader
        {
            private const string Separator = ",";

            public Csvdata Read(string path)
            {
                using var streamReader = new StreamReader(path);
                var columns = streamReader.ReadLine()?.Split(Separator);
                if (columns == null)
                    throw new InvalidDataException("CSV file is empty or invalid.");

                var rows = new List<string[]>();

                while (!streamReader.EndOfStream)
                {
                    var cellInRow = streamReader.ReadLine()?.Split(Separator);
                    if (cellInRow != null)
                    {
                        rows.Add(cellInRow);
                    }
                }

                return new Csvdata(columns, rows);
            }
        }
        public class Csvdata
        {
            public string?[] Columns { get; }
            public IEnumerable<string[]> Rows { get; }

            public Csvdata(string?[] columns, IEnumerable<string[]> rows)
            {
                Columns = columns;
                Rows = rows;
            }
        }

    }


}
