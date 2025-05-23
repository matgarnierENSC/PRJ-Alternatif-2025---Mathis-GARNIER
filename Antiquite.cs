// Classe représentant une Antiquité, héritée de la classe Oeuvre
public class Antiquite : Oeuvre
{
    // Poids de l'antiquité en kilogrammes
    public int Poids { get; }

    // Constructeur avec validation du poids
    public Antiquite(string nom, int largeur, int hauteur, int poids)
        : base(nom, largeur, hauteur)
    {
        // Vérifie que le poids est dans une plage raisonnable
        if (poids <= 0 || poids > 1000)
            throw new ArgumentOutOfRangeException(nameof(poids), "Poids invalide.");
        Poids = poids;
    }

    // Redéfinition de ToString pour afficher les infos de l'antiquité
    public override string ToString()
    {
        return $"Antiquité : {Titre} | Poids : {Poids} kg";
    }
}
