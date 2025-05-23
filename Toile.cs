// Représente une œuvre de type toile, avec des propriétés spécifiques
public class Toile : Oeuvre
{
    public bool ForteLumiere { get; }      // Indique si la toile supporte une forte lumière
    public bool CouleurChaude { get; }     // Indique si la toile est composée de couleurs chaudes

    public Toile(string titre, int largeur, int hauteur, bool forteLumiere, bool couleurChaude)
        : base(titre, largeur, hauteur)
    {
        // Vérifie que les dimensions sont valides
        if (largeur <= 0)
            throw new ArgumentException("La largeur doit être strictement positive !");
        
        if (hauteur <= 0)
            throw new ArgumentException("La hauteur doit être strictement positive !");

        ForteLumiere = forteLumiere;
        CouleurChaude = couleurChaude;
    }

    // Affiche les détails de la toile sous forme textuelle
    public override string ToString()
    {
        string lumiere = ForteLumiere ? "oui" : "non";
        string couleur = CouleurChaude ? "chaude" : "froide";
        return $"Toile : {Titre} | Taille : {Largeur}x{Hauteur} | Exposition à la lumière : {lumiere} | Couleur  : {couleur}";
    }
}
