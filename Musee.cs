public class Musee
{
    public string Nom { get; }
    public List<Chambre> Chambres { get; }

    // Initialise le musée avec un nom et une liste vide de chambres
    public Musee(string nom)
    {
        Nom = nom;
        Chambres = new List<Chambre>();
    }

    // Ajoute une chambre au musée
    public void AjouterChambre(Chambre chambre)
    {
        Chambres.Add(chambre);
    }

    // Tente d'ajouter une œuvre dans la première chambre compatible
    public bool AjouterOeuvre(Oeuvre oeuvre)
    {
        foreach (var chambre in Chambres)
        {
            try
            {
                chambre.AjouterOeuvre(oeuvre);
                return true;
            }
            catch (InvalidOperationException)
            {
                continue;
            }
        }
        return false;
    }

    // Affiche une vue graphique simplifiée du musée en console
    public void AfficherCarte()
    {
        Console.Clear();
        Console.WriteLine();

        foreach (var chambre in Chambres)
        {
            int chauds = 0, froids = 0;
            foreach (var mur in chambre.Murs)
            {
                if (mur.Couleur == CouleurMur.Chaude) chauds++;
                else if (mur.Couleur == CouleurMur.Froide) froids++;
            }
            string icone = chauds >= froids ? "🔥" : "❄️";

            Console.WriteLine($"\nChambre {chambre.Nom} {icone}   Luminosité : {chambre.Luminosite}");

            List<Zone>[] murs = new List<Zone>[4];
            for (int i = 0; i < 4; i++)
                murs[i] = i < chambre.Murs.Count ? chambre.Murs[i].Zones : new List<Zone>();

            Console.Write("    ┌");
            for (int i = 0; i < 3; i++) Console.Write(GetSymbole(murs[0], i, chambre));
            Console.WriteLine("┐");

            for (int i = 0; i < 3; i++)
            {
                string gauche = GetSymbole(murs[3], i, chambre);
                string droite = GetSymbole(murs[1], i, chambre);
                Console.WriteLine($"    {gauche}       {droite} │");
            }

            Console.Write("    └");
            for (int i = 0; i < 3; i++) Console.Write(GetSymbole(murs[2], i, chambre));
            Console.WriteLine("┘");

            Console.WriteLine(new string('═', 30));
        }

        Console.WriteLine("\n🔎 Légende : 🟩 libre | 🖼️ toile | 🏺 antiquité | 🟥 occupée | ⬛ zone manquante");
    }

    // Retourne le symbole graphique correspondant à une zone du mur
    private string GetSymbole(List<Zone> zones, int i, Chambre chambre)
    {
        if (i >= zones.Count) return " ";
        Zone zone = zones[i];

        if (zone.EstLibre) return "🟩";

        foreach (var oeuvre in chambre.Oeuvres)
        {
            if (oeuvre.Zone == zone)
            {
                if (oeuvre is Toile) return "🖼️";
                if (oeuvre is Antiquite) return "🏺";
            }
        }

        return "🟥";
    }

    // Affiche un résumé textuel complet de toutes les chambres et œuvres
    public override string ToString()
    {
        int totalOeuvres = 0;
        int totalZonesLibres = 0;
        int chambresAvecPlace = 0;

        string res = $"🎨 Bienvenue au {Nom} !\n";
        res += new string('═', 40) + "\n";

        foreach (var chambre in Chambres)
        {
            int chauds = 0, froids = 0, zonesLibres = 0;

            foreach (var mur in chambre.Murs)
            {
                if (mur.Couleur == CouleurMur.Chaude) chauds++;
                if (mur.Couleur == CouleurMur.Froide) froids++;

                foreach (var zone in mur.Zones)
                {
                    if (zone.EstLibre) zonesLibres++;
                }
            }

            totalZonesLibres += zonesLibres;
            if (zonesLibres > 0) chambresAvecPlace++;

            string temperature = chauds >= froids ? "🔥 Chauds" : "❄️ Froids";
            res += $"🏛️ Chambre « {chambre.Nom} » (#{chambre.Numero}) | Luminosité : {chambre.Luminosite} | Murs : {temperature}\n";

            foreach (var mur in chambre.Murs)
            {
                res += $"  🧱 Mur {mur.Couleur} avec {mur.Zones.Count} zones :\n";
                foreach (var zone in mur.Zones)
                {
                    string etat = zone.EstLibre ? "🟩 libre" : "🟥 occupée";
                    string type = zone is ZoneMurale ? "🖼️ ZoneMurale" :
                                  zone is Vitrine ? "🏺 Vitrine" : "❔ Zone inconnue";
                    res += $"     - {type} ({zone.Largeur}x{zone.Hauteur}) → {etat}";
                    if (zone is Vitrine vitrine)
                        res += $" | capacité max : {vitrine.CapaciteMax}kg";
                    res += "\n";
                }
            }

            if (chambre.Oeuvres.Count == 0)
                res += "  ⚠️ Aucune œuvre exposée\n";
            else
            {
                res += "  📦 Œuvres exposées :\n";
                foreach (var oeuvre in chambre.Oeuvres)
                {
                    totalOeuvres++;
                    string icone = oeuvre is Toile ? "🖼️" :
                                   oeuvre is Antiquite ? "🏺" : "❔";
                    res += $"     {icone} {oeuvre}\n";
                }
            }

            res += new string('-', 40) + "\n";
        }

        res += $"📊 Bilan du musée :\n";
        res += $"   → Total œuvres exposées : {totalOeuvres}\n";
        res += $"   → Zones disponibles : {totalZonesLibres}\n";
        res += $"   → Chambres avec de la place : {chambresAvecPlace}/{Chambres.Count}\n";
        res += new string('═', 40) + "\n";

        return res;
    }

    // Affiche tous les détails d’une chambre spécifique
    public string AfficherChambre(int numero)
    {
        foreach (var chambre in Chambres)
        {
            if (chambre.Numero == numero)
            {
                string recap = "";
                int chauds = 0, froids = 0;

                foreach (var mur in chambre.Murs)
                {
                    if (mur.Couleur == CouleurMur.Chaude) chauds++;
                    if (mur.Couleur == CouleurMur.Froide) froids++;
                }

                string temperature = chauds >= froids ? "🔥 Chauds" : "❄️ Froids";
                recap += $"🏛️ Chambre « {chambre.Nom} » (#{chambre.Numero}) | Luminosité : {chambre.Luminosite} | Murs : {temperature}\n";

                foreach (var mur in chambre.Murs)
                {
                    recap += $"  🧱 Mur {mur.Couleur} avec {mur.Zones.Count} zones :\n";
                    foreach (var zone in mur.Zones)
                    {
                        string etat = zone.EstLibre ? "🟩 libre" : "🟥 occupée";
                        string type = zone is ZoneMurale ? "🖼️ ZoneMurale" :
                                      zone is Vitrine ? "🏺 Vitrine" : "❔ Zone inconnue";
                        recap += $"     - {type} ({zone.Largeur}x{zone.Hauteur}) → {etat}";
                        if (zone is Vitrine vitrine)
                            recap += $" | capacité max : {vitrine.CapaciteMax}kg";
                        recap += "\n";
                    }
                }

                if (chambre.Oeuvres.Count == 0)
                    recap += "  ⚠️ Aucune œuvre exposée\n";
                else
                {
                    recap += "  📦 Œuvres exposées :\n";
                    foreach (var oeuvre in chambre.Oeuvres)
                    {
                        string icone = oeuvre is Toile ? "🖼️" :
                                       oeuvre is Antiquite ? "🏺" : "❔";
                        recap += $"     {icone} {oeuvre}\n";
                    }
                }

                return recap;
            }
        }

        return "⛔ Chambre non trouvée.";
    }
}
