using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TGDBHashTool.Models.Data;
using TGDBHashTool.Models.SimpleHashes;

namespace TGDBHashTool
{
    static class Program
    {
        public static string XmlFilename { get; set; } = "../../data.xml";
        public static DataCollection Collection;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var xmlIndex = Array.IndexOf(args, "--xml");
            var csIndex = Array.IndexOf(args, "--cs");
            var csOptionsIndex = Array.IndexOf(args, "--cs-opts");

            

            if (xmlIndex != -1)
            {
                XmlFilename = args[xmlIndex + 1];
            }

            if (File.Exists(XmlFilename))
            {
                using (var file = File.OpenRead(XmlFilename))
                {
                    Collection = Xml.Deserialize<DataCollection>(file);
                }
            }
            else
            {
                Collection = new DataCollection();
            }
            
            if (csIndex != -1 && csOptionsIndex != -1)
            {
                var csOptions = args[csOptionsIndex + 1].Split(',');
                var cs = Cs.Generate(csOptions[0], csOptions[1], csOptions[2], Collection);
                File.WriteAllText(args[csIndex + 1], cs);
            }
            else
            {
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new MainForm());

                using (var file = File.Create(XmlFilename))
                {
                    Collection.Groups = Collection.Groups.OrderBy(e => e.Name).ToList();
                    foreach (var group in Collection.Groups)
                    {
                        group.Games = group.Games.OrderBy(e => e.Name).ToList();
                        foreach (var game in group.Games)
                        {
                            game.Roms = game.Roms.OrderBy(e => e.Name).ToList();
                        }
                    }

                    Xml.Serialize<DataCollection>(file, Collection);
                }

                var simple = new SimpleHashes();
                foreach (var entry in Cs.GetHashDictionary(Collection).OrderBy(e => e.Key).Where(e => e.Value.Count > 0))
                {
                    simple.Hashes.Add(new SimpleHash()
                    {
                        Crc32 = entry.Key,
                        TgdbId = entry.Value
                    });
                }

                using (var file = File.Create("simple.xml"))
                {
                    Xml.Serialize<SimpleHashes>(file, simple);
                }
            }
        }
    }
}
