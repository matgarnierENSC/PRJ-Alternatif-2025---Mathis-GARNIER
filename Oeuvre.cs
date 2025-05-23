// Classe abstraite représentant une œuvre exposable dans le musée
public abstract class Oeuvre
{
    public string Titre { get; }       // Titre de l'œuvre
    public int Largeur { get; }        // Largeur en cm
    public int Hauteur { get; }        // Hauteur en cm

    public Zone Zone { get; set; }     // Zone où l'œuvre est exposée (peut être null)

    // Constructeur de base
    public Oeuvre(string titre, int largeur, int hauteur)
    {
        Titre = titre;
        Largeur = largeur;
        Hauteur = hauteur;
    }
}
