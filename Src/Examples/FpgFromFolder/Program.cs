using System;
using System.Collections.Generic;

namespace FpgFromFolder
{
    class Program
    {
        static void Main ( string[] args )
        {

            var thisAppPath = System.Reflection.Assembly.GetExecutingAssembly ().Location;
            var folderWithImages = System.IO.Path.Combine ( System.IO.Path.GetDirectoryName ( thisAppPath ), "TestFpg" );

            var folderName = new System.IO.DirectoryInfo ( folderWithImages ).Name;
            var fpgFileName = $"{folderName}.fpg";

            var mapCount = 0;
            var fpgMaker = new Files2FpgMaker ();
            fpgMaker.FileAdded += ( s, a ) =>
            {
                Console.WriteLine ( $"Adding file '{a.Path}'" );
                mapCount++;
            };
            fpgMaker.FileSkipped += ( s, a ) => Console.WriteLine ( $"Skipping file '{a.Path}'" );

            try
            {
                fpgMaker.Make ( EnumerateFiles ( folderWithImages, "*.png" ), fpgFileName );
            }
            catch ( Exception e )
            {
                Console.Error.WriteLine ( e );
                Console.ReadKey ();
                return;
            }

            Console.WriteLine ( $"Fpg '{fpgFileName}' created successfully with {mapCount} maps!" );
            Console.ReadKey ();
        }

        private static IEnumerable<string> EnumerateFiles ( string path, string searchPattern )
        {
            return System.IO.Directory.EnumerateFiles ( path, searchPattern );
        }
    }
}
