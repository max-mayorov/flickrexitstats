using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MetadataExtractor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Directory = MetadataExtractor.Directory;
using Formatting = Newtonsoft.Json.Formatting;

namespace exifstatsExtract
{
    public  class Program
    {
        public class Metadata
        {
            public string ObjectId { get; set; }
            public MetadataExtractor.Directory[] Directories { get; set; }
        }

        public  static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide path");
                return;
            }

            var path = args[0];
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            List<Metadata> metadata = new List<Metadata>();
            DateTime s = DateTime.Now;
            foreach (var file in directoryInfo.EnumerateFiles())
            {
                Console.WriteLine(file.FullName);
                metadata.Add(new Metadata
                {
                    ObjectId = file.FullName,
                    Directories = ImageMetadataReader.ReadMetadata(file.FullName).ToArray()
                });


                /*
                // Iterate over the data and print to System.out
                //
                // A Metadata object contains multiple Directory objects
                //
                foreach (var directory in metadata.Select(x => x))
                {

                    //
                    // Each Directory stores values in Tag objects
                    //
                    Console.WriteLine("\t" + directory.Name);
                    foreach (var tag in directory.Tags)
                    {
                        Console.WriteLine("\t\t" + tag.TagName + "\t\t\t" + tag.TagType + "\t|\t" + tag);
                    }
                }
                 * */
            }
            DateTime e = DateTime.Now;
            Debug.WriteLine("Elapsed {0} s", (e - s).TotalSeconds);
            Debug.WriteLine("Per file {0} ms", (e - s).TotalMilliseconds/metadata.Count);
            
            File.WriteAllText("metadata.json", JsonConvert.SerializeObject(metadata, Formatting.Indented));
        }
    }
}
