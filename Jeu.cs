public class Jeu
{
    private Musee musee;

    // Initialise le musée avec ses chambres, murs et zones
    public void InitialiserMusee()
    {
        musee = new Musee("Musée Plaisir");

        // Méthode locale pour créer un mur avec 2 zones murales et 1 vitrine
        Mur CreerMur(CouleurMur couleur, int vitrineCapacite)
        {
            return new Mur(couleur, new List<Zone>
            {
                new ZoneMurale(150, 120),
                new ZoneMurale(140, 110),
                new Vitrine(vitrineCapacite) { Largeur = 90, Hauteur = 90 }
            });
        }

        // Création des 4 chambres avec des caractéristiques différentes
        var chambre1 = new Chambre("Été", 1, 8, new List<Mur> {
            CreerMur(CouleurMur.Chaude, 100),
            CreerMur(CouleurMur.Chaude, 100),
            CreerMur(CouleurMur.Chaude, 100),
            CreerMur(CouleurMur.Chaude, 100)
        });

        var chambre2 = new Chambre("Hiver", 2, 8, new List<Mur> {
            CreerMur(CouleurMur.Froide, 600),
            CreerMur(CouleurMur.Froide, 600),
            CreerMur(CouleurMur.Froide, 600),
            CreerMur(CouleurMur.Froide, 600)
        });

        var chambre3 = new Chambre("Printemps", 3, 2, new List<Mur> {
            CreerMur(CouleurMur.Chaude, 50),
            CreerMur(CouleurMur.Chaude, 50),
            CreerMur(CouleurMur.Chaude, 50),
            CreerMur(CouleurMur.Chaude, 50)
        });

        var chambre4 = new Chambre("Automne", 4, 2, new List<Mur> {
            CreerMur(CouleurMur.Froide, 300),
            CreerMur(CouleurMur.Froide, 300),
            CreerMur(CouleurMur.Froide, 300),
            CreerMur(CouleurMur.Froide, 300)
        });

        // Ajout des chambres au musée
        musee.AjouterChambre(chambre1);
        musee.AjouterChambre(chambre2);
        musee.AjouterChambre(chambre3);
        musee.AjouterChambre(chambre4);

        Console.Clear(); // Nettoyage de la console après l'initialisation
    }

    // Lancement du programme principal avec le menu
    public void Lancer()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("🎨 Musée Plaisir — Menu principal");
            Console.WriteLine("═════════════════════════════════");
            Console.WriteLine("1 - Ajouter une œuvre");
            Console.WriteLine("2 - Voir carte du musée");
            Console.WriteLine("3 - Voir une chambre");
            Console.WriteLine("4 - Voir le récap complet");
            Console.WriteLine("5 - Quitter");
            Console.WriteLine("═════════════════════════════════");
            Console.Write("Votre choix : ");
            string choix = Console.ReadLine();

            Console.Clear();

            switch (choix)
            {
                case "1":
                    AjouterOeuvre();
                    break;

                case "2":
                    Console.Clear();
                    musee.AfficherCarte();
                    Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                    Console.ReadKey();
                    break;

                case "3":
                    Console.Write("Entrez le numéro de la chambre (1 à 4) : ");
                    if (int.TryParse(Console.ReadLine(), out int num))
                    {
                        Console.Clear();
                        Console.WriteLine($"--- Détail de la chambre #{num} ---\n");
                        Console.WriteLine(musee.AfficherChambre(num));
                    }
                    else
                    {
                        Console.WriteLine("⛔ Numéro invalide.");
                    }
                    PauseAvantRetour();
                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine("--- Composition complète du musée ---\n");
                    Console.WriteLine(musee.ToString());
                    PauseAvantRetour();
                    break;

                case "5":
                    Console.WriteLine("👋 Merci d’avoir visité le musée !");
                    return;

                default:
                    Console.WriteLine("⛔ Choix invalide. Veuillez entrer un chiffre entre 1 et 5.");
                    PauseAvantRetour();
                    break;
            }
        }
    }

    // Ajout d'une œuvre via les inputs utilisateur
    private void AjouterOeuvre()
    {
        Console.Clear();
        Console.WriteLine("➕ Ajouter une œuvre au musée");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("Quel type d’œuvre ? (1 = Toile, 2 = Antiquité)");
        string choix = Console.ReadLine();

        Console.Write("Titre de l’œuvre : ");
        string titre = Console.ReadLine();

        Oeuvre oeuvre = null;
        bool placee = false;

        // Création de la bonne instance selon le type
        if (choix == "1")
        {
            int largeur = DemanderEntier("Largeur : ");
            int hauteur = DemanderEntier("Hauteur : ");

            Console.Write("Supporte la lumière forte ? (o/n) : ");
            bool forteLumiere = Console.ReadLine().ToLower() == "o";

            Console.Write("Couleurs chaudes ? (o/n) : ");
            bool couleursChaudes = Console.ReadLine().ToLower() == "o";

            oeuvre = new Toile(titre, largeur, hauteur, forteLumiere, couleursChaudes);
        }
        else if (choix == "2")
        {
            int largeur = DemanderEntier("Largeur : ");
            int hauteur = DemanderEntier("Hauteur : ");
            int poids = DemanderEntier("Poids en kg : ");

            oeuvre = new Antiquite(titre, largeur, hauteur, poids);
        }
        else
        {
            Console.WriteLine("⛔ Type d’œuvre invalide.");
            PauseAvantRetour();
            return;
        }

        // Tentative de placement dans une chambre
        placee = musee.AjouterOeuvre(oeuvre);

        Console.Clear();

        if (placee)
        {
            musee.AfficherCarte();
            Console.WriteLine($"\n✅ '{titre}' a été placée avec succès.");
        }
        else
        {
            Console.WriteLine($"❌ Impossible de placer '{titre}'.");
            Console.WriteLine("🛠️ Analyse des raisons :");
            foreach (var chambre in musee.Chambres)
            {
                chambre.IsCompatibleDebug(oeuvre);
            }
        }

        PauseAvantRetour();
    }

    // Demande un entier à l'utilisateur avec validation
    private int DemanderEntier(string question)
    {
        Console.Write(question);
        string saisie = Console.ReadLine();
        int valeur;

        while (!int.TryParse(saisie, out valeur))
        {
            Console.Write("⛔ Entrée invalide. Entrez un nombre entier : ");
            saisie = Console.ReadLine();
        }

        return valeur;
    }

    // Affiche un message d'attente pour revenir au menu
    private void PauseAvantRetour()
    {
        Console.WriteLine();
        Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
        Console.ReadKey(true);
    }
}
