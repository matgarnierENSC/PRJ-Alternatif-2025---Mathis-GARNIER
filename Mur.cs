// Type énuméré représentant la couleur d’un mur
public enum CouleurMur
{
    Chaude,   // Murs aux couleurs chaudes (ex : rouge, orange)
    Froide    // Murs aux couleurs froides (ex : bleu, vert)
}

public class Mur
{
    public CouleurMur Couleur { get; }           // Couleur dominante du mur
    public List<Zone> Zones { get; }             // Liste des zones sur ce mur (ZoneMurale ou Vitrine)

    public Mur(CouleurMur couleur, List<Zone> zones)
    {
        Couleur = couleur;
        Zones = zones;
    }

    // Vérifie si au moins une zone du mur est compatible avec l’œuvre
    public bool EstCompatible(Oeuvre oeuvre)
    {
        foreach (var zone in Zones)
        {
            if (zone.EstCompatible(oeuvre))
            {
                return true;
            }
        }
        return false;
    }
}
