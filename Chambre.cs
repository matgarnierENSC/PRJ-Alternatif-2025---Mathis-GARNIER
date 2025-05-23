public class Chambre
{
    public int Numero { get; }
    public int Luminosite { get; set; }
    public string Nom { get; }
    public List<Mur> Murs { get; }
    public List<Oeuvre> Oeuvres { get; }

    public Chambre(string nom, int numero, int luminosite, List<Mur> murs)
    {
        Nom = nom;
        Numero = numero;
        Luminosite = luminosite;
        Murs = murs;
        Oeuvres = new List<Oeuvre>();
    }

    // Tente d'ajouter une œuvre à une zone compatible, après vérification
    public void AjouterOeuvre(Oeuvre oeuvre)
    {
        if (!IsCompatible(oeuvre))
            throw new InvalidOperationException("Œuvre incompatible.");

        var zonesPossibles = new List<(Mur murChoisi, Zone zoneChoisie)>();

        // Recherche des zones compatibles
        foreach (var mur in Murs)
        {
            foreach (var zone in mur.Zones)
            {
                if (zone.EstLibre && zone.EstCompatible(oeuvre))
                    zonesPossibles.Add((mur, zone));
            }
        }

        if (zonesPossibles.Count == 0)
            throw new InvalidOperationException("Aucune zone compatible disponible.");

        (Mur murChoisi, Zone zoneChoisie) selection;

        // Sélection automatique si une seule zone compatible
        if (zonesPossibles.Count == 1)
        {
            selection = zonesPossibles[0];
            Console.WriteLine("✅ Zone automatiquement sélectionnée.");
        }
        else
        {
            // Sinon, demander à l'utilisateur de choisir une zone parmi plusieurs
            Console.WriteLine("\n🧐 Plusieurs zones compatibles trouvées. Veuillez choisir :\n");

            for (int i = 0; i < zonesPossibles.Count; i++)
            {
                var (mur, zone) = zonesPossibles[i];
                int murIndex = Murs.IndexOf(mur);
                int zoneIndex = mur.Zones.IndexOf(zone);

                string type = zone is ZoneMurale ? "🖼️ ZoneMurale" :
                              zone is Vitrine ? "🏺 Vitrine" : "❔ Inconnue";
                string extra = zone is Vitrine vitrine ? $" | capacité : {vitrine.CapaciteMax}kg" : "";

                Console.WriteLine($"[{i}] {type} sur mur {murIndex + 1}, zone {zoneIndex + 1} ({zone.Largeur}x{zone.Hauteur}){extra} | chambre #{Numero}");
            }

            int choix;
            do
            {
                Console.Write("\nVotre choix (entrez un numéro valide) : ");
            }
            while (!int.TryParse(Console.ReadLine(), out choix) || choix < 0 || choix >= zonesPossibles.Count);

            selection = zonesPossibles[choix];
        }

        // Marquer la zone comme occupée et enregistrer l’œuvre
        selection.zoneChoisie.Occuper();
        oeuvre.Zone = selection.zoneChoisie;
        Oeuvres.Add(oeuvre);
    }

    // Vérifie si la chambre peut accueillir l’œuvre (sans afficher d'erreur)
    public bool IsCompatible(Oeuvre oeuvre)
    {
        if (oeuvre is Toile toile)
        {
            bool bonneLumiere = toile.ForteLumiere == (Luminosite > 5);
            if (!bonneLumiere) return false;

            CouleurMur couleurCible = toile.CouleurChaude ? CouleurMur.Chaude : CouleurMur.Froide;

            foreach (var mur in Murs)
            {
                if (mur.Couleur != couleurCible) continue;

                foreach (var zone in mur.Zones)
                {
                    if (zone.EstLibre && zone is ZoneMurale && zone.EstCompatible(toile))
                        return true;
                }
            }

            return false;
        }

        if (oeuvre is Antiquite)
        {
            foreach (var mur in Murs)
            {
                foreach (var zone in mur.Zones)
                {
                    if (zone.EstLibre && zone is Vitrine vitrine && vitrine.EstCompatible(oeuvre))
                        return true;
                }
            }

            return false;
        }

        return false;
    }

    // Version "débogage" de la compatibilité : affiche les raisons du refus si besoin
    public bool IsCompatibleDebug(Oeuvre oeuvre)
    {
        Console.WriteLine($"\n🔎 Chambre « {Nom} » (#{Numero})");

        if (oeuvre is Toile toile)
        {
            bool bonneLumiere = toile.ForteLumiere == (Luminosite > 5);
            bool couleurOK = false;
            bool tailleOK = false;

            foreach (var mur in Murs)
            {
                if (mur.Couleur == (toile.CouleurChaude ? CouleurMur.Chaude : CouleurMur.Froide))
                    couleurOK = true;

                foreach (var zone in mur.Zones)
                {
                    if (zone is ZoneMurale && zone.EstLibre &&
                        zone.Largeur >= toile.Largeur + 40 &&
                        zone.Hauteur >= toile.Hauteur + 40)
                        tailleOK = true;
                }
            }

            if (!bonneLumiere) Console.WriteLine("❌ Lumière incompatible");
            if (!couleurOK) Console.WriteLine("❌ Couleur du mur incompatible");
            if (!tailleOK) Console.WriteLine("❌ Toile trop grande pour les zones murales");

            return bonneLumiere && couleurOK && tailleOK;
        }

        if (oeuvre is Antiquite antiq)
        {
            bool vitrineCompatible = false;
            bool vitrineExistante = false;
            bool vitrineTropPetite = false;

            foreach (var mur in Murs)
            {
                foreach (var zone in mur.Zones)
                {
                    if (zone is Vitrine vitrine && zone.EstLibre)
                    {
                        vitrineExistante = true;

                        if (vitrine.CapaciteMax >= antiq.Poids)
                            vitrineCompatible = true;
                        else
                            vitrineTropPetite = true;
                    }
                }
            }

            if (!vitrineCompatible)
            {
                if (!vitrineExistante)
                    Console.WriteLine("❌ Aucune vitrine disponible (occupée ou absente)");
                else if (vitrineTropPetite)
                    Console.WriteLine("❌ Trop lourd pour les vitrines de cette chambre");
            }

            return vitrineCompatible;
        }

        Console.WriteLine("❌ Type d’œuvre non reconnu.");
        return false;
    }
}
