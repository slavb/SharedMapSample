using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using SQLite;
#if ANDROID
using Volcanoes;
#endif

namespace NuclearPlants
{
	public class NuclearRepository
	{
		public NuclearRepository ()
		{
		}

		const string DatabaseName = "NuclearData.db";

		static string GetDatabasePath ()
		{
			#if ANDROID
			var documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			return documentsDirectory + "/" + DatabaseName;
			#else
			return DatabaseName;
			#endif
		}

		public static void EnsureDatabase ()
		{

			#if !ANDROID
			//For iOS not required?
			return;
			#else

			var stream = AssetsHelper.Assets.Open (DatabaseName);

			var databaseFileName = GetDatabasePath ();

			System.Diagnostics.Debug.WriteLine ("Database Path: " + databaseFileName);

			//Exit if old file exists
			if (File.Exists (databaseFileName) ) {
				System.Diagnostics.Debug.WriteLine ("Database already exists");
				return;
			}

			if (!File.Exists (databaseFileName)) {
				System.Diagnostics.Debug.WriteLine ("Copy database");

				//Save stream to file
				var fileStream = File.Create(databaseFileName);
				//stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(fileStream);
				fileStream.Close();
			}

			stream.Close ();

			#endif
		}

		static List<Nuclear> nuclear = new List<Nuclear>();

		public static List<Nuclear> Nuclear {
			get {
				return nuclear;
			}
			private set {
				nuclear = value;
			}
		}

		//TODO: Move to Shared???
		public static void LoadFromDatabase()
		{
			Debug.WriteLine ("LoadFromDatabase");

			if (nuclear.Count != 0) {
				return;
			}

			EnsureDatabase ();

			var start = DateTime.Now;

			var conn = new SQLiteConnection (GetDatabasePath ());

			nuclear = conn.Query<Nuclear>("select * from Nuclear");

            Debug.WriteLine("LoadFromDatabase completed: " + DateTime.Now.Subtract(start).ToString());
		}

		public static List<Nuclear> GetLocationByName (string text)
		{
            Debug.WriteLine("GetLocationByName");

			if (string.IsNullOrEmpty(text)) {
				return null;
			}

			var list = from e in nuclear
					where e.Name.StartsWith (text, StringComparison.OrdinalIgnoreCase)
			           select e;

			return list.ToList ();
		}
	}
}

