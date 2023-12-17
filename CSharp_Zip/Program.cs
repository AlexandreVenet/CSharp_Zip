using System.IO.Compression;

namespace CSharp_Zip
{
	internal class Program
	{
		private static string _cheminDossierSource = @".\DossierTest";
		private static string _cheminDossierSortie = @".\Extraction";
		private static string _cheminZip = @".\sortie.zip";
		private static string _cheminZipNew = @".\nouveau.zip";

		static void Main(string[] args)
		{
			Nettoyer();
			ZipperRepertoire();
			DezipperRepertoire();
			ManipulerTxt();
		}

		private static void Nettoyer()
		{
			if (File.Exists(_cheminZip))
			{
				File.Delete(_cheminZip);
			}

			if (File.Exists(_cheminZipNew))
			{
				File.Delete(_cheminZipNew);
			}

			if (Directory.Exists(_cheminDossierSortie))
			{
				Directory.Delete(_cheminDossierSortie, true);
			}
		}

		private static void ZipperRepertoire()
		{
			ZipFile.CreateFromDirectory(_cheminDossierSource, _cheminZip);
		}

		private static void DezipperRepertoire()
		{
			ZipFile.ExtractToDirectory(_cheminZip, _cheminDossierSortie, true); // avec remplacement si existant
		}

		private static void ManipulerTxt()
		{
			string cheminExtraction = Path.Combine(_cheminDossierSortie, "Nouveau.txt");

			// Extraire le fichier txt du .zip

			using (ZipArchive zip = ZipFile.OpenRead(_cheminZip))
			{
				ZipArchiveEntry? entry = zip.GetEntry("Fichier.txt");
				if (entry == null)
				{
					Console.WriteLine("Le fichier n'existe pas dans l'archive.");
					return;
				}
				entry.ExtractToFile(cheminExtraction, true);
			}

			// Ajouter du texte

			using (StreamWriter sw = File.AppendText(cheminExtraction))
			{
				sw.WriteLine();
				sw.WriteLine("-----");
				sw.WriteLine("Texte supplémentaire");
				sw.WriteLine("-----");
			}

			// Créer une archive zip et y mettre le nouveau fichier

			using (FileStream sw = File.Create(_cheminZipNew))
			{
				using (ZipArchive zip = new(sw, ZipArchiveMode.Update))
				{
					ZipArchiveEntry entry = zip.CreateEntryFromFile(cheminExtraction, "Nouveau Fichier.txt");
				}
			}

		}
	}
}
